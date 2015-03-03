namespace CorkDistrict.Migrations.MigrationsModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CorkDistrict.Models;
    using System.Collections.Generic;

    internal sealed class ModelConfig : DbMigrationsConfiguration<CorkDistrict.DAL.CorkDistrictContext>
    {
        public ModelConfig()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CorkDistrict.DAL.CorkDistrictContext context)
        {
            var cards = new List<Card>();

            for(int i = 0; i < 10; i++)
            {
                cards.Add(new Card { CardID = 140000 + i, Created = DateTime.Today, Uses = 3, isPromo = i < 5 });
            }

            cards.ForEach(c => context.Cards.AddOrUpdate(p => p.CardID, c));
            context.SaveChanges();
        }
    }
}
