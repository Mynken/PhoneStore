using PhoneStore.Data.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Web.Controllers
{
    public class NavController : Controller
    {
        private IPhonerepository repository;

        public NavController(IPhonerepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null/*, bool horizontalNav = false*/)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Phones
                .Select(phone => phone.Category)
                .Distinct()
                .OrderBy(x => x);
            //string viewName = horizontalNav ? "MenuHorizontal" : "Menu";

            return PartialView("MixMenu", categories);
        }
    }
}
