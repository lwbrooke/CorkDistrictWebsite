using CorkDistrict.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CorkDistrict.DAL
{
    public class CorkDistrictContext : DbContext
    {
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<CashPurchase> CashPurchases { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Activation> Activations { get; set; }
    }
}