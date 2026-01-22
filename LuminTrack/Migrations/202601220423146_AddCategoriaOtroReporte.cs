namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriaOtroReporte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportes", "CategoriaOtro", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportes", "CategoriaOtro");
        }
    }
}
