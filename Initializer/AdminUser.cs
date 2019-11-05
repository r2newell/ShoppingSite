using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using EC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EC.Initializer
{
    public class AdminUser : DropCreateDatabaseAlways<ApplicationDbContext>
    {
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