using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhoneStore.Data.Abstarct;
using System.Collections.Generic;
using PhoneStore.Data.Entities;
using PhoneStore.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Tests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Phones()
        {
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone { PhoneId = 1, Name = "Phone1" },
                new Phone { PhoneId = 2, Name = "Phone2" },
                new Phone { PhoneId = 3, Name = "Phone3" },
                new Phone { PhoneId = 4, Name = "Phone4" },
                new Phone { PhoneId = 5, Name = "Phone5" }
            });

            AdminController controller = new AdminController(mock.Object);

            List<Phone> result = ((IEnumerable<Phone>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual("Phone1", result[0].Name);
            Assert.AreEqual("Phone2", result[1].Name);
            Assert.AreEqual("Phone5", result[4].Name);
        }

        [TestMethod]
        public void Can_Edit_Phones()
        {
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone { PhoneId = 1, Name = "Phone1" },
                new Phone { PhoneId = 2, Name = "Phone2" },
                new Phone { PhoneId = 3, Name = "Phone3" },
                new Phone { PhoneId = 4, Name = "Phone4" },
                new Phone { PhoneId = 5, Name = "Phone5" }
            });

            AdminController controller = new AdminController(mock.Object);

            Phone phone1 = controller.Edit(1).ViewData.Model as Phone;
            Phone phone2 = controller.Edit(2).ViewData.Model as Phone;
            Phone phone3 = controller.Edit(3).ViewData.Model as Phone;

            Assert.AreEqual(1, phone1.PhoneId);
            Assert.AreEqual(2, phone2.PhoneId);
            Assert.AreEqual(3, phone3.PhoneId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Phone()
        {
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone { PhoneId = 1, Name = "Phone1" },
                new Phone { PhoneId = 2, Name = "Phone2" },
                new Phone { PhoneId = 3, Name = "Phone3" },
                new Phone { PhoneId = 4, Name = "Phone4" },
                new Phone { PhoneId = 5, Name = "Phone5" }
            });

            AdminController controller = new AdminController(mock.Object);

            Phone result = controller.Edit(6).ViewData.Model as Phone;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();

            AdminController controller = new AdminController(mock.Object);

            Phone phone = new Phone { Name = "test" };
            ActionResult result = controller.Edit(phone);

            mock.Verify(m => m.SavePhone(phone));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            AdminController controller = new AdminController(mock.Object);
            Phone phone = new Phone { Name = "Test" };
            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.Edit(phone);

            mock.Verify(m => m.SavePhone(It.IsAny<Phone>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            // Организация - создание объекта Game
            Phone phone = new Phone { PhoneId = 2, Name = "Phone2" };

            // Организация - создание имитированного хранилища данных
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
    {
        new Phone { PhoneId = 1, Name = "Phone1"},
        new Phone { PhoneId = 2, Name = "Phone2"},
        new Phone { PhoneId = 3, Name = "Phone3"},
        new Phone { PhoneId = 4, Name = "Phone4"},
        new Phone { PhoneId = 5, Name = "Phone5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(phone.PhoneId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeletePhone(phone.PhoneId));
        }
    }
}
