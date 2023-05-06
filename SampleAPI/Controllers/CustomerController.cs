using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using SampleAPI.Models;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SampleAPI.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        [Route("api/v1/createCustomer")]
        public HttpResponseMessage createCustomer([FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("dbConString")))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO Customer (Username, Email, FirstName, LastName, CreatedOn, IsActive) OUTPUT INSERTED.UserId " +
                                        "VALUES (@Username, @Email, @FirstName, @LastName, @CreatedOn, @IsActive);";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Username", customer.Username);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);
                    con.Open();
                    customer.UserId = (Guid)cmd.ExecuteScalar();
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }


        [HttpGet]
        [Route("api/v1/getAllCustomers")]
        public List<Customer> getAllCustomers()
        {
            List<Customer> customerList = new List<Customer>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("dbConString")))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer;", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.UserId = (Guid)reader["UserId"];
                        customer.Username = reader["Username"].ToString();
                        customer.Email = reader["Email"].ToString();
                        customer.FirstName = reader["FirstName"].ToString();
                        customer.LastName = reader["LastName"].ToString();
                        customer.CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString());
                        customer.IsActive = Convert.ToInt32(reader["IsActive"]);

                        customerList.Add(customer);
                    }
                }
            }
            return customerList;
        }


        [HttpPut]
        [Route("api/v1/updateCustomer")]
        public HttpResponseMessage updateCustomer([FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("dbConString")))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "UPDATE Customer SET Username = @Username, Email = @Email, FirstName = @FirstName, LastName = @LastName, " +
                                        "CreatedOn = @CreatedOn, IsActive = @IsActive WHERE UserId = @UserId;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserId", customer.UserId);
                    cmd.Parameters.AddWithValue("@Username", customer.Username);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }


        [HttpDelete]
        [Route("api/v1/deleteCustomer")]
        public HttpResponseMessage deleteCustomer([FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("dbConString")))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Customer WHERE UserId = @UserId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserId", customer.UserId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }


    }
}
