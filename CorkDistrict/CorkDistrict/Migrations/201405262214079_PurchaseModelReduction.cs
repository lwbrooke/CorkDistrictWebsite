namespace CorkDistrict.Migrations.MigrationsModels
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseModelReduction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "TimeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Purchases", "Location", c => c.String(nullable: false));
            DropColumn("dbo.Purchases", "FirstName");
            DropColumn("dbo.Purchases", "MiddleName");
            DropColumn("dbo.Purchases", "LastName");
            DropColumn("dbo.Purchases", "AddressLine1");
            DropColumn("dbo.Purchases", "AddressLine2");
            DropColumn("dbo.Purchases", "City");
            DropColumn("dbo.Purchases", "State");
            DropColumn("dbo.Purchases", "ZipCode");
            DropColumn("dbo.Purchases", "Country");
            DropColumn("dbo.Purchases", "ExpirationMonth");
            DropColumn("dbo.Purchases", "ExpirationYear");
            DropColumn("dbo.Purchases", "CVC");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "CVC", c => c.Int(nullable: false));
            AddColumn("dbo.Purchases", "ExpirationYear", c => c.Int(nullable: false));
            AddColumn("dbo.Purchases", "ExpirationMonth", c => c.Int(nullable: false));
            AddColumn("dbo.Purchases", "Country", c => c.String(nullable: false));
            AddColumn("dbo.Purchases", "ZipCode", c => c.Int(nullable: false));
            AddColumn("dbo.Purchases", "State", c => c.String(nullable: false));
            AddColumn("dbo.Purchases", "City", c => c.String(nullable: false));
            AddColumn("dbo.Purchases", "AddressLine2", c => c.String());
            AddColumn("dbo.Purchases", "AddressLine1", c => c.String(nullable: false));
            AddColumn("dbo.Purchases", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.Purchases", "MiddleName", c => c.String(maxLength: 1));
            AddColumn("dbo.Purchases", "FirstName", c => c.String(nullable: false));
            DropColumn("dbo.Purchases", "Location");
            DropColumn("dbo.Purchases", "TimeStamp");
        }
    }
}
