namespace SampleAPI.Models
{
    public class Product
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public string SupplierId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int IsActive { get; set; }

        public Supplier supplier { get; set; }
    }
}
