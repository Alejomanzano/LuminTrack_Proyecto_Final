namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUbicacionReporte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportes", "Parroquia", c => c.String(nullable: false));
            AddColumn("dbo.Reportes", "CodigoPostal", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Reportes", "CategoriaOtro", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportes", "CategoriaOtro");
            DropColumn("dbo.Reportes", "CodigoPostal");
            DropColumn("dbo.Reportes", "Parroquia");
        }
    }
}
