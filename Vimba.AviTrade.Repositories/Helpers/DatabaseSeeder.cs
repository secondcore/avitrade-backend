using System;
using System.Collections.Generic;
using System.Data.Entity;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Settings;

namespace Vimba.AviTrade.Repositories.Helpers
{
    public class DatabaseSeeder
    {
        private readonly int[] _aircraftIds = {1, 2, 3, 4};
        private readonly int[] _airportIds = {1, 2, 3, 4, 5};
        //private readonly int[] _contractIds = {1, 2, 3, 4};
        private readonly int[] _contractIds = { 1 };
        private readonly string[] _currencyIds = { "AED", "USD", "SAR", "LEB", "EGY" };
        private readonly int[] _itemIds = {1, 2, 3, 4, 5, 6};
        private readonly string[] _operators = {"Al Jaber", "Gulf Wings", "ExecuJet", "Xyz", "Zaman"};
        private readonly int[] _traderIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private IAircraftsRepository _aircraftsRepository;
        private IAirportsRepository _airportsRepository;
        private AviTradeContext _context;
        private IContractsRepository _contractsRepository;
        private ICurrenciesRepository _currenciesRepository;
        private IInvoicesRepository _invoicesRepository;
        private IItemsRepository _itemsRepository;
        private IOrdersRepository _ordersRepository;
        private IPeriodsRepository _periodsRepository;
        private ITradersRepository _tradersRepository;

        public DatabaseSeeder()
        {
            Database.SetInitializer(new DropCreateDatabaseAlwaysWithSeedData());
        }

        public DatabaseSeeder(bool ignore)
        {
        }

        public void PrepareRepositories(bool initialize = false)
        {
            // Kick the model creation process if not already created (hence force is false). In other words, do 
            // not wait on any database activities. Without this, nothing happens unless some context activities take
            // place.
            _context = new AviTradeContext();
            if (initialize)
            {
                // Kick the model creation process if not already created (hence force is false). In other words, do 
                // not wait on any database activities. Without this, nothing happens unless some context activities take
                // place.
                _context.Database.Initialize(force: false);
            }

            _contractsRepository = new ContractsRepository(_context);
            _airportsRepository = new AirportsRepository(_context);
            _aircraftsRepository = new AircraftsRepository(_context);
            _itemsRepository = new ItemsRepository(_context);
            _currenciesRepository = new CurrenciesRepository(_context);
            _periodsRepository = new PeriodsRepository(_context);
            _invoicesRepository = new InvoicesRepository(_context, _periodsRepository);
            _ordersRepository = new OrdersRepository(_context, _contractsRepository, _airportsRepository,
                                                     _aircraftsRepository, _itemsRepository, _currenciesRepository,
                                                     _invoicesRepository);
            _tradersRepository = new TradersRepository(_context);
        }

        public void CreateInitialData()
        {
            PrepareRepositories(true);

            // Create Registration Tokens for all Merchants
            _tradersRepository.CreateRegistrationTokens();

            _context.Dispose();
        }

        public void CreateOrders(int orders = 100, int maxDaysBetweenOrderEvents = 7)
        {
            for (int i = 0; i < orders; i++)
            {
                PrepareRepositories(false);

                // Get a new date between a year ago and now
                //TimeSpan timeSpan = DateTime.Now.AddDays(365) - DateTime.Now;
                //var newSpan = new TimeSpan(new Random(i).Next(0, (int) timeSpan.TotalDays), 0, 0, 0);
                var newSpan = new TimeSpan(new Random(i).Next(-365, 0), 0, 0, 0);

                // Order Date
                DateTime orderDate = DateTime.Now + newSpan;
                Order order = CreateOrder(orderDate);

                // Seller Quotation Date
                int sellerQuotationDays = new Random(i).Next(maxDaysBetweenOrderEvents);
                if (sellerQuotationDays > 0)
                {
                    DateTime sellerQuotationDate = orderDate.AddDays(sellerQuotationDays);
                    QuoteOrder(order, sellerQuotationDate);

                    // Buyer Approval Date
                    int buyerApprovalDays = new Random(sellerQuotationDays).Next(maxDaysBetweenOrderEvents);
                    if (buyerApprovalDays > 0)
                    {
                        DateTime buyerApprovalDate = sellerQuotationDate.AddDays(buyerApprovalDays);
                        ApproveOrder(order, buyerApprovalDate);

                        // Fulfillment Date
                        int fulfillmentDays = new Random(i + buyerApprovalDays).Next(maxDaysBetweenOrderEvents);
                        if (fulfillmentDays > 0)
                        {
                            DateTime fulfillmentDate = buyerApprovalDate.AddDays(fulfillmentDays);
                            FulfillOrder(order, fulfillmentDate);
                        }
                    }
                }
                else
                {
                    IgnoreOrder(order);
                }

                Console.WriteLine("Order [" + order.Id + "] Created!");
                _context.Dispose();
            }
        }

