using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorkDistrict.ViewModels
{
    public class AggregateCardStatsViewModel
    {
        [Required]
        [Display(Name = "Total Cards")]
        public int Count { get; set; }

        [Required]
        [Display(Name = "Active Cards")]
        public int ActiveCount { get; set; }

        [Required]
        [Display(Name = "Cards with Uses Remaining")]
        public int UsesRemaining { get; set; }

        [Required]
        [Display(Name = "Activations in Month/Year")]
        public int NewActives { get; set; }
    }
}