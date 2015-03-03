using CorkDistrict.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class CCViewModel
    {
        [Required]
        [Display(Name = "Tasting Card Number")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(6, ErrorMessage = "Tasting Card numbers are 6 digits", MinimumLength = 6)]
        public string CardID { get; set; } // what card
        [Required]
        [Display(Name = "Customer Email")]
        [RegularExpression("\\b[a-zA-Z0-9._-]+@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,4}\\b", ErrorMessage = "Invalid Email Address Format")] // might need revising
        public string Email { get; set; }
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
        [Required(ErrorMessage = "The Card Expiration Month is Required")]
        [Display(Name = "Expiration Month")]
        public string ExpirationMonth { get; set; } // billing info
        [Required(ErrorMessage = "The Card Expiration Year is Required")]
        [Display(Name = "Expiration Year")]
        public string ExpirationYear { get; set; } // billing info
        [Required]
        [Display(Name = "CVC/Security Code")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Security codes must be 3 or 4 digits")]
        public string CVC { get; set; } // billing info

        public ApplicationUser GetUser()
        {
            var Db = new ApplicationDbContext();
            var users = Db.Users;
            foreach (var user in users)
            {
                if (user.Email.Equals(this.Email)) return user;
            }

            var idManager = new IdentityManager();
            var newUser = new ApplicationUser 
                {
                    UserName = this.Email.Substring(0, this.Email.IndexOf("@")) + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day, 
                    Name = "Change Me", 
                    Email = this.Email 
                };
            idManager.CreateUser(newUser, "password");
            idManager.AddUserToRole(newUser.Id, "User");

            foreach (var user in Db.Users)
            {
                if (newUser.UserName.Equals(user.UserName)) return user;
            }

            return null;
        }
    }
}