using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneStore.Data.Entities;
using Moq;
using PhoneStore.Data.Abstarct;
using System.Collections.Generic;
using System.Linq;
using PhoneStore.Web.Controllers;
using System.Web.Mvc;

namespace PhoneStore.Tests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Phone phone = new Phone
            {
                PhoneId = 2,
                Name = "Phone2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone> {
                new Phone {PhoneId = 1, Name = "Phone1"},
                phone,
                new Phone {PhoneId = 3, Name = "Phone3"}
            }.AsQueryable());

            // Организация - создание контроллера
           PhoneController controller = new PhoneController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(phone.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone> {
                new Phone {PhoneId = 1, Name = "Phone1"},
                new Phone {PhoneId = 2, Name = "Phone2"}
            }.AsQueryable());

            // Организация - создание контроллера
            PhoneController controller = new PhoneController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}
