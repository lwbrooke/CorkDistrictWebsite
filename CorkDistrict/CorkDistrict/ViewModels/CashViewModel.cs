using CorkDistrict.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class CashViewModel
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

        public ApplicationUser GetUser()
        {
            var Db = new ApplicationDbContext();
            var users = Db.Users;
            foreach(var user in users)
            {
                if (user.Email.ToLower().Equals(this.Email.ToLower())) return user;
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