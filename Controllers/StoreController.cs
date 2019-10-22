using EC.Models;
using EC.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC.Controllers
{
    public class StoreController : Controller
    {
        private ProductContext ProductContext = new ProductContext();

        public ActionResult Index()
        {

            var products = ProductContext.Products;
            return View(products);
        }


        public ActionResult AddToCart(Product product)
        {
            if (Session["Cart"] == null)
                Session["Cart"] = new Dictionary<int, CartItem>();
            
            Session["Product"] = product;
            CartItem cart = new CartItem { };
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart([Bind(Include = "ItemID,Total,Quantity")] CartItem cart)
        {
            Product product = (Product)Session["Product"];

            if(!ModelState.IsValid)
                return View(cart);

            cart.Product = product;
            cart.ProductId = product.ProductId;
            cart.Total = cart.Sum();
            var ShoppingCart = (Dictionary<int, CartItem>)Session["Cart"];
            ShoppingCart[cart.ProductId] = cart;
            Session["Cart"] = ShoppingCart;
            ViewBag.Message = "Successfully, added to your cart.";
            return View(cart);
        }

    }
}