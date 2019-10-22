using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EC.Models.Context;
using PagedList;
namespace EC.Models
{
    public class ProductsController : Controller
    {
        private ProductContext db = new ProductContext();
        const int PageSize = 20; // number of records on a page
       
        // GET: Products
        public ActionResult Index(string SortOrder, int? PageNumber, string search)
        {
            ViewBag.Sort = SortOrder;
            ViewBag.Search = search;
            var products = from m in db.Products select m; 

            if(!string.IsNullOrEmpty(search))
                products = db.Products.Where(m => m.ProductName.ToUpper().Contains(search));
            
            switch(SortOrder)
            {
                case "Descending":
                    products = db.Products.OrderByDescending(m => m.ProductName);
                    break;
                case "Ascending":
                    products = db.Products.OrderBy(m => m.ProductName);
                    break;
                case "Date Ascending":
                    products = db.Products.OrderBy(m => m.Date);
                    break;
                default:
                    products = db.Products.OrderByDescending(m => m.Date);
                    break;       
            }
            int page = (PageNumber ?? 1);
            return View(products.ToPagedList(page, PageSize));
        }

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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "category_description");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Description,Quantity,Price,ImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Date = DateTime.Now;
                product.ImagePath = SaveImage(product); 
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

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
