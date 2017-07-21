using PhoneStore.Data.Abstarct;
using PhoneStore.Data.Entities;
using PhoneStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Web.Controllers
{
    public class CartController : Controller
    {
        private IPhonerepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IPhonerepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public ViewResult Index(ShoppingCart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(ShoppingCart cart, int phoneId, string returnUrl)
        {
            Phone phone = repository.Phones
                .FirstOrDefault(g => g.PhoneId == phoneId);

            if (phone != null)
            {
                cart.AddItem(phone, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(ShoppingCart cart, int phoneId, string returnUrl)
        {
            Phone phone = repository.Phones
                .FirstOrDefault(g => g.PhoneId == phoneId);

            if (phone != null)
            {
                cart.RemoveLine(phone);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(ShoppingCart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new OrderDetails());
        }

        [HttpPost]
        public ViewResult Checkout(ShoppingCart cart, OrderDetails orderDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry your bag is empty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, orderDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(orderDetails);
            }
        }

    }
}