using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Settings;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Repositories.Helpers;

namespace Vimba.AviTrade.Repositories.Tests
{
    [TestClass]
    public class WhenOrdersRepositoryCreateOrderExecutes : GenericRepositoryTests
    {
        // Seeder Methods - I would like them to be in every test case...just to remind me that they are there and can be used
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext context)
        {
        }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            InitializeNewTestEnvironment(context);
        }

        [TestInitialize]
        public virtual void InitializeBeforeEachTestMethod()
        {
        }

        [TestCleanup]
        public virtual void CleanupAfterEachTestMethod()
        {
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            DisposeTestEnvironment();
        }

        [AssemblyCleanup]
        public static void CleanupAssembly()
        {
        }

        // T E S T Methods
        [TestMethod]
        public void ItShouldReturnProperOrder()
        {
            // Arrange
            var items = new List<OrderLineItemDto>();
            items.Add(new OrderLineItemDto () {ItemId = 1, Units = 2, CurrencyId = "AED", PricePerUnit = 0});
            items.Add(new OrderLineItemDto () {ItemId = 2, Units = 1, CurrencyId = "AED", PricePerUnit = 0});
            items.Add(new OrderLineItemDto () {ItemId = 3, Units = 3, CurrencyId = "AED", PricePerUnit = 0});
            items.Add(new OrderLineItemDto () {ItemId = 4, Units = 2, CurrencyId = "AED", PricePerUnit = 0});
            
            // Act
            var order = _ordersRepository.Create(new OrderSettings(DateTime.Now, 1, 0, null, null, 1, 1, 1, "XYZ", "AF8787", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1), items));

            // Assert - Although this is in violation of one assert per test case, it is reasonable to do this because we are actually only testing the order
            Assert.IsNotNull(order);
            Assert.AreEqual(items.Count, order.LineItems.Count);
            CollectionAssert.AllItemsAreNotNull(order.LineItems);
            Assert.AreEqual(Order.Submitted, order.Status);
            Assert.AreEqual(1, order.Buyer.Id);
            Assert.AreEqual(3, order.Seller.Id);
            Assert.AreEqual(0, order.Amount);
            Assert.AreEqual(1, order.Buyer.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void ItShouldThrowAnExceptionIfBadInput()
        {
            // Arrange
            var items = new List<OrderLineItemDto>();
            items.Add(new OrderLineItemDto() { ItemId = 1, Units = 2, CurrencyId = "AED", PricePerUnit = 0 });

            // Act
            var order = _ordersRepository.Create(new OrderSettings(DateTime.Now, -1, 0, null, null, 1, 1, 1, "XYZ", "AF8787", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1), items));

            // Assert happens on the attribute as we expect this 'Create' to cause an exception!!
        }
    }
}
