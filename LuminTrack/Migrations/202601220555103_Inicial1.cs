namespace LuminTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Luminarias", "AlturaPoste", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Luminarias", "AlturaPoste", c => c.Single(nullable: false));
        }
    }
}
