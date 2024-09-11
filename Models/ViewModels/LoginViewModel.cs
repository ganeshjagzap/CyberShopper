using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels { 
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}   
