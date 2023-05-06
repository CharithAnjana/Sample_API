namespace SampleAPI.Models
{
    public class Order
    {
        public string OrderId { get; set; }

        public string ProductId { get; set; }

        public int OrderStatus { get; set; }

        public int OrderType { get; set; }

        public string OrderBy { get; set; }

        public DateTime OrderedOn { get; set; }

        public DateTime ShippedOn { get; set; }

        public int IsActive { get; set; }

        public Product product { get; set; }

        public Customer customer { get; set; }
    }
}
