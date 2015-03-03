namespace CorkDistrict.Migrations.MigrationsModels
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CashAddition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashPurchases",
                c => new
                    {
                        CardID = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        WineryID = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CardID)
                .ForeignKey("dbo.Cards", t => t.CardID)
                .Index(t => t.CardID);
            
            AlterColumn("dbo.Purchases", "MiddleName", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashPurchases", "CardID", "dbo.Cards");
            DropIndex("dbo.CashPurchases", new[] { "CardID" });
            AlterColumn("dbo.Purchases", "MiddleName", c => c.String());
            DropTable("dbo.CashPurchases");
        }
    }
}
