using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public Guid SupplierId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int IsActive { get; set; }

        public Supplier supplier { get; set; }
    }
}
