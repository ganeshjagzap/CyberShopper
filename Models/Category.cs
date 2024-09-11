using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
