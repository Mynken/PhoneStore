using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneStore.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using PhoneStore.Data.Abstarct;
using Moq;
using PhoneStore.Web.Controllers;
using System.Web.Mvc;
using PhoneStore.Web.Models;

namespace PhoneStore.Tests
{
    [TestClass]
    public class ShopingCartTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Phone phone1 = new Phone { PhoneId = 1, Name = "Phone1" };
            Phone phone2 = new Phone { PhoneId = 2, Name = "Phone2" };

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Phone, phone1);
            Assert.AreEqual(results[1].Phone, phone2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Phone phone1 = new Phone { PhoneId = 1, Name = "Phone1" };
            Phone phone2 = new Phone { PhoneId = 2, Name = "Phone2" };

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Phone.PhoneId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Phone phone1 = new Phone { PhoneId = 1, Name = "Phone1" };
            Phone phone2 = new Phone { PhoneId = 2, Name = "Phone2" };
            Phone phone3 = new Phone { PhoneId = 3, Name = "Phone3" };

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 4);
            cart.AddItem(phone3, 2);
            cart.AddItem(phone2, 1);

            // Действие
            cart.RemoveLine(phone2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Phone == phone2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Phone phone1 = new Phone { PhoneId = 1, Name = "Phone1", Price = 100 };
            Phone phone2 = new Phone { PhoneId = 2, Name = "Phone2", Price = 55 };

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            decimal result = cart.SumTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Phone phone1 = new Phone { PhoneId = 1, Name = "Phone1", Price = 100 };
            Phone phone2 = new Phone { PhoneId = 2, Name = "Phone2", Price = 55 };

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            Mock<IOrderProcessor> mock2 = new Mock<IOrderProcessor>();
            mock.Setup(m => m.Phones).Returns(new List<Phone> {
        new Phone {PhoneId = 1, Name = "Phone1", Category = "Cat1"},
    }.AsQueryable());

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, mock2.Object);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Phone.PhoneId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhonerepository> mock = new Mock<IPhonerepository>();
            Mock<IOrderProcessor> mock2 = new Mock<IOrderProcessor>();
            mock.Setup(m => m.Phones).Returns(new List<Phone> {
        new Phone {PhoneId = 1, Name = "Phone1", Category = "Cat1"},
    }.AsQueryable());

            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, mock2.Object);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Организация - создание корзины
            ShoppingCart cart = new ShoppingCart();

            // Организация - создание контроллера
            CartController target = new CartController(null, mock.Object);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            ShoppingCart cart = new ShoppingCart();

            // Организация - создание деталей о доставке
            OrderDetails shippingDetails = new OrderDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingCart>(), It.IsAny<OrderDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_OrderDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(new Phone(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new OrderDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingCart>(), It.IsAny<OrderDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(new Phone(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new OrderDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingCart>(), It.IsAny<OrderDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
