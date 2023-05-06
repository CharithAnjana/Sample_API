using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Models;
using System.Data.SqlClient;

namespace SampleAPI.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        [Route("api/v1/getActiveOrdersByCustomer/{customerId}")]
        public List<Order> getActiveOrdersByCustomer(Guid customerId)
        {
            List<Order> orderList = new List<Order>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("dbConString")))
            {
                SqlCommand cmd = new SqlCommand("getActiveOrdersByCustomer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Order order = new Order();
                        Product product = new Product();
                        Supplier supplier = new Supplier();

                        order.OrderId = (Guid)reader["OrderId"];
                        order.ProductId = (Guid)reader["ProductId"];
                        order.OrderStatus = Convert.ToInt32(reader["OrderStatus"]);
                        order.OrderType = Convert.ToInt32(reader["OrderType"]);
                        order.OrderBy = (Guid)reader["OrderBy"];
                        order.OrderedOn = DateTime.Parse(reader["OrderedOn"].ToString());
                        order.ShippedOn = DateTime.Parse(reader["ShippedOn"].ToString());
                        order.IsActive = Convert.ToInt32(reader["OrderIsActive"]);

                        product.ProductId = (Guid)reader["ProductId"];
                        product.ProductName = reader["ProductName"].ToString();
                        product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                        product.SupplierId = (Guid)reader["SupplierId"];
                        product.CreatedOn = DateTime.Parse(reader["PrdCreatedOn"].ToString());
                        product.IsActive = Convert.ToInt32(reader["PrdIsActive"]);
                        order.product = product;

                        supplier.SupplierId = (Guid)reader["SupplierId"];
                        supplier.SupplierName = reader["SupplierName"].ToString();
                        supplier.CreatedOn = DateTime.Parse(reader["SupCreatedOn"].ToString());
                        supplier.IsActive = Convert.ToInt32(reader["SupIsAcive"]);
                        product.supplier = supplier;


                        orderList.Add(order);
                    }
                }
            }
            return orderList;
        }
    }
}
