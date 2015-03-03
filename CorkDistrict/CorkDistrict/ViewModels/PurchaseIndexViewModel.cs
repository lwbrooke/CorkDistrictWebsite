using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class PurchaseIndexViewModel
    {
        [Required]
        [Display(Name = "Tasting Card Number")]
        public int CardID { get; set; }
        [Required]
        [Display(Name = "Uses Left")]
        public int uses { get; set; }
        [Required]
        [Display(Name = "Owner")]
        public string owner { get; set; }
        [Required]
        [Display(Name = "Where Purchased")]
        public string purchaseLocation { get; set; }
        [Required]
        [Display(Name = "When Purchased")]
        public DateTime timestamp { get; set; }
        [Required]
        [Display(Name = "Purchase Method")]
        public string method { get; set; }
    }
}