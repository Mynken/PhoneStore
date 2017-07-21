using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneStore.Web.Models
{
    public class CartIndexViewModel
    {
        public ShoppingCart Cart { get; set; }
        public string ReturnUrl
        {
            get; set;
        }
    }
}