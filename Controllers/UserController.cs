using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EC.Models;
using EC.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace EC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserContext db = new UserContext();
        // GET: User
       

        [HttpGet]
        [ActionName("Details")]
        // GET: User/Details/5
        public ActionResult Details()
        {
            User user = (User)Session["User"]; 
            if (user == null)
            {
                return RedirectToAction("Create");
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FirstName,LastName,Dob")] User user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                Session["User"] = user;
                return RedirectToAction("Details");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit()
        {
            User user = (User)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Create");
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,LastName,Dob")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(user);
        }

        public ActionResult Delete()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed()
        {
            User user = (User)Session["User"];
            if (user != null)
            {
                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
            }
           
            var Email = (string)Session["Email"];
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            manager.Delete(manager.FindByEmail(Email));
            Session["User"] = null;
            return RedirectToAction("Index", "Home", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
