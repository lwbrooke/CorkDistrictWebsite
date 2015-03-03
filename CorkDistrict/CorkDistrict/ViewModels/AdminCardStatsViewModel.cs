using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class AdminCardStatsViewModel
    {
                
        [Required]
        [Display(Name = "Card Number")]
        public int CardID { get; set; }

        [Required]
        [Display(Name = "Uses Left")]
        public int remainingUses { get; set; }

        [Required]
        [Display(Name = "Purchased At")]
        public string purchasedAt { get; set; }

        [Required]
        [Display(Name = "Owner")]
        public string owner { get; set; }

        [Required]
        [Display(Name = "Visted Wineries")]
        [UIHint("Array")]
        public string[] vistedWineries { get; set; }

    }
}