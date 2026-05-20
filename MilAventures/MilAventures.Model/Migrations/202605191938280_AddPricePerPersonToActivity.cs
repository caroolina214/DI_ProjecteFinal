namespace MilAventures.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPricePerPersonToActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "price_per_person", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activity", "price_per_person");
        }
    }
}
