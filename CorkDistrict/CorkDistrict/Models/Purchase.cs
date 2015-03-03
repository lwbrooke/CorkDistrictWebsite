using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorkDistrict.Models
{
    public class Purchase
    {
        [Key]
        [ForeignKey("Card")]
        [Display(Name = "Tasting Card Number")]
        public int CardID { get; set; } // what card
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; } // when purchased
        [Required]
        [Display(Name = "Location of Purchase")]
        public string Location { get; set; } // where purchased
        [Required]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNum { get; set; } // billing info

        public virtual Card Card { get; set; }

    }
}