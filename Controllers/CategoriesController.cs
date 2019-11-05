using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EC.Models.Context;
using PagedList;


namespace EC.Models
{
    [Authorize(Roles = "Administrator,Employee,Manager")]
    public class CategoriesController : Controller
    {
        private ProductContext db = new ProductContext();
        const int PageSize = 10; 
        [Authorize(Roles = "Administrator,Employee,Manager")] // GET: Categories
        public ActionResult Index(string SortOrder, int? PageNumber, string Search)
        {
            ViewBag.CurrentSort = SortOrder;
            ViewBag.SortOrder = string.IsNullOrEmpty(SortOrder) ?  "Descending" : string.Empty;
            ViewBag.Search = Search;
            var categories = from m in db.Categories select m;

            if (!string.IsNullOrEmpty(Search))
            {
                categories = categories.Where(m => m.Description.Contains(Search));
                PageNumber = 1;
            }

            switch (SortOrder)
            {
                case "Descending":
                    categories = categories.OrderByDescending(m => m.Description);
                    break;
                default:
                    categories = categories.OrderBy(m => m.Description);
                    break;
            }

            int page = (PageNumber ?? 1);
            return View(categories.ToPagedList(page, PageSize));
        }

        [Authorize(Roles = "Administrator,Employee,Manager")]// GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "Administrator,Manager")]
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> Create([Bind(Include = "CategoryID,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [Authorize(Roles = "Administrator,Manager")]
        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryID,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
               await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [Authorize(Roles = "Administrator,Manager")]
        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> AddProducts()
        {
            var categories = from m in db.Categories select m;
            Session["categories"] = await categories.ToDictionaryAsync(m => m.CategoryID);
            Session["products"] = db.Products.Include(m => m.Categories).ToDictionary(m => m.ProductId);
            ViewData["ProductId"] = new SelectList(items: db.Products, dataValueField: "ProductId", dataTextField: "ProductName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProducts([Bind(Include = "ID,categories")]AddProducts category)
        {
            ViewData["ProductId"] = new SelectList(items: db.Products, dataValueField: "ProductId", dataTextField: "ProductName");
            if (!ModelState.IsValid)
                return View(category);
            
            
            var categories = (Dictionary<int, Category>)Session["categories"];
            var product = await db.Products.FindAsync(category.ID);
            if (product.Categories == null)
                product.Categories = new List<Category>();
            

            foreach (int index in category.categories)
            {
                Category output = null;
                categories.TryGetValue(index, out output);
                db.Categories.Attach(output);
                if (product.Categories.Contains(output) == false)
                    product.Categories.Add(output);
                
            }

            Session.Remove("products");
            Session.Remove("categories");
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator,Manager")]
        public ActionResult DeleteAddProduct(int? Id)
        {
            var product = db.Products.Include(m => m.Categories).First(m => m.ProductId == Id);
            TempData["product"] = product;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAddProduct([Bind(Include = "ID,categories")]AddProducts category)
        {
            var product = db.Products.Include(m=> m.Categories).First(m => m.ProductId == category.ID);
            if(!ModelState.IsValid)
            {
                TempData["product"] = product;
                return View(category);
            }

            foreach (int index in category.categories)
                product.Categories.Remove(product.Categories.Find(m => m.CategoryID == index));
              
           
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Products");
        }


        [Authorize(Roles = "Administrator,Employee,Manager")]
        public ActionResult DetailsCategory(int? Id)
        {
            var product = from p in db.Products select p;
            var item = product.Include(m => m.Categories).First(m => m.ProductId == Id); 
            return View(item); 
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
