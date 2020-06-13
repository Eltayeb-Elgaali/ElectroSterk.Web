using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Xml.Schema;
using DA;
using ElectroSterk.Web.Models;
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

        public ActionResult PaypalPartial()
        {
            List<Cart> cart = Session["cart"] as List<Cart>;

            return PartialView(cart);
        }

        public void PlaceOrder()
        {
            List<Cart> cart = Session["cart"] as List<Cart>;

            string username = User.Identity.Name;

            int orderId = 0;

            using (var db = new ElectroSterkDbContext())
            {
                Order order = new Order();
                var user = db.OrderUsers.FirstOrDefault(x => x.UserName == username);
                int userId = user.Id;
                order.UserId = userId;
                order.CreatedOn = DateTime.Now;

                db.Orders.Add(order);
                db.SaveChanges();

                orderId = order.OrderId;

                OrderDetails orderDetails = new OrderDetails();

                foreach (var item in cart)
                {
                    orderDetails.OrderId = orderId;
                    orderDetails.UserId = userId;
                    orderDetails.ProductId = item.ProductId;
                    orderDetails.Quantity = item.Quantity;

                    db.OrderDetails.Add(orderDetails);

                    db.SaveChanges();
                }

            }

            //email 
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("d0a13fae182e56", "759256fa7de8e5"),
                EnableSsl = true
            };
            client.Send("admin@example.com", "admin@example.com", "New Order", "You have a new order . order number is " + orderId);

            Session["cart"] = null;
        }
    }
}