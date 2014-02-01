using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Web.Controllers;
using Vimba.AviTrade.Web.ViewModels;

namespace Vimba.AviTrade.Web.Tests.Controllers
{
    [TestClass]
    public class WhenBuildAjaxOrdersViewModelGetsInvoked
    {
        [TestMethod]
        public void ItReturnsNotNullModel()
        {
            // Arrange
            AviTradeContext dbContext = new AviTradeContext();
            IItemsRepository itemsRepository = new ItemsRepository(dbContext);
            IAirportsRepository airportsRepository = new AirportsRepository(dbContext);
            IAircraftsRepository aircraftsRepository = new AircraftsRepository(dbContext);
            IContractsRepository contractsRepository = new ContractsRepository(dbContext);
            ITradersRepository tradersRepository = new TradersRepository(dbContext);
            IPeriodsRepository periodsRepository = new PeriodsRepository(dbContext);
            IInvoicesRepository invoicesRepository = new InvoicesRepository(dbContext, periodsRepository);
            ICurrenciesRepository currenciesRepository = new CurrenciesRepository(dbContext);
            IOrdersRepository ordersRepository = new OrdersRepository(dbContext, contractsRepository, airportsRepository, aircraftsRepository, itemsRepository, currenciesRepository, invoicesRepository);
            DataServiceController controller = null; // new DataServiceController(contractsRepository, tradersRepository, ordersRepository, itemsRepository, airportsRepository, aircraftsRepository);

            // Act
            AjaxOrdersViewModel model = controller.BuildAjaxOrdersViewModel(1, 0, "1,2,3,4", DateTime.Now.AddDays(30),
                                                                            true, 0, 20);

            // Assert
            Assert.IsNotNull(model);
        }
    }
}
