namespace CorkDistrict.Migrations.MigrationsModels
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchanges5162014 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activations",
                c => new
                    {
                        CardID = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        UserID = c.String(),
                    })
                .PrimaryKey(t => t.CardID)
                .ForeignKey("dbo.Cards", t => t.CardID)
                .Index(t => t.CardID);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Uses = c.Int(nullable: false),
                        isPromo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        CardID = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(),
                        LastName = c.String(nullable: false),
                        AddressLine1 = c.String(nullable: false),
                        AddressLine2 = c.String(),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Country = c.String(nullable: false),
                        CreditCardNum = c.String(nullable: false),
                        ExpirationMonth = c.Int(nullable: false),
                        ExpirationYear = c.Int(nullable: false),
                        CVC = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardID)
                .ForeignKey("dbo.Cards", t => t.CardID)
                .Index(t => t.CardID);
            
            CreateTable(
                "dbo.Redemptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        WineryID = c.String(),
                        CardID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cards", t => t.CardID, cascadeDelete: true)
                .Index(t => t.CardID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activations", "CardID", "dbo.Cards");
            DropForeignKey("dbo.Redemptions", "CardID", "dbo.Cards");
            DropForeignKey("dbo.Purchases", "CardID", "dbo.Cards");
            DropIndex("dbo.Activations", new[] { "CardID" });
            DropIndex("dbo.Redemptions", new[] { "CardID" });
            DropIndex("dbo.Purchases", new[] { "CardID" });
            DropTable("dbo.Redemptions");
            DropTable("dbo.Purchases");
            DropTable("dbo.Cards");
            DropTable("dbo.Activations");
        }
    }
}
