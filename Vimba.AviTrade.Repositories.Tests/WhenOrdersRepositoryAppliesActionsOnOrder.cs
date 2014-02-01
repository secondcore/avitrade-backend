using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Settings;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Repositories.Helpers;

namespace Vimba.AviTrade.Repositories.Tests
{
    [TestClass]
    public class WhenOrdersRepositoryAppliesActionsOnOrder : GenericRepositoryTests
    {
        // Perhaps a violation of the TFD rules...but what the heck...I could not find anything to maintain state.
        private Order _order;

        // Seeder Methods - I would like them to be in every test case...just to remind me that they are there and can be used
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

        // T E S T Methods
        [TestMethod]
        public void ItTransitionsToSumbittedAfterNew()
        {
            // Arrange
            var items = new List<OrderLineItemDto>();
            items.Add(new OrderLineItemDto () {ItemId = 1, Units = 2, CurrencyId = "AED", PricePerUnit = 0});
            items.Add(new OrderLineItemDto () {ItemId = 2, Units = 1, CurrencyId = "AED", PricePerUnit = 0});
            items.Add(new OrderLineItemDto () {ItemId = 3, Units = 3, CurrencyId = "AED", PricePerUnit = 0});
            items.Add(new OrderLineItemDto () {ItemId = 4, Units = 2, CurrencyId = "AED", PricePerUnit = 0});
            
            // Act
            _order = _ordersRepository.Create(new OrderSettings(DateTime.Now, 1, 0, null, null, 1, 1, 1, "XYZ", "AF8787", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1), items) 
/*List<OrderLineItemDto> lineItems*/);

            // Assert
            Assert.AreEqual(Order.Submitted, _order.Status);
        }

        [TestMethod]
        public void ItTransactionsToQuotedAfterQuote()
        {
            // Arrange
            ItTransitionsToSumbittedAfterNew();

            var items = new List<OrderLineItemDto>();
            items.Add(new OrderLineItemDto() { ItemId = 1, Units = 2, CurrencyId = "AED", PricePerUnit = 100 });
            items.Add(new OrderLineItemDto() { ItemId = 2, Units = 1, CurrencyId = "AED", PricePerUnit = 250 });
            items.Add(new OrderLineItemDto() { ItemId = 3, Units = 3, CurrencyId = "AED", PricePerUnit = 1100 });
            items.Add(new OrderLineItemDto() { ItemId = 4, Units = 2, CurrencyId = "AED", PricePerUnit = 300 });

            // Act
            _ordersRepository.Quote(_order.Id, DateTime.Now, items);
            // Pick up the line items that have been quoted
            var quotedLineItemsSize = (from i in _order.LineItems where i.PricePerUnit != 0 select i).ToList().Count;

            // Assert
            Assert.AreEqual(items.Count, quotedLineItemsSize);
            Assert.AreEqual(Order.Quoted, _order.Status);
        }

        [TestMethod]
        public void ItTransitionsToApproveAfterApprove()
        {
            // Arrange
            ItTransactionsToQuotedAfterQuote();

            // Act
            _ordersRepository.Approve(_order.Id, DateTime.Now);

            // Assert
            Assert.AreEqual(Order.Approved, _order.Status);
        }

        [TestMethod]
        public void ItTransistionsToIgnoredAfterIgnore()
        {
            // Arrange
            ItTransitionsToSumbittedAfterNew();

            // Act
            _ordersRepository.Ignore(_order.Id, "System");

            // Assert
            Assert.IsTrue(_ordersRepository.IsIgnored(_order.Id, "System"));
        }

        [TestMethod]
        public void ItTransitionsToNotIgnoredAfterResetIgnore()
        {
            // Arrange
            ItTransistionsToIgnoredAfterIgnore();

            // Act
            _ordersRepository.ResetIgnore(_order.Id, "System");

            // Assert
            Assert.IsFalse(_ordersRepository.IsIgnored(_order.Id, "System"));
        }

        [TestMethod]
        public void ItTransistionsToViewedAfterView()
        {
            // Arrange
            ItTransitionsToSumbittedAfterNew();

            // Act
            _ordersRepository.View(_order.Id, "System");

            // Assert
            Assert.IsTrue(_ordersRepository.IsViewed(_order.Id, "System"));
        }

        [TestMethod]
        public void ItTransistionsToNotViewedAfterResetView()
        {
            // Arrange
            ItTransistionsToViewedAfterView();

            // Act
            _ordersRepository.ResetView(_order.Id, "System");

            // Assert
            Assert.IsFalse(_ordersRepository.IsViewed(_order.Id, "System"));
        }

        [TestMethod]
        public void ItTransitionsToArchivedAfterArchive()
        {
            // Arrange
            ItTransitionsToSumbittedAfterNew();

            // Act
            _ordersRepository.Archive(_order.Id, "System");

            // Assert
            Assert.IsTrue(_ordersRepository.IsArchived(_order.Id, "System"));
        }

        [TestMethod]
        public void ItTransitionsToNotArchivedAfterResetArchive()
        {
            // Arrange
            ItTransitionsToArchivedAfterArchive();

            // Act
            _ordersRepository.ResetArchive(_order.Id, "System");

            // Assert
            Assert.IsFalse(_ordersRepository.IsArchived(_order.Id, "System"));
        }
    }
}
