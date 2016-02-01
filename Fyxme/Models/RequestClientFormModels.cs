using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fyxme.Models
{
    public class Request
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile phone (or home phone) is required")]
        public string MobilePhone { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "The damage description can't exceed 500 caracters")]
        [StringLength(500)]
        public string DamageDescription { get; set; }
    }
}
