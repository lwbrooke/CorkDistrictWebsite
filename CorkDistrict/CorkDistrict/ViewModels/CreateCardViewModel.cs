using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class CreateCardViewModel
    {
        [Required]
        [Display(Name = "Number of Cards to be added?")]
        [Range(1, 100000, ErrorMessage = "Must add between 1 and 100,000 cards")]
        public int Amount { get; set; }
        [Required]
        [Display(Name = "Promotional Cards?")]
        public bool IsPromo { get; set; }
        [RegularExpression("promo [A-Za-z\\d]+", ErrorMessage = "Only alphanumeric characters are allowed, and must start with 'promo '")]
        [Display(Name = "If Promotional, Which Vendor?")]
        public string PromoOwner { get; set; }
    }
}