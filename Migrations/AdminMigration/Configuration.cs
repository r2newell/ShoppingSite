namespace EC.AdminMigration
{
    using EC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"AdminMigration";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var user = new ApplicationUser { Email = "newell-romario@hotmail.com", EmailConfirmed = true, UserName = "newell-romario@hotmail.com" };
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            manager.Create(user, "Pa$$word1!");
            context.Users.Add(user);
            context.SaveChanges();

            var roles = new List<string>
            {
                 "Administrator",
                 "Customer",
                 "Manager",
                 "Employee"
            };

            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            AddRoles(roles, rolemanager);
            manager.AddToRole(user.Id, "Administrator");
            context.SaveChanges();
            base.Seed(context);
        }

        public void AddRoles(List<string> roles, RoleManager<IdentityRole> manager)
        {
            foreach (string role in roles)
            {
                if (manager.RoleExists(role) == false)
                    manager.Create(new IdentityRole { Name = role });

            }
        }
    }
}
