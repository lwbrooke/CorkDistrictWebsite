using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class RedemptionIndexViewModel
    {
        [Display(Name = "Tasting Card Number")]
        public int CardID { get; set; }
        [Display(Name = "Owner")]
        public string owner { get; set; }
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; } // when redeemed
        [Display(Name = "Redeemed At")]
        public string WineryID { get; set; } // where redeemed
        [Display(Name = "Uses Remaining")]
        public int uses { get; set; }
    }
}