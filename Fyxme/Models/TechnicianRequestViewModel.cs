using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web;
using System.Web.Mvc;

namespace Fyxme.Models
{
    public class TechnicianRequestViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 12 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Mobile phone (or home phone) is required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The phone number is not valid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Resume is required")]
        public string Resume { get; set; }
    }
}

