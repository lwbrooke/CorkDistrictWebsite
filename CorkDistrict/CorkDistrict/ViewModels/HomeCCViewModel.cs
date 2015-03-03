using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorkDistrict.ViewModels
{
    public class HomeCCViewModel
    {
        [Required]
        [Display(Name = "Tasting Card Number")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(6, ErrorMessage = "Tasting Card numbers are 6 digits", MinimumLength = 6)]
        public string CardID { get; set; } // what card
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Only english letters are allowed")]
        public string FirstName { get; set; } // billing info
        [Display(Name = "Middle Initial")]
        [RegularExpression("[A-Za-z]", ErrorMessage = "Only english letters are allowed")]
        [StringLength(1)]
        public string MiddleName { get; set; } // billing info
        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Only english letters are allowed")]
        public string LastName { get; set; } // billing info
        [Required]
        [Display(Name = "Address Line 1")]
        [RegularExpression("[A-Za-z\\d\\s]+", ErrorMessage = "Only alphanumeric characters are allowed")]
        public string AddressLine1 { get; set; } // billing info
        [Display(Name = "Address Line 2")]
        [RegularExpression("[A-Za-z\\d\\s]+", ErrorMessage = "Only alphanumeric characters are allowed")]
        public string AddressLine2 { get; set; } // billing info
        [Required]
        [RegularExpression("[A-Za-z\\s]+", ErrorMessage = "Only english letters are allowed")]
        public string City { get; set; } // billing info
        [Required]
        public string State { get; set; } // billing info
        [Required]
        [Display(Name = "Zip Code")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(5, ErrorMessage = "Zipcodes must be 5 digits", MinimumLength = 5)]
        public string ZipCode { get; set; } // billing info
        [Required]
        [RegularExpression("[A-Za-z\\d\\s]+", ErrorMessage = "Only alphanumeric characters are allowed")]
        public string Country { get; set; } // billing info
        [Required]
        [Display(Name = "Credit Card Number")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(16, MinimumLength = 15, ErrorMessage = "Credit cards must be between 15 and 16 digits")]
        public string CreditCardNum { get; set; } // billing info
        [Required]
        [Display(Name = "Expiration Month")]
        public string ExpirationMonth { get; set; } // billing info
        [Required]
        [Display(Name = "Expiration Year")]
        public string ExpirationYear { get; set; } // billing info
        [Required]
        [Display(Name = "CVC/Security Code")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Security codes must be 3 or 4 digits")]
        public string CVC { get; set; } // billing info
    }
}