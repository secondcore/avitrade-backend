using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Settings;
using Vimba.AviTrade.Models.Stats;
using Vimba.AviTrade.Repositories.Helpers;

namespace Vimba.AviTrade.Repositories
{
	public class OrdersRepository : GenericRepository<Order>, IOrdersRepository
	{
		private IContractsRepository _contractsRepository = null;
		private IAirportsRepository _airportsRepository = null;
		private IAircraftsRepository _aircraftsRepository = null;
		private IItemsRepository _itemsRepository = null;
		private ICurrenciesRepository _currenciesRepository = null;
		private IInvoicesRepository _invoicesRepository = null;

		protected override IQueryable<Order> DefaultSet
		{
			get
			{
				return _Context.Orders
					.Include("Contract.BillingCurrency")
					.Include("Contract.TimeZone")
					.Include("Contract.Instance")
					.Include("Buyer")
					.Include("Seller")

					//.Include("Invoice")
					.Include("LineItems.Item")
					.Include("LineItems.Currency")
					.Include("TakeoffAirport")
					.Include("LandingAirport")
					.Include("Aircraft");
			}
		}

		protected override Func<Order, object> DefaultOrderBy
		{
			get
			{
				return x => x.OrderDate;
			}
		}

		public OrdersRepository(AviTradeContext context,
								IContractsRepository cntRep,
								IAirportsRepository airRep,
								IAircraftsRepository acrftRep,
								IItemsRepository itemsRep,
								ICurrenciesRepository currRep,
								IInvoicesRepository invRep)
			: base(context)
		{
			_contractsRepository = cntRep;
			_airportsRepository = airRep;
			_aircraftsRepository = acrftRep;
			_itemsRepository = itemsRep;
			_currenciesRepository = currRep;
			_invoicesRepository = invRep;
		}

		public override Order FindById(int id)
		{
			var query = (from i in DefaultSet
						 where i.Id == id
						 select i).SingleOrDefault();

			return query;
		}

		public Order Create(OrderSettings orderSettings)
		{
			// Pick the contract and traders
			Contract contract = _contractsRepository.FindById(orderSettings.ContractId);
			if (contract == null)
				throw new Exception("Invalid contract orderId [" + orderSettings.ContractId + "]!!!");

			// Sanity checking
			Trader buyer = null;
			Trader seller = null;

			if (orderSettings.BuyerIndexInContract == 0)
			{
				buyer = contract.TraderOne;
				seller = contract.TraderTwo;
			}
			else
			{
				buyer = contract.TraderTwo;
				seller = contract.TraderOne;
			}

			if (buyer == null || seller == null)
				throw new Exception("Unable to resolve the buyer or seller!!!");

			if (buyer.Id == seller.Id)
				throw new Exception("A buyer and seller cannot be the same!!!");

			if (buyer.CreditCards.Count == 0 && orderSettings.BuyerCreditCard == null)
				throw new Exception("The buyer has no credit cards!!!");

			if (seller.CreditCards.Count == 0 && orderSettings.SellerCreditCard == null)
				throw new Exception("The seller has no credit cards!!!");

			CreditCard bCreditCard = null;
			CreditCard sCreditCard = null;
			if (orderSettings.BuyerCreditCard != null)
				bCreditCard = orderSettings.BuyerCreditCard;
			else
				bCreditCard = buyer.CreditCards.First();

			if (orderSettings.SellerCreditCard != null)
				sCreditCard = orderSettings.SellerCreditCard;
			else
				sCreditCard = seller.CreditCards.First();

			if (bCreditCard == null || sCreditCard == null)
				throw new Exception("Unable to resolve the buyer or seller credit card!!!");

			Airport landingAirport = _airportsRepository.FindById(orderSettings.LandingAirportId);
			Airport takeOffAirport = _airportsRepository.FindById(orderSettings.TakeOffAirportId);
			Aircraft aircraft = _aircraftsRepository.FindById(orderSettings.AircraftId);

			// Generate the order
			Order order = new Order();
			order.OrderDate = orderSettings.OrderDate;
			order.QuotationDate = orderSettings.OrderDate;
			order.IsQuoted = false;
			order.ApprovalDate = orderSettings.OrderDate;
			order.IsApproved = false;
			order.FulfilmentDate = orderSettings.OrderDate;
			order.IsFulfilled = false;

			order.Status = Order.Submitted;

			order.Contract = contract;
			order.Buyer = buyer;
			order.Seller = seller;

			order.Amount = 0;
			order.AdminFee = 0;
			order.PivotExchangeRate = 0;
			order.GlobalPivotExchangeRate = 0;

			order.BuyerCardHolderName = bCreditCard.HolderName;
			order.BuyerCardNumber = bCreditCard.CardNumber;
			order.BuyerCardExpDate = bCreditCard.ExpDate;
			order.BuyerCardSecurityCode = bCreditCard.SecurityCode;
			order.BuyerPaypalUserId = "";
			order.BuyerPaypalPassword = "";
			order.BuyerReferenceNumber = "";

			order.SellerCardHolderName = sCreditCard.HolderName;
			order.SellerCardNumber = sCreditCard.CardNumber;
			order.SellerCardExpDate = sCreditCard.ExpDate;
			order.SellerCardSecurityCode = sCreditCard.SecurityCode;
			order.SellerPaypalUserId = "";
			order.SellerPaypalPassword = "";
			order.SellerReferenceNumber = "";

			order.TakeoffAirport = takeOffAirport;
			order.LandingAirport = landingAirport;
			order.Aircraft = aircraft;

            if (orderSettings.Operateur == null)
                order.Operator = "";
            else
                order.Operator = orderSettings.Operateur;

            if (orderSettings.FlightNumber == null)
                order.FlightNumber = "";
            else
                order.FlightNumber = orderSettings.FlightNumber;

            if (orderSettings.EstimateTakeOff == null || orderSettings.EstimateTakeOff.Year < DateTime.Now.Year)
				order.EstimatedTakeoffTime = order.OrderDate.AddDays(new Random().Next(7));

            if (orderSettings.EstimateLanding == null || orderSettings.EstimateLanding.Year < DateTime.Now.Year)
                order.EstimatedLandingTime = order.EstimatedTakeoffTime.AddHours(new Random().Next(7));

			_Context.Orders.Add(order);

			// Now attach the line items
			AttachLineItems(ref order, orderSettings.LineItems);

			_Context.SaveChanges();

			return order;
		}

