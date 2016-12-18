namespace CarritoSosua.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Localidads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Turnoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        TimeFrom = c.DateTime(nullable: false),
                        TimeTo = c.DateTime(nullable: false),
                        LocalidadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localidads", t => t.LocalidadId, cascadeDelete: true)
                .Index(t => t.LocalidadId);
            
            CreateTable(
                "dbo.PublicadorTurnoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PublicadorId = c.Int(nullable: false),
                        TurnoId = c.Int(nullable: false),
                        Day = c.DateTime(nullable: false),
                        Disponible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publicadors", t => t.PublicadorId, cascadeDelete: true)
                .ForeignKey("dbo.Turnoes", t => t.TurnoId, cascadeDelete: true)
                .Index(t => t.PublicadorId)
                .Index(t => t.TurnoId);
            
            CreateTable(
                "dbo.Publicadors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublicadorTurnoes", "TurnoId", "dbo.Turnoes");
            DropForeignKey("dbo.PublicadorTurnoes", "PublicadorId", "dbo.Publicadors");
            DropForeignKey("dbo.Turnoes", "LocalidadId", "dbo.Localidads");
            DropIndex("dbo.PublicadorTurnoes", new[] { "TurnoId" });
            DropIndex("dbo.PublicadorTurnoes", new[] { "PublicadorId" });
            DropIndex("dbo.Turnoes", new[] { "LocalidadId" });
            DropTable("dbo.Publicadors");
            DropTable("dbo.PublicadorTurnoes");
            DropTable("dbo.Turnoes");
            DropTable("dbo.Localidads");
        }
    }
}
