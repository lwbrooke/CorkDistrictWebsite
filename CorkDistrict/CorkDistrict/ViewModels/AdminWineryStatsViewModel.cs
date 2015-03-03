using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class AdminWineryStatsViewModel
    {
        [Required]
        [Display(Name = "Winery Name")]
        public string siteID { get; set; }

        [Required]
        [Display(Name = "Cards Sold")]
        public int soldCards { get; set; }

        [Required]
        [Display(Name = "Promos Redeemed")]
        public int promosRedeemed { get; set; }

        [Required]
        [Display(Name = "Total Redemptions")]
        public int totalRedeemed { get; set; }

    }
}



    