		public void Quote(int id, DateTime quoteDate, List<OrderLineItemDto> lineItems)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			order.QuotationDate = quoteDate;
			order.IsQuoted = true;
			order.Status = Order.Quoted;

			// Now attach the line items
			AttachLineItems(ref order, lineItems);

			_Context.SaveChanges();
		}

		public void Approve(int id, DateTime approveDate)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			// TODO: Charge the credit cards
			// TODO: Update the buyer and seller reference numbers
			order.BuyerReferenceNumber = Utilities.GenerateRandomNumber(12);
			order.SellerReferenceNumber = Utilities.GenerateRandomNumber(12);

			order.ApprovalDate = approveDate;
			order.IsApproved = true;
			order.Status = Order.Approved;

			_Context.SaveChanges();

			// Generate an invoice from seller to buyer i.e. using the seller's invoice number generation scheme
			_invoicesRepository.Create(order, order.Seller.GenerateInvoiceNumber(approveDate));
		}

		public void Fulfill(int id, DateTime flflDate)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			order.FulfilmentDate = flflDate;
			order.IsFulfilled = true;
			order.Status = Order.Fulfilled;

			_Context.SaveChanges();
		}

		public void Ignore(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			if (order.Status != Order.Submitted && order.Status != Order.Quoted)
				throw new Exception("Order [" + id + "] can no longer be ignored!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderIgnore orderIgnore = FindOrderIgnoreByOrderAndUser(order, user);
			if (orderIgnore == null)
			{
				orderIgnore = new OrderIgnore();
				orderIgnore.User = user;
				orderIgnore.Order = order;
				orderIgnore.IgnoreDate = DateTime.Now;
				_Context.OrderIgnores.Add(orderIgnore);
			}
			else
			{
				orderIgnore.IgnoreDate = DateTime.Now;
			}

			_Context.SaveChanges();
		}

		public void ResetIgnore(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderIgnore orderIgnore = FindOrderIgnoreByOrderAndUser(order, user);
			if (orderIgnore != null)
			{
				_Context.OrderIgnores.Remove(orderIgnore);
				_Context.SaveChanges();
			}
		}

		public bool IsIgnored(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderIgnore orderIgnore = FindOrderIgnoreByOrderAndUser(order, user);
			if (orderIgnore != null)
				return true;
			else
				return false;
		}

		public void View(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderView orderView = FindOrderViewByOrderAndUser(order, user);
			if (orderView == null)
			{
				orderView = new OrderView();
				orderView.User = user;
				orderView.Order = order;
				orderView.Status = order.Status;
				orderView.ViewDate = DateTime.Now;
				_Context.OrderViews.Add(orderView);
			}
			else
			{
				orderView.ViewDate = DateTime.Now;
			}

			_Context.SaveChanges();
		}

		public void ResetView(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderView orderView = FindOrderViewByOrderAndUser(order, user);
			if (orderView != null)
			{
				_Context.OrderViews.Remove(orderView);
				_Context.SaveChanges();
			}
		}

		public bool IsViewed(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderView orderView = FindOrderViewByOrderAndUser(order, user);
			if (orderView != null)
				return true;
			else
				return false;
		}

		public void Archive(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderArchive orderArchive = FindOrderArchiveByOrderAndUser(order, user);
			if (orderArchive == null)
			{
				orderArchive = new OrderArchive();
				orderArchive.User = user;
				orderArchive.Order = order;
				orderArchive.ArchiveDate = DateTime.Now;
				_Context.OrderArchives.Add(orderArchive);
			}
			else
			{
				orderArchive.ArchiveDate = DateTime.Now;
			}

			_Context.SaveChanges();
		}

		public void ResetArchive(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderArchive orderArchive = FindOrderArchiveByOrderAndUser(order, user);
			if (orderArchive != null)
			{
				_Context.OrderArchives.Remove(orderArchive);
				_Context.SaveChanges();
			}
		}

		public bool IsArchived(int id, string userId)
		{
			Order order = FindById(id);
			if (order == null)
				throw new Exception("Order [" + id + "] is not valid!");

			User user = FindUserByLoginId(userId);
			if (user == null)
				throw new Exception("User [" + userId + "] is not valid!");

			OrderArchive orderArchive = FindOrderArchiveByOrderAndUser(order, user);
			if (orderArchive != null)
				return true;
			else
				return false;
		}

		public List<GenericIntegerIdStringName> RetrievePartnerTraders(int partnerId, DateTime filterByTransactionDate)
		{
			var traders = new List<GenericIntegerIdStringName>();
			var buyers = _Context.Database.SqlQuery<GenericIntegerIdStringName>(
				" SELECT t.Id, t.Name " +
				" FROM Orders o, Traders t  " +
				" WHERE o.BuyerId = " + partnerId +
				" AND o.TransactionDate >= " + filterByTransactionDate +
				" AND o.BuyerId = t.Id " +
				" GROUP BY t.Id, t.Name "
				).ToList();

			traders.AddRange(buyers);

			var sellers = _Context.Database.SqlQuery<GenericIntegerIdStringName>(
				" SELECT t.Id, t.Name " +
				" FROM Orders o, Traders t  " +
				" WHERE o.SellerId = " + partnerId +
				" AND o.TransactionDate >= " + filterByTransactionDate +
				" AND o.SellerId = t.Id " +
				" GROUP BY t.Id, t.Name "
				).ToList();

			traders.AddRange(sellers);
			return traders;
		}

		/* P R I V A T E  M E T H O D S */

		private User FindUserByLoginId(string login)
		{
			var user = (from i in _Context.Users
						where i.Login == login
						select i).SingleOrDefault();

			if (user == null && login == "System")
			{
				user = new User();
				user.Name = "System";
				user.Login = "System";
				user.Password = Utilities.GenerateRandomNumber(128);
				user.Email = "system@system.com";
				user.Group = FindSystemGroup();
				user.Role = FindSystemRole();
				_Context.Users.Add(user);
				_Context.SaveChanges();
			}

			return user;
		}

		private OrderView FindOrderViewByOrderAndUser(Order order, User user)
		{
			var query = (from i in _Context.OrderViews.Include("User").Include("Order")
						 where (i.Order.Id == order.Id && i.User.Id == user.Id && i.Status == order.Status)
						 select i).SingleOrDefault();

			return query;
		}

		private OrderArchive FindOrderArchiveByOrderAndUser(Order order, User user)
		{
			var query = (from i in _Context.OrderArchives.Include("User").Include("Order")
						 where (i.Order.Id == order.Id && i.User.Id == user.Id)
						 select i).SingleOrDefault();

			return query;
		}

		private OrderIgnore FindOrderIgnoreByOrderAndUser(Order order, User user)
		{
			var query = (from i in _Context.OrderIgnores.Include("User").Include("Order")
						 where (i.Order.Id == order.Id && i.User.Id == user.Id)
						 select i).SingleOrDefault();

			return query;
		}

		private OrderLineItem FindLineItemByOrderAndItemAndCurrency(Order order, Item item, Currency currency)
		{
			var query = (from i in _Context.OrderLineItems.Include("Item").Include("Currency").Include("Order")
						 where (i.Order.Id == order.Id && i.Item.Id == item.Id && i.Currency.Id == currency.Id)
						 select i).SingleOrDefault();

			return query;
		}

		private Group FindSystemGroup()
		{
			var query = (from i in _Context.Groups
						 where (i.Id == Group.AviTrade)
						 select i).SingleOrDefault();

			return query;
		}

		private Role FindSystemRole()
		{
			var query = (from i in _Context.Roles
						 where (i.Id == Role.Admins)
						 select i).SingleOrDefault();

			return query;
		}

		private void AttachLineItems(ref Order order, List<OrderLineItemDto> lineItems)
		{
			double totalOrderAmount = 0;
			foreach (OrderLineItemDto lineItemDto in lineItems)
			{
				Item item = _itemsRepository.FindById(lineItemDto.ItemId);
				Currency currency = _currenciesRepository.FindById(lineItemDto.CurrencyId);
				double[] rates = Utilities.GetExchangeRates(currency, order.Contract.Instance.PivotCurrency, order.Contract.Instance.GlobalPivotCurrency);

				if (item != null && currency != null)
				{
					OrderLineItem orderLineItem = FindLineItemByOrderAndItemAndCurrency(order, item, currency);
					if (orderLineItem != null)
					{
						orderLineItem.PricePerUnit = lineItemDto.PricePerUnit;
						orderLineItem.Amount = orderLineItem.PricePerUnit * orderLineItem.Units;
						orderLineItem.AdminFee = (orderLineItem.Amount * order.Contract.Instance.AdminFeePercentage) / 100;
						orderLineItem.PivotExchangeRate = rates[0];
						orderLineItem.GlobalPivotExchangeRate = rates[1];
					}
					else
					{
						orderLineItem = new OrderLineItem();
						orderLineItem.FulfilmentDate = DateTime.Now;
						orderLineItem.Units = lineItemDto.Units;
						orderLineItem.Currency = currency;
						orderLineItem.Item = item;
						orderLineItem.Order = order;
						orderLineItem.PricePerUnit = lineItemDto.PricePerUnit;
						orderLineItem.Amount = orderLineItem.PricePerUnit * orderLineItem.Units;
						orderLineItem.AdminFee = (orderLineItem.Amount * order.Contract.Instance.AdminFeePercentage) / 100;
						orderLineItem.PivotExchangeRate = rates[0];
						orderLineItem.GlobalPivotExchangeRate = rates[1];
						orderLineItem.Instructions = lineItemDto.Instructions;
                        if (orderLineItem.Instructions == null)
                            orderLineItem.Instructions = "";
						_Context.OrderLineItems.Add(orderLineItem);
					}

					totalOrderAmount += (orderLineItem.Units * (orderLineItem.PricePerUnit / orderLineItem.Currency.Rate)) * order.Contract.BillingCurrency.Rate;
				}
			}

			double[] orderRates = Utilities.GetExchangeRates(order.Contract.BillingCurrency, order.Contract.Instance.PivotCurrency, order.Contract.Instance.GlobalPivotCurrency);
			order.Amount = totalOrderAmount;
			order.AdminFee = (totalOrderAmount * order.Contract.Instance.AdminFeePercentage) / 100;
			order.PivotExchangeRate = orderRates[0];
			order.GlobalPivotExchangeRate = orderRates[1];
		}
	}
}