namespace EC.ProductMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Country : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Country", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Country");
        }
    }
}
