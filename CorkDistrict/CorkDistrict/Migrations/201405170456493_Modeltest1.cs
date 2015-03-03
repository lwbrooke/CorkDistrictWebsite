namespace CorkDistrict.Migrations.MigrationsModels
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modeltest1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchases", "CardID", "dbo.Cards");
            DropForeignKey("dbo.Redemptions", "CardID", "dbo.Cards");
            DropForeignKey("dbo.Activations", "CardID", "dbo.Cards");
            DropIndex("dbo.Purchases", new[] { "CardID" });
            DropIndex("dbo.Redemptions", new[] { "CardID" });
            DropIndex("dbo.Activations", new[] { "CardID" });
            AddColumn("dbo.Cards", "CardID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Cards");
            AddPrimaryKey("dbo.Cards", "CardID");
            CreateIndex("dbo.Purchases", "CardID");
            CreateIndex("dbo.Redemptions", "CardID");
            CreateIndex("dbo.Activations", "CardID");
            AddForeignKey("dbo.Purchases", "CardID", "dbo.Cards", "CardID");
            AddForeignKey("dbo.Redemptions", "CardID", "dbo.Cards", "CardID", cascadeDelete: true);
            AddForeignKey("dbo.Activations", "CardID", "dbo.Cards", "CardID");
            DropColumn("dbo.Cards", "ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Activations", "CardID", "dbo.Cards");
            DropForeignKey("dbo.Redemptions", "CardID", "dbo.Cards");
            DropForeignKey("dbo.Purchases", "CardID", "dbo.Cards");
            DropIndex("dbo.Activations", new[] { "CardID" });
            DropIndex("dbo.Redemptions", new[] { "CardID" });
            DropIndex("dbo.Purchases", new[] { "CardID" });
            DropPrimaryKey("dbo.Cards");
            AddPrimaryKey("dbo.Cards", "ID");
            DropColumn("dbo.Cards", "CardID");
            CreateIndex("dbo.Activations", "CardID");
            CreateIndex("dbo.Redemptions", "CardID");
            CreateIndex("dbo.Purchases", "CardID");
            AddForeignKey("dbo.Activations", "CardID", "dbo.Cards", "ID");
            AddForeignKey("dbo.Redemptions", "CardID", "dbo.Cards", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Purchases", "CardID", "dbo.Cards", "ID");
        }
    }
}
