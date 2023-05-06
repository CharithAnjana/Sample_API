using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Models
{
    public class Supplier
    {
        public Guid SupplierId { get; set; }

        [StringLength(50)]
        public string SupplierName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int IsActive { get; set; }
    }
}
