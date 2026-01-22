namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Luminarias", "Ubicacion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Luminarias", "Ubicacion");
        }
    }
}
