using PhoneStore.Data.Abstarct;
using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IPhonerepository repository;
        public AdminController (IPhonerepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Phones);
        }

        public ViewResult Edit(int phoneId)
        {
            Phone phone = repository.Phones.
                FirstOrDefault(p => p.PhoneId == phoneId);
            return View(phone);
        }

        [HttpPost]
        public ActionResult Edit (Phone phone, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    phone.ImageMimeType = image.ContentType;
                    phone.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(phone.ImageData, 0, image.ContentLength);
                }
                repository.SavePhone(phone);
                TempData["message"] = string.Format("Changes in  \"{0}\" was saved", phone.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(phone);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Phone());
        }

        [HttpPost]
        public ActionResult Delete(int phoneId)
        {
            Phone deletedPhone = repository.DeletePhone(phoneId);
            if(deletedPhone != null)
            {
                TempData["message"] = string.Format("Phone \"{0}\" deleted",
                    deletedPhone.Name);
            }
            return RedirectToAction("Index");
        }
    }
}