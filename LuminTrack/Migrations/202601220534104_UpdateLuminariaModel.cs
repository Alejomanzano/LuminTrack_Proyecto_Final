namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLuminariaModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Luminarias", "CodigoLuminaria", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Luminarias", "AlturaPoste", c => c.Single(nullable: false));
            DropColumn("dbo.Luminarias", "Latitud");
            DropColumn("dbo.Luminarias", "Longitud");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Luminarias", "Longitud", c => c.Single(nullable: false));
            AddColumn("dbo.Luminarias", "Latitud", c => c.Single(nullable: false));
            DropColumn("dbo.Luminarias", "AlturaPoste");
            DropColumn("dbo.Luminarias", "CodigoLuminaria");
        }
    }
}
