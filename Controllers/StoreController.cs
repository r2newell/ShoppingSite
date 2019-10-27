using System;
using PagedList;
using PagedList.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EC.Models;
using EC.Models.Context;

namespace EC.Controllers
{
    public class StoreController : Controller
    {
        private ProductContext db = new ProductContext();
        private const int PageSize = 12;
        public ActionResult Index(string Search, int? PageNumber, int? min, int? max, int? Filter, string Price, string Name)
        {
            var products = from p in db.Products.Include(p => p.Categories) select p;
            var categories = from c in db.Categories.Include(c => c.Products) select c;
            ViewBag.Search = Search;
            ViewBag.Price = !string.IsNullOrEmpty(Price)? Price: ViewBag.Price;
            ViewBag.Name = !string.IsNullOrEmpty(Name)? Name : ViewBag.Name;
            ViewBag.Max = max;
            ViewBag.Min = min;
            ViewBag.Categories = new SelectList(categories.ToList(), dataValueField: "CategoryID", dataTextField: "Description");

            if(!string.IsNullOrEmpty(Search))
            {
                
                products =  products.Where(m => m.ProductName.Contains(Search));
                PageNumber = 1; 
            }

            if(min != null && min > 0 && min < max)
            {
                products = products.Where(m => m.Price >= min);
                PageNumber = 1;

            }

            if(min != null && max > 0 && min < max)
            {
                products = products.Where(m => m.Price <= max);
                PageNumber = 1;
            }

            switch (ViewBag.Price)
            {
                case "Price_Descending":
                    products = products.OrderByDescending(m => m.Price); 
                    break;
                default:
                    products = products.OrderBy(m => m.Price);
                    break; 
            }

            switch(ViewBag.Name)
            {
                case "Name_Descending":
                    products = products.OrderByDescending(m => m.ProductName);
                    break;

                default:
                    products = products.OrderBy(m => m.ProductName);
                    break;
            }

            return View(products.ToPagedList(PageNumber ?? 1, PageSize));
        }


        
        public ActionResult AddToCart(int? id)
        {
            var product = db.Products.Find(id);
            if(Session["ShoppingCart"] == null)
            {
                Session["ShoppingCart"] = new ShoppingCart { CartItems = new List<CartItem>()};
            }
          
            CartItem item = new CartItem { Product = product, ProductId = product.ProductId, Quantity = 0, Total = 0.00};
            return View(item);
        }

        public ActionResult AddToCart([Bind(Include = "ItemID,ProductId,Product,Total,CartId,ShoppingCart,Quantity")]CartItem item)
        {
            bool state = false;
            var cart = (ShoppingCart)Session["ShoppingCart"];

            if(item.Quantity == 0)
            {
                state = true;
                ModelState.AddModelError("Quantity", "Quantity must be greater than zero");
            }

            if(item.Quantity > item.Product.Quantity)
            {
                state = true;
                ModelState.AddModelError("Quantity", "Can not be greater than product quantity");
            }

            if(state)
            {
                return View(item);
            }

            item.Total = item.Sum();
            var duplicate = cart.CartItems.Find(m => m.ProductId == item.ProductId);

            if(duplicate == null)
                cart.CartItems.Add(item);
            else
            {
                cart.CartItems.Remove(duplicate);
                cart.CartItems.Add(item);
            }
            return View();
        }
    }
}