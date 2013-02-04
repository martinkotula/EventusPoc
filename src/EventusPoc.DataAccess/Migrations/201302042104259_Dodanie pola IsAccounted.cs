namespace EventusPoc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodaniepolaIsAccounted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsAccounted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsAccounted");
        }
    }
}
