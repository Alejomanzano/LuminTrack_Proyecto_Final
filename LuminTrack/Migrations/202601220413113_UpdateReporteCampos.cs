namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporteCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportes", "OtraCategoria", c => c.String(maxLength: 300));
            AlterColumn("dbo.Reportes", "Estado", c => c.String());
            AlterColumn("dbo.Reportes", "UsuarioEmail", c => c.String(nullable: false));
            DropColumn("dbo.Reportes", "CategoriaOtro");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reportes", "CategoriaOtro", c => c.String(maxLength: 300));
            AlterColumn("dbo.Reportes", "UsuarioEmail", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Reportes", "Estado", c => c.String(nullable: false));
            DropColumn("dbo.Reportes", "OtraCategoria");
        }
    }
}
