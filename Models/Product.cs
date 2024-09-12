using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        [StringLength(50)]
        public string ModelNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string ModelName { get; set; }

        [Required]
        public decimal UnitCost { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(2048)]
        public string ImageUrl { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
