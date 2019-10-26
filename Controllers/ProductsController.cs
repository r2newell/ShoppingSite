using EC.Models.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace EC.Models
{
    [Authorize(Roles = "Administrator,Employee,Manager")]
    public class ProductsController : Controller
    {
        private ProductContext db = new ProductContext();
        const int PageSize = 20; // number of records on a page

        [Authorize(Roles = "Administrator,Employee,Manager")]
        // GET: Products
        public ActionResult Index(string SortOrder, int? PageNumber, string Search)
        {
            ViewBag.CurrentSort = SortOrder;
            ViewBag.SortOrder = string.IsNullOrEmpty(SortOrder) ? "Descending" : string.Empty;
            ViewBag.DateSort = string.IsNullOrEmpty(SortOrder) ? "Date Ascending" : string.Empty;
            ViewBag.Search = Search;
            var products = from m in db.Products select m;

            if (!string.IsNullOrEmpty(Search))
            {
                products = products.Where(m => m.ProductName.ToUpper().Contains(Search));
                PageNumber = 1;
            }

            switch (SortOrder)
            {
                case "Descending":
                    products = products.OrderByDescending(m => m.ProductName);
                    break;
                case "Ascending":
                    products = products.OrderBy(m => m.ProductName);
                    break;
                case "Date Ascending":
                    products = products.OrderBy(m => m.Date);
                    break;
                default:
                    products = products.OrderByDescending(m => m.Date);
                    break;
            }
            int page = (PageNumber ?? 1);
            return View(products.ToPagedList(page, PageSize));
        }

        [Authorize(Roles = "Administrator,Employee,Manager")]
        [HttpGet]// GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Manager,Administrator")]
        // GET: Products/Create
        public ActionResult Create()
        {
            Session["categories"] = db.Categories.ToDictionary(m => m.CategoryID);
            ViewBag.Categories = Session["categories"];
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Description,Quantity,category,Price,ImageFile")] Product product)
        {
            var dictionary = (Dictionary<int, Category>)Session["categories"];
            if (ModelState.IsValid)
            {
                product.Categories = new List<Category>();
                foreach (var category in product.category)
                {
                    product.Categories.Add(dictionary[category]);
                }
                product.Date = DateTime.Now;
                product.ImagePath = SaveImage(product);
                db.Products.Add(product);
                db.SaveChanges();
                Session.Remove("categories");
                return RedirectToAction("Index");
            }
            ViewBag.Categories = dictionary;
            return View(product);
        }

        [Authorize(Roles = "Administrator,Manager")]
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Product_name,Product_description,Product_quantity,Price,ImagePath,ImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    DeleteImage(product);
                    product.ImagePath = null;
                    product.ImagePath = SaveImage(product);
                }
                db.Products.Add(product);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Administrator,Manager")]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = product = db.Products.Find(id);
            DeleteImage(product);
            product.ImagePath = null;
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ImageUpload(Product product)
        {
            ViewBag.Image = product;
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        private string SaveImage(Product product)
        {
            if (product.ImageFile == null)
                return null;
            string FileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string Extension = Path.GetExtension(product.ImageFile.FileName);
            FileName = FileName + DateTime.Now.ToString("yymmssfff") + Extension;
            string ImagePath = "~/ProductImages/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/ProductImages"), FileName);
            product.ImageFile.SaveAs(FileName);
            return ImagePath;
        }

        private void DeleteImage(Product product)
        {
            if (product.ImagePath == null)
                return;
            string FullPath = Server.MapPath(product.ImagePath);
            FileInfo fileInfo = new FileInfo(FullPath);
            fileInfo.Delete();

        }
    }
}
