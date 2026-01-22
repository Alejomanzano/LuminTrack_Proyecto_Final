namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporteModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportes", "FechaCreacion", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reportes", "Fecha");
            DropColumn("dbo.Reportes", "Latitud");
            DropColumn("dbo.Reportes", "Longitud");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reportes", "Longitud", c => c.Single(nullable: false));
            AddColumn("dbo.Reportes", "Latitud", c => c.Single(nullable: false));
            AddColumn("dbo.Reportes", "Fecha", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reportes", "FechaCreacion");
        }
    }
}
