using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EC.Models.Context
{
    public class UserContext: DbContext
    {
        public UserContext() : base("DefaultConnection")
        {
        }
        public DbSet<User> users { get; set; }
    }
}

