using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace PhoneStore.Web.Infrastructure.Binders
{
    public class CartModelBinder : System.Web.Mvc.IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext,
            System.Web.Mvc.ModelBindingContext bindingContext)
        {
            // Получить объект Cart из сеанса
            ShoppingCart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (ShoppingCart)controllerContext.HttpContext.Session[sessionKey];
            }

            // Создать объект Cart если он не обнаружен в сеансе
            if (cart == null)
            {
                cart = new ShoppingCart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }

            // Возвратить объект Cart
            return cart;
        }
    }
}