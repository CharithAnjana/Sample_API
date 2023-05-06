using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Models
{
    public class Customer
    {
        public Guid UserId { get; set; }

        [StringLength(30)]
        public string Username { get; set; }

        [StringLength(20)]
        public string Email { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int IsActive { get; set; }
    }
}
