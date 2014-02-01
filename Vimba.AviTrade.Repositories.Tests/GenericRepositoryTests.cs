using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using Vimba.AviTrade.Repositories;

namespace Vimba.AviTrade.Repositories.Tests
{
    public class GenericRepositoryTests
    {
        protected static TestContext _testContext {get; set;}
        protected static AviTradeContext _aviTradeContext { get; set; }
        protected static IContractsRepository _contractsRepository { get; set; }
        protected static IAirportsRepository _airportsRepository { get; set; }
        protected static IAircraftsRepository _aircraftsRepository { get; set; }
        protected static IItemsRepository _itemsRepository { get; set; }
        protected static ICurrenciesRepository _currenciesRepository { get; set; }
        protected static IPeriodsRepository _periodsRepository { get; set; }
        protected static IInvoicesRepository _invoicesRepository { get; set; }
        protected static IOrdersRepository _ordersRepository { get; set; }
        protected static ITradersRepository _tradersRepository { get; set; }

        public GenericRepositoryTests()
        {
            // TODO: How do I inject the test context here?
        }

        public static void InitializeNewTestEnvironment(TestContext context)
        {
            _testContext = context;
            SetupNewDatabaseEnvironment();
            SetupRepositories(_aviTradeContext);
        }

        public static void InitializeExistingTestEnvironment(TestContext context)
        {
            _testContext = context;
            SetupExistingDatabaseEnvironment();
            SetupRepositories(_aviTradeContext);
        }

        public static void DisposeTestEnvironment()
        {
            _aviTradeContext.Dispose();
        }

        /* P R I V A T E */

        private static void SetupNewDatabaseEnvironment()
        {
            Database.SetInitializer(new DropCreateDatabaseAlwaysWithSeedData());
            _aviTradeContext = new AviTradeContext();
            // Kick the model creation process if not already created (hence force is false). In other words, do 
            // not wait on any database activities. Without this, nothing happens unless some context activities take
            // place.
            _aviTradeContext.Database.Initialize(force: false);
        }

        private static void SetupExistingDatabaseEnvironment()
        {
            _aviTradeContext = new AviTradeContext();
        }

        private static void SetupRepositories(AviTradeContext ctx)
        {
            _contractsRepository = new ContractsRepository(ctx);
            _airportsRepository = new AirportsRepository(ctx);
            _aircraftsRepository = new AircraftsRepository(ctx);
            _itemsRepository = new ItemsRepository(ctx);
            _currenciesRepository = new CurrenciesRepository(ctx);
            _periodsRepository = new PeriodsRepository(ctx);
            _invoicesRepository = new InvoicesRepository(ctx, _periodsRepository);
            _ordersRepository = new OrdersRepository(ctx, _contractsRepository, _airportsRepository, _aircraftsRepository, _itemsRepository, _currenciesRepository, _invoicesRepository);
            _tradersRepository = new TradersRepository(ctx);
        }
    }
}
