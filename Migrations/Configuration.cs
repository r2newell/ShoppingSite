namespace EC.Migrations
{
    using EC.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EC.Models.Context.ProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EC.Models.Context.ProductContext context)
        {

            var categories = new List<Category>
            {
                new Category{ Description = "Fashion"}, 
                new Category{ Description = "Pants"},
                new Category{ Description = "Shoes"}, 
                new Category{ Description = "Male"}, 
                new Category{ Description = "Female"}, 
                new Category{ Description = "Gaming Console"}, 
                new Category{ Description = "Computers"},
                new Category{ Description = "Smartphones"}, 
                new Category{ Description = "Tablets"}, 
                new Category{ Description = "Phone Accessories"}, 
                new Category{ Description = "Clothes"},
                new Category{ Description = "Jewelry"},
                new Category{ Description = "Office Supplies"}
            };

            context.Categories.AddOrUpdate(categories.ToArray());
            context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
