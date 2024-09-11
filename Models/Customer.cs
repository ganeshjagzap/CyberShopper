using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(15)] // Assuming the length should be 15 based on main changes
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string DeliveryAddress { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
