using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhoneStore.Data.Abstarct;
using PhoneStore.Data.Entities;
using PhoneStore.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PhoneStore.Web.Models;
using PhoneStore.Web.HtmlHelpers;

namespace PhoneStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // (arrange)
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone { PhoneId = 1, Name = "Phone1"},
                new Phone { PhoneId = 2, Name = "Phone2"},
                new Phone { PhoneId = 3, Name = "Phone3"},
                new Phone { PhoneId = 4, Name = "Phone4"},
                new Phone { PhoneId = 5, Name = "Phone5"}
            });
            PhoneController controller = new PhoneController(mock.Object);
            controller.pageSize = 3;

            // (act)
            PhonesListViewModel result = (PhonesListViewModel)controller.List(null, 2).Model;

            // (assert)
            List<Phone> phones = result.Phones.ToList();
            Assert.IsTrue(phones.Count == 2);
            Assert.AreEqual(phones[0].Name, "Phone4");
            Assert.AreEqual(phones[1].Name, "Phone5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
    {
        new Phone { PhoneId = 1, Name = "Phone1"},
        new Phone { PhoneId = 2, Name = "Phone2"},
        new Phone { PhoneId = 3, Name = "Phone3"},
        new Phone { PhoneId = 4, Name = "Phone4"},
        new Phone { PhoneId = 5, Name = "Phone5"}
    });
            PhoneController controller = new PhoneController(mock.Object);
            controller.pageSize = 3;

            // Act
            PhonesListViewModel result
                = (PhonesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
    {
        new Phone { PhoneId = 1, Name = "Phone1", Category="Cat1"},
        new Phone { PhoneId = 2, Name = "Phone2", Category="Cat2"},
        new Phone { PhoneId = 3, Name = "Phone3", Category="Cat1"},
        new Phone { PhoneId = 4, Name = "Phone4", Category="Cat2"},
        new Phone { PhoneId = 5, Name = "Phone5", Category="Cat3"}
    });
            PhoneController controller = new PhoneController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Phone> result = ((PhonesListViewModel)controller.List("Cat2", 1).Model)
                .Phones.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Phone2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Phone4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone> {
        new Phone { PhoneId = 1, Name = "Phone1", Category="Симулятор"},
        new Phone { PhoneId = 2, Name = "Phone2", Category="Симулятор"},
        new Phone { PhoneId = 3, Name = "Phone3", Category="Шутер"},
        new Phone { PhoneId = 4, Name = "Phone4", Category="RPG"},
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "RPG");
            Assert.AreEqual(results[1], "Симулятор");
            Assert.AreEqual(results[2], "Шутер");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new Phone[] {
        new Phone { PhoneId = 1, Name = "Phone1", Category="2Sim"},
        new Phone { PhoneId = 2, Name = "Phone2", Category="Fast"}
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Fast";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
    {
        new Phone { PhoneId = 1, Name = "Phone1", Category="Cat1"},
        new Phone { PhoneId = 2, Name = "Phone2", Category="Cat2"},
        new Phone { PhoneId = 3, Name = "Phone3", Category="Cat1"},
        new Phone { PhoneId = 4, Name = "Phone4", Category="Cat2"},
        new Phone { PhoneId = 5, Name = "Phone5", Category="Cat3"}
    });
            PhoneController controller = new PhoneController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((PhonesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((PhonesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((PhonesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((PhonesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;
            
            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
