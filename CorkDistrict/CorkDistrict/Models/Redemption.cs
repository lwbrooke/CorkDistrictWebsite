using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorkDistrict.Models
{
    public class Redemption
    {
        [Key]
        public int ID { get; set; } // redemption id
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; } // when redeemed
        public string WineryID { get; set; } // where redeemed
        [Required]
        [ForeignKey("Card")]
        [Display(Name = "Card Number")]
        public int CardID { get; set; } // what card

        public virtual Card Card { get; set; }
    }
}