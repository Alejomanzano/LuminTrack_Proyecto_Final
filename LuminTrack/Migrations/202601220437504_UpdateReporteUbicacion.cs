namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporteUbicacion : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reportes", "CategoriaOtro");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reportes", "CategoriaOtro", c => c.String());
        }
    }
}
