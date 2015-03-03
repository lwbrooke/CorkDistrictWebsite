using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorkDistrict.Models
{
    public class Card
    {
        [Key]
        [Display(Name = "Card Number")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CardID { get; set; } // what card
        public DateTime Created { get; set; } // when created
        [Range(0, 3)]
        public int Uses { get; set; } // how many uses left
        [Required]
        [Display(Name = "Promotional Card?")]
        public bool isPromo { get; set; } // is a promotional card

        public virtual Activation Activation { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual CashPurchase CashPurchase { get; set; }
        public virtual ICollection<Redemption> Redemption { get; set; }
    }
}