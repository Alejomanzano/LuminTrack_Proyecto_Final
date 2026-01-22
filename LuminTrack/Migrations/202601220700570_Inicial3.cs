namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reportes", "UsuarioEmail", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reportes", "UsuarioEmail", c => c.String(nullable: false));
        }
    }
}
