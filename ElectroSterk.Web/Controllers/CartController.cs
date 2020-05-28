using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Schema;
using DA;
using Entities;

namespace ElectroSterk.Web.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<Cart> ?? new List<Cart>();

            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }

            decimal totla = 0m;
            foreach (var item in cart)
            {
                totla += item.Total;
            }

            ViewBag.GrandTotal = totla;

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            Cart cart = new Cart();

            var quantity = 0;
            decimal price = 0m;

            if (Session["cart"] != null)
            {
                var list = (List<Cart>) Session["cart"];
                foreach (var item in list)
                {
                    quantity += item.Quantity;
                    price += item.Quantity * item.Price;
                }
                cart.Quantity = quantity;
                cart.Price = price;
            }
            else
            {
                cart.Quantity = 0;
                cart.Price = 0m;
            }

            return PartialView(cart);
        }

        public ActionResult AddToCartPartial(int id)
        {
            List<Cart> cart = Session["cart"] as List<Cart> ?? new List<Cart>();

            Cart model = new Cart();

            using (var db = new ElectroSterkDbContext())
            {
                Product product = db.Products.Find(id);

                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

                if (productInCart == null)
                {
                    cart.Add(new Cart()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.ImageName
                    });
                }
                else
                {
                    productInCart.Quantity++; 
                }
            }

            int quantity = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                quantity += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = quantity;
            model.Price = price;

            Session["cart"] = cart;

            return PartialView(model);
        }

        public JsonResult IncrementProduct(int productId)
        {
            List<Cart> cart = Session["cart"] as List<Cart>;

            using (var db = new ElectroSterkDbContext())
            {
                Cart model = cart.FirstOrDefault(x => x.ProductId == productId);

                model.Quantity++;
                var result = new {quantity = model.Quantity, price = model.Price};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult DecrementProduct(int productId)
        {
            List<Cart> cart = Session["cart"] as List<Cart>;

            using (var db = new ElectroSterkDbContext())
            {
                Cart model = cart.FirstOrDefault(x => x.ProductId == productId);

                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    cart.Remove(model);
                }

                var result = new { quantity = model.Quantity, price = model.Price };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public void RemoveProduct(int productId)
        {
            List<Cart> cart = Session["cart"] as List<Cart>;

            using (var db = new ElectroSterkDbContext())
            {
                Cart model = cart.FirstOrDefault(x => x.ProductId == productId);

                cart.Remove(model);
            }
        }
    }
}