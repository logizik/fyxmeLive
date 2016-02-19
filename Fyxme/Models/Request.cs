using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web;
using System.Web.Mvc;

namespace Fyxme.Models
{
    public class Request
    {
        public int RequestId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile phone (or home phone) is required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The phone number is not valid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        public string ZipCode { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string Origin { get; set; }

        /*[Required(ErrorMessage = "Car brand is required")]*/
        public string SelectedCarMakerId { get; set; }
        public IEnumerable<SelectListItem> DDListCarMakers { get; set; }

        [Required(ErrorMessage = "Car model is required")]
        public string SelectedCarModelId { get; set; }
        public IEnumerable<SelectListItem> DDListCarModels { get; set; }

        [Required(ErrorMessage = "Car year is required")]
        public string SelectedCarYearId { get; set; }
        public IEnumerable<SelectListItem> DDListCarYears { get; set; }

        public List<HttpPostedFileBase> UploadedCarImages { get; set; }

        [MaxLength(500, ErrorMessage = "The damage description can't exceed 500 caracters")]
        public string DamageDescription { get; set; }
    }
}

