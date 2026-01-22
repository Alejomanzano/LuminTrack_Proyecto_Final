namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "FechaRegistro", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "PasswordHash", c => c.String(nullable: false));
            DropColumn("dbo.Usuarios", "Nombre");
            DropColumn("dbo.Usuarios", "Apellido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Apellido", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Usuarios", "Nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Usuarios", "PasswordHash", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Usuarios", "FechaRegistro");
        }
    }
}
