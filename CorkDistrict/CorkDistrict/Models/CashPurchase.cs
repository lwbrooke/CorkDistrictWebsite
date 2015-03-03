using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CorkDistrict.Models
{
    public class CashPurchase
    {
        [Key]
        [ForeignKey("Card")]
        [Display(Name = "Tasting Card Number")]
        public int CardID { get; set; } // what card
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; } // when payed
        [Required]
        [Display(Name = "WineryID")]
        public string WineryID { get; set; } // where purchased


        public virtual Card Card { get; set; }
    }
}