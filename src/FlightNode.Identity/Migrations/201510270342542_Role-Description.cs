namespace FlightNode.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleDescription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Roles", "Description", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "Description", c => c.String());
        }
    }
}
