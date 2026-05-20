namespace MilAventures.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        id_activity = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 150),
                        description = c.String(),
                        init_date = c.DateTime(nullable: false),
                        end_date = c.DateTime(nullable: false),
                        difficulty = c.Int(nullable: false),
                        max_participants = c.Int(nullable: false),
                        start_end_point = c.String(),
                        id_category = c.Int(nullable: false),
                        id_guide = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_activity)
                .ForeignKey("dbo.Category", t => t.id_category, cascadeDelete: true)
                .ForeignKey("dbo.Guide", t => t.id_guide, cascadeDelete: true)
                .Index(t => t.id_category)
                .Index(t => t.id_guide);
            
            CreateTable(
                "dbo.BookingLine",
                c => new
                    {
                        id_line = c.Int(nullable: false, identity: true),
                        quantity = c.Int(nullable: false),
                        price_at_moment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bookingId = c.Int(nullable: false),
                        activityId = c.Int(),
                        equipmentId = c.Int(),
                    })
                .PrimaryKey(t => t.id_line)
                .ForeignKey("dbo.Activity", t => t.activityId)
                .ForeignKey("dbo.Booking", t => t.bookingId, cascadeDelete: true)
                .ForeignKey("dbo.Equipment", t => t.equipmentId)
                .Index(t => t.bookingId)
                .Index(t => t.activityId)
                .Index(t => t.equipmentId);
            
            CreateTable(
                "dbo.Booking",
                c => new
                    {
                        id_booking = c.Int(nullable: false, identity: true),
                        created_at = c.DateTime(nullable: false),
                        total_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        participants = c.Int(nullable: false),
                        notes = c.String(),
                        id_client = c.Int(nullable: false),
                        id_book_status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_booking)
                .ForeignKey("dbo.BookingStatus", t => t.id_book_status, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.id_client, cascadeDelete: true)
                .Index(t => t.id_client)
                .Index(t => t.id_book_status);
            
            CreateTable(
                "dbo.BookingStatus",
                c => new
                    {
                        id_book_status = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.id_book_status);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        id_client = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                        surname = c.String(nullable: false, maxLength: 100),
                        email = c.String(maxLength: 150),
                        phone = c.String(maxLength: 30),
                        photo = c.String(),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_client);
            
            CreateTable(
                "dbo.Equipment",
                c => new
                    {
                        id_equipment = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 150),
                        description = c.String(),
                        price_per_day = c.Decimal(nullable: false, precision: 18, scale: 2),
                        units = c.Int(nullable: false),
                        min_stock = c.Int(nullable: false),
                        id_category = c.Int(nullable: false),
                        id_status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_equipment)
                .ForeignKey("dbo.Category", t => t.id_category, cascadeDelete: true)
                .ForeignKey("dbo.EquipmentStatus", t => t.id_status, cascadeDelete: true)
                .Index(t => t.id_category)
                .Index(t => t.id_status);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        id_category = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.id_category);
            
            CreateTable(
                "dbo.EquipmentStatus",
                c => new
                    {
                        id_status = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.id_status);
            
            CreateTable(
                "dbo.Guide",
                c => new
                    {
                        id_guide = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                        surname = c.String(nullable: false, maxLength: 100),
                        email = c.String(maxLength: 150),
                        phone = c.String(maxLength: 30),
                        photo = c.String(),
                        specialty = c.String(maxLength: 150),
                        credentials = c.String(),
                        experience_level = c.String(maxLength: 50),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_guide);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activity", "id_guide", "dbo.Guide");
            DropForeignKey("dbo.Equipment", "id_status", "dbo.EquipmentStatus");
            DropForeignKey("dbo.Equipment", "id_category", "dbo.Category");
            DropForeignKey("dbo.Activity", "id_category", "dbo.Category");
            DropForeignKey("dbo.BookingLine", "equipmentId", "dbo.Equipment");
            DropForeignKey("dbo.Booking", "id_client", "dbo.Client");
            DropForeignKey("dbo.Booking", "id_book_status", "dbo.BookingStatus");
            DropForeignKey("dbo.BookingLine", "bookingId", "dbo.Booking");
            DropForeignKey("dbo.BookingLine", "activityId", "dbo.Activity");
            DropIndex("dbo.Equipment", new[] { "id_status" });
            DropIndex("dbo.Equipment", new[] { "id_category" });
            DropIndex("dbo.Booking", new[] { "id_book_status" });
            DropIndex("dbo.Booking", new[] { "id_client" });
            DropIndex("dbo.BookingLine", new[] { "equipmentId" });
            DropIndex("dbo.BookingLine", new[] { "activityId" });
            DropIndex("dbo.BookingLine", new[] { "bookingId" });
            DropIndex("dbo.Activity", new[] { "id_guide" });
            DropIndex("dbo.Activity", new[] { "id_category" });
            DropTable("dbo.Guide");
            DropTable("dbo.EquipmentStatus");
            DropTable("dbo.Category");
            DropTable("dbo.Equipment");
            DropTable("dbo.Client");
            DropTable("dbo.BookingStatus");
            DropTable("dbo.Booking");
            DropTable("dbo.BookingLine");
            DropTable("dbo.Activity");
        }
    }
}
