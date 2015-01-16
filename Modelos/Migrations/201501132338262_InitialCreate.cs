namespace Modelos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locks",
                c => new
                    {
                        IdLock = c.Int(nullable: false, identity: true),
                        IdUser = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdLock)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        email = c.String(nullable: false, maxLength: 50),
                        username = c.String(nullable: false, maxLength: 50),
                        password = c.String(nullable: false, maxLength: 50),
                        reference_id = c.Int(nullable: false),
                        locked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdUser);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        IdLogin = c.Int(nullable: false, identity: true),
                        IdUser = c.Int(nullable: false),
                        IdStatus = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        ip_address = c.String(),
                        browser = c.String(),
                    })
                .PrimaryKey(t => t.IdLogin)
                .ForeignKey("dbo.Status", t => t.IdStatus, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdStatus);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        IdStatus = c.Int(nullable: false, identity: true),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.IdStatus);
            
            CreateTable(
                "dbo.Rols",
                c => new
                    {
                        IdRol = c.Int(nullable: false, identity: true),
                        reference_id = c.Int(nullable: false),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.IdRol);
            
            CreateTable(
                "dbo.Rol_User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRol = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rols", t => t.IdRol, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdRol)
                .Index(t => t.IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rol_User", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Rol_User", "IdRol", "dbo.Rols");
            DropForeignKey("dbo.Logins", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Logins", "IdStatus", "dbo.Status");
            DropForeignKey("dbo.Locks", "IdUser", "dbo.Users");
            DropIndex("dbo.Rol_User", new[] { "IdUser" });
            DropIndex("dbo.Rol_User", new[] { "IdRol" });
            DropIndex("dbo.Logins", new[] { "IdStatus" });
            DropIndex("dbo.Logins", new[] { "IdUser" });
            DropIndex("dbo.Locks", new[] { "IdUser" });
            DropTable("dbo.Rol_User");
            DropTable("dbo.Rols");
            DropTable("dbo.Status");
            DropTable("dbo.Logins");
            DropTable("dbo.Users");
            DropTable("dbo.Locks");
        }
    }
}
