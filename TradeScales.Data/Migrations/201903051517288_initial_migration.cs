namespace TradeScales.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Destination",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Driver",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Error",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Haulier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StatusMessage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastModifiedBy = c.String(nullable: false),
                        Status = c.String(nullable: false),
                        TicketNumber = c.String(nullable: false, maxLength: 50),
                        TimeIn = c.String(),
                        TimeOut = c.String(),
                        LastModified = c.String(),
                        HaulierId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        OrderNumber = c.String(nullable: false),
                        DeliveryNumber = c.String(nullable: false),
                        SealFrom = c.String(),
                        SealTo = c.String(),
                        GrossWeight = c.Double(nullable: false),
                        TareWeight = c.Double(nullable: false),
                        NettWeight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Destination", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.Driver", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Haulier", t => t.HaulierId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.HaulierId)
                .Index(t => t.CustomerId)
                .Index(t => t.DestinationId)
                .Index(t => t.ProductId)
                .Index(t => t.DriverId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.Vehicle",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Make = c.String(nullable: false, maxLength: 50),
                        Registration = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 200),
                        HashedPassword = c.String(nullable: false, maxLength: 200),
                        Salt = c.String(nullable: false, maxLength: 200),
                        IsLocked = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Ticket", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.Ticket", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Ticket", "HaulierId", "dbo.Haulier");
            DropForeignKey("dbo.Ticket", "DriverId", "dbo.Driver");
            DropForeignKey("dbo.Ticket", "DestinationId", "dbo.Destination");
            DropForeignKey("dbo.Ticket", "CustomerId", "dbo.Customer");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.Ticket", new[] { "VehicleId" });
            DropIndex("dbo.Ticket", new[] { "DriverId" });
            DropIndex("dbo.Ticket", new[] { "ProductId" });
            DropIndex("dbo.Ticket", new[] { "DestinationId" });
            DropIndex("dbo.Ticket", new[] { "CustomerId" });
            DropIndex("dbo.Ticket", new[] { "HaulierId" });
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.Vehicle");
            DropTable("dbo.Ticket");
            DropTable("dbo.StatusMessage");
            DropTable("dbo.Role");
            DropTable("dbo.Product");
            DropTable("dbo.Haulier");
            DropTable("dbo.Error");
            DropTable("dbo.Driver");
            DropTable("dbo.Destination");
            DropTable("dbo.Customer");
        }
    }
}
