using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class RedeemViewModel
    {
        [Required]
        [Display(Name = "Tasting Card Number")]
        [RegularExpression("\\d+", ErrorMessage = "Only numbers are allowed")]
        [StringLength(6, ErrorMessage = "Tasting Card numbers are 6 digits", MinimumLength = 6)]
        public string CardID { get; set; } // what card
    }
}