        private Order CreateOrder(DateTime orderDate)
        {
            DateTime estimatedTakeOff = orderDate.AddDays(new Random().Next(7));
            DateTime estimatedLanding = estimatedTakeOff.AddHours(new Random().Next(7));

            var orderLineItems = new List<OrderLineItemDto>();
            int items = new Random().Next(30);
            if (items == 0) items++;
            for (int i = 0; i < items; i++)
            {
                var lineItemDto = new OrderLineItemDto();
                lineItemDto.ItemId = _itemIds[new Random(i).Next(_itemIds.Length)];
                lineItemDto.CurrencyId = _currencyIds[new Random(i).Next(_currencyIds.Length)];
                lineItemDto.Units = new Random(i).Next(1, 20);
                lineItemDto.Instructions = "Psylum Huft.....";
                if (!IsItemExists(orderLineItems, lineItemDto.ItemId))
                    orderLineItems.Add(lineItemDto);
            }

            // The 3rd parameter is the buyer index in the contract. It could be 0 or 1. 
            int buyerIndex = new Random().Next(0, 2);
            if (buyerIndex != 0 && buyerIndex != 1)
                buyerIndex = 1;

            return
                _ordersRepository.Create(new OrderSettings(orderDate,
                                                           _contractIds[new Random(items).Next(_contractIds.Length)],
                                                           buyerIndex, // Should be 0 or 1
                                                           null, null,
                                                           _airportIds[new Random(items).Next(_airportIds.Length)],
                                                           _airportIds[new Random(items).Next(_airportIds.Length)],
                                                           _aircraftIds[new Random(items).Next(_aircraftIds.Length)],
                                                           _operators[new Random(items).Next(_operators.Length)],
                                                           Utilities.GenerateRandomNumber(4).ToUpper(), estimatedTakeOff,
                                                           estimatedLanding, orderLineItems));
        }

        private void QuoteOrder(Order order, DateTime quoteDate)
        {
            var lineItemDtos = new List<OrderLineItemDto>();
            foreach (OrderLineItem lineItem in order.LineItems)
            {
                var lineItemDto = new OrderLineItemDto();
                lineItemDto.ItemId = lineItem.Item.Id;
                lineItemDto.Units = lineItem.Units;
                lineItemDto.CurrencyId = lineItem.Currency.Id;
                lineItemDto.PricePerUnit = Utilities.GenerateRandomAmount(lineItemDto.ItemId, 500);
                lineItemDtos.Add(lineItemDto);
            }

            _ordersRepository.Quote(order.Id, quoteDate, lineItemDtos);
        }

        private void IgnoreOrder(Order order)
        {
            _ordersRepository.Ignore(order.Id, "System");
        }

        private void ApproveOrder(Order order, DateTime approveDate)
        {
            _ordersRepository.Approve(order.Id, approveDate);
        }

        private void FulfillOrder(Order order, DateTime flflDate)
        {
            _ordersRepository.Fulfill(order.Id, flflDate);
        }

        private bool IsItemExists(List<OrderLineItemDto> orderLineItemDtos, int itemId)
        {
            foreach (OrderLineItemDto dto in orderLineItemDtos)
            {
                if (dto.ItemId == itemId)
                    return true;
            }

            return false;
        }
    }
}