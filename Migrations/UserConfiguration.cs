namespace EC.Migrations
{
    using EC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class UserConfiguration : DbMigrationsConfiguration<EC.Models.ApplicationDbContext>
    {
        public UserConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EC.Models.ApplicationDbContext context)
        {
            var user = new  ApplicationUser{ Email = "newell-romario@hotmail.com", EmailConfirmed = true, UserName = "newell-romario@hotmail.com"};
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
