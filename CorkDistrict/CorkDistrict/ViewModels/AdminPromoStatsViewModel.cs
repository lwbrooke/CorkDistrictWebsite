using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class AdminPromoStatsViewModel
    {
        [Required]
        [Display(Name = "Card Number")]
        public int CardID { get; set; }

        [Required]
        [Display(Name = "Visits Remaining")]
        public int remainingUses { get; set; }

        [Required]
        [Display(Name = "Issued By")]
        public string vendorIssuer { get; set; }

        [Required]
        [Display(Name = "Visted Wineries")]
        [UIHint("Array")]
        public string[] locationsUsed { get; set; }


    }
}