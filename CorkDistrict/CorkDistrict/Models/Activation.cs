using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorkDistrict.Models
{
    public class Activation
    {
        [Key]
        [ForeignKey("Card")]
        [Display(Name = "Card Number")]
        public int CardID { get; set; } // what card
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; } // when activated
        [Display(Name = "User ID")]
        public string UserID { get; set; } // who owns

        public virtual Card Card { get; set; }
    }
}