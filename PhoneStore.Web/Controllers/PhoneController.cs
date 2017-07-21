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
    public class PhoneController : Controller
    {
        public int pageSize = 2;
        private IPhonerepository repository;
        public PhoneController(IPhonerepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            PhonesListViewModel model = new PhonesListViewModel
            {
                Phones = repository.Phones
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(phone => phone.PhoneId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
            repository.Phones.Count() :
            repository.Phones.Where(phone => phone.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int phoneId)
        {
            Phone phone = repository.Phones
                .FirstOrDefault(p => p.PhoneId == phoneId);

            if (phone != null)
            {
                return File(phone.ImageData, phone.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ViewResult Detail(int? phoneId)
        {
            Phone phone = repository.Phones.
                FirstOrDefault(p => p.PhoneId == phoneId);
            return View(phone);
        }
    }
}
