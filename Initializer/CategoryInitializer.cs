using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using EC.Models.Context;
using EC.Models;

namespace EC.Initializer
{
    public class CategoryInitializer : DropCreateDatabaseAlways<ProductContext>
    {
        protected override void Seed(ProductContext context)
        {
            var categories = new List<Category>
            {
                new Category{Description = "None"},
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

            context.Categories.AddRange(categories.ToArray());
            context.SaveChanges();
        }
    }
}