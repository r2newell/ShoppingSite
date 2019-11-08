namespace EC.ProductMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Address2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Address2", c => c.String(nullable: false));
            AddColumn("dbo.OrderDetails", "City", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "City");
            DropColumn("dbo.OrderDetails", "Address2");
        }
    }
}
