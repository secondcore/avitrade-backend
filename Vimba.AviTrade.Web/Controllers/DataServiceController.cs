using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Settings;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Web.Areas.Traders.ViewModels;
using Vimba.AviTrade.Web.ViewModels;

namespace Vimba.AviTrade.Web.Controllers
{
	/// <summary>
	/// This controller is expected to be called remotely from JavaScript AJAX calls. Hence it always returns Json data.
	/// </summary>
	public class DataServiceController : Controller
	{
		private IContractsRepository _contractsRepository = null;
		private ITradersRepository _tradersRepository = null;
		private IOrdersRepository _ordersRepository = null;
		private IInvoicesRepository _invoicesRepository = null;
		private IItemsRepository _itemsRepository = null;
		private IAirportsRepository _airportsRepository = null;
		private IAircraftsRepository _aircraftsRepository = null;
        private ICurrenciesRepository _currenciesRepository = null;

		public DataServiceController(IContractsRepository cntrcRep,
									 ITradersRepository trdrRep,
									 IOrdersRepository ordRep,
									 IInvoicesRepository invRep,
									 IItemsRepository itmRep,
									 IAirportsRepository airRep,
									 IAircraftsRepository acrRep,
                                     ICurrenciesRepository curRep)
		{
			_contractsRepository = cntrcRep;
			_tradersRepository = trdrRep;
			_ordersRepository = ordRep;
			_invoicesRepository = invRep;
			_itemsRepository = itmRep;
			_airportsRepository = airRep;
			_aircraftsRepository = acrRep;
            _currenciesRepository = curRep;
        }

		// Item Categories
		public ActionResult RetrieveItemCategories()
		{
			var categories = _itemsRepository.RetrieveCategories(); // No Pagination
			return Json(categories, JsonRequestBehavior.AllowGet);
		}

		// Items
		public ActionResult RetrieveItems(string filterByCatagory)
		{
			Expression<Func<Item, bool>> filter = c => (c.Category == filterByCatagory);

			var items = _itemsRepository.Search(filter, 0, 10).ToList(); // No Pagination
			return Json(items, JsonRequestBehavior.AllowGet);
		}

		// Airports
		public ActionResult RetrieveAirports(string filterByCountry = "")
		{
			var airports = new List<Airport>();

			if (filterByCountry == "")
			{
				airports = _airportsRepository.GetAll().ToList();
			}
			else
			{
				Expression<Func<Airport, bool>> filter = c => (c.Country.Id == filterByCountry);
				airports = _airportsRepository.Search(filter, 0, 10).ToList(); // No Pagination
			}

			return Json(airports, JsonRequestBehavior.AllowGet);
		}

		// Aircrafts
		public ActionResult RetrieveAircrafts()
		{
			var aircrafts = _aircraftsRepository.GetAll().ToList();
			return Json(aircrafts, JsonRequestBehavior.AllowGet);
		}

        // Currencies
        public ActionResult RetrieveCurrencies()
        {
            var currs = _currenciesRepository.GetAll().ToList(); // No Pagination
            return Json(currs, JsonRequestBehavior.AllowGet);
        }

        // Contracts
		public ActionResult RetrieveContracts(int filterByTraderId, DateTime filterByEndDate)
		{
            // Active contracts only
			Expression<Func<Contract, bool>> filter = c => ((c.TraderOne.Id == filterByTraderId || c.TraderTwo.Id == filterByTraderId) &&
															c.EndDate >= (filterByEndDate == null ? DateTime.Now : filterByEndDate));

			var contracts = _contractsRepository.Search(filter, 0, 10).ToList(); // No Pagination
			var contractModels = new List<ContractViewModel>();
			foreach (Contract contract in contracts)
			{
				contractModels.Add(new ContractViewModel(filterByTraderId, contract));
			}
			return Json(contractModels, JsonRequestBehavior.AllowGet);
		}

        public ActionResult RetrieveTradersByCountry(string filterByCountry)
		{
			Expression<Func<Trader, bool>> filter = t => (t.Country.Id == filterByCountry);

			var traders = _tradersRepository.Search(filter, 0, 10).ToList();
			return Json(traders, JsonRequestBehavior.AllowGet);
		}

		// Return all DISTINCT partner traders that transacted with the traderId after the filtereByTransactionDate
		public ActionResult RetrieveTradersByPartner(int traderId, DateTime filterByTransactionDate)
		{
			var traders = _ordersRepository.RetrievePartnerTraders(traderId, filterByTransactionDate).ToList();
			return Json(traders, JsonRequestBehavior.AllowGet);
		}

		public ActionResult RetrieveOrders(int traderId, DateTime filterByOrderDate)
		{
			var model = BuildAjaxOrdersViewModel(traderId, filterByOrderDate);
			return Json(model, JsonRequestBehavior.AllowGet);
		}

        // Work in progress
		public ActionResult RetrieveArchivedOrders(int traderId, int filterByStatus, string filterByTraderIds, DateTime filterByOrderDate, bool filterBySeller, int page, int pageSize)
		{
			var model = BuildAjaxOrdersViewModel(traderId, filterByStatus, filterByTraderIds, filterByOrderDate, filterBySeller, page, pageSize);
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		// Must be the initial call to retrieve the order creaion process...we are returning the OrderSeetings which is used to create orders
		public ActionResult StartOrderCreation(int traderId)
		{
			var model = new OrderSettings();
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		public ActionResult CreateOrder(int traderId, OrderSettings settings)
		{
            //TODO: KLUDGE - determine the order buyer index from the settings contract
		    var contract = _contractsRepository.FindById(settings.ContractId);
            if (contract.TraderOne.Id == traderId)
                settings.BuyerIndexInContract = 0;
            else
                settings.BuyerIndexInContract = 1;
            // END OF KLUDGE

            var order = _ordersRepository.Create(settings);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateOrder(int traderId, OrderSettings settings)
		{
			throw new Exception("Not supported yet!!!");

			//var order = _ordersRepository.Update(settings);
			//return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

        public ActionResult RetrieveOrder(int traderId, int orderId)
        {
            var order = _ordersRepository.FindById(orderId);
            return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuoteOrder(int traderId, int orderId, DateTime quoteDate, List<OrderLineItemDto> lineItems)
		{
			_ordersRepository.Quote(orderId, quoteDate, lineItems);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ApproveOrder(int traderId, int orderId, DateTime approveDate)
		{
			_ordersRepository.Approve(orderId, approveDate);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult FulfillOrder(int traderId, int orderId, DateTime flflDate)
		{
			_ordersRepository.Fulfill(orderId, flflDate);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult IgnoreOrder(int traderId, int orderId, string userId)
		{
			_ordersRepository.Ignore(orderId, userId);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ResetIgnoreOrder(int traderId, int orderId, string userId)
		{
			_ordersRepository.ResetIgnore(orderId, userId);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ViewOrder(int traderId, int orderId, string userId)
		{
			_ordersRepository.View(orderId, userId);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ResetViewOrder(int traderId, int orderId, string userId)
		{
			_ordersRepository.ResetView(orderId, userId);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ArchiveOrder(int traderId, int orderId, string userId)
		{
			_ordersRepository.Archive(orderId, userId);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ResetArchiveOrder(int traderId, int orderId, string userId)
		{
			_ordersRepository.ResetArchive(orderId, userId);
			var order = _ordersRepository.FindById(orderId);
			return Json(new OrderViewModel(traderId, order), JsonRequestBehavior.AllowGet);
		}

		public ActionResult RetrieveOrderInvoice(int orderId)
		{
			var invoice = _invoicesRepository.FindByOrderId(orderId);
			return Json(new InvoiceViewModel(invoice), JsonRequestBehavior.AllowGet);
		}

		/* P R I V A T E */

		// Refactored for testing!!! Should be made private! But I am using this from a test case
		public AjaxOrdersViewModel BuildAjaxOrdersViewModel(int traderId, DateTime filterByOrderDate)
		{
			if (filterByOrderDate == null)
				throw new Exception("Boundary Date is mandatory!!");

			Expression<Func<Order, bool>> filter = o => (o.Buyer.Id == traderId || o.Seller.Id == traderId) && o.OrderDate.Month == filterByOrderDate.Month && o.OrderDate.Year == filterByOrderDate.Year;
			var model = new AjaxOrdersViewModel();
			model.Items = new List<OrderViewModel>();
			var orders = _ordersRepository.Search(filter, 0, 0, o => o.OrderDate, true).ToList();
			foreach (Order order in orders)
			{
				model.Items.Add(new OrderViewModel(traderId, order));
			}

			return model;
		}

		// Refactored for testing!!! Should be made private! But I am using this from a test case
		public AjaxOrdersViewModel BuildAjaxOrdersViewModel(int traderId, int filterByStatus, string filterByTraderIds,
															 DateTime filterByOrderDate, bool filterBySeller, int page,
															 int pageSize)
		{
			if (filterByOrderDate == null)
				throw new Exception("Boundary Date is mandatory!!");

			List<int> intTraderIds = RetrieveTraderIds(filterByTraderIds);
			Expression<Func<Order, bool>> filter = null;
			if (filterByStatus == Order.Submitted)
			{
				filter = o => ((filterBySeller ? o.Seller.Id == traderId : o.Buyer.Id == traderId) &&

					//((intTraderIds.Count == 0) ? true : IsInTraderList(intTraderIds, o.Seller.Id)) &&
							   o.Status == Order.Submitted &&
							   o.OrderDate >= filterByOrderDate);
			}
			else if (filterByStatus == Order.Quoted)
			{
				filter = o => ((filterBySeller ? o.Seller.Id == traderId : o.Buyer.Id == traderId) &&

					//((intTraderIds.Count == 0) ? true : IsInTraderList(intTraderIds, o.Buyer.Id)) &&
							   o.Status == Order.Quoted &&
							   o.OrderDate >= filterByOrderDate);
			}
			else if (filterByStatus == Order.Approved)
			{
				filter = o => ((filterBySeller ? o.Seller.Id == traderId : o.Buyer.Id == traderId) &&

					//(((intTraderIds.Count == 0) ? true : IsInTraderList(intTraderIds, o.Buyer.Id)) ||
					//((intTraderIds.Count == 0) ? true : IsInTraderList(intTraderIds, o.Seller.Id))) &&
							   o.Status == Order.Approved &&
							   o.OrderDate >= filterByOrderDate);
			}
			else if (filterByStatus == Order.Fulfilled)
			{
				filter = o => ((filterBySeller ? o.Seller.Id == traderId : o.Buyer.Id == traderId) &&

					//(((intTraderIds.Count == 0) ? true : IsInTraderList(intTraderIds, o.Buyer.Id)) ||
					//((intTraderIds.Count == 0) ? true : IsInTraderList(intTraderIds, o.Seller.Id))) &&
							   o.Status == Order.Fulfilled &&
							   o.OrderDate >= filterByOrderDate);
			}

			var model = new AjaxOrdersViewModel();
			model.FilterByOrderDate = filterByOrderDate;
			model.FilterByStatus = filterByStatus;
			model.FilterByTraderIds = filterByTraderIds;
			model.FilterBySeller = filterBySeller;
			model.Items = new List<OrderViewModel>();
			var orders = _ordersRepository.Search(filter, page, pageSize, o => o.OrderDate, true).ToList();
			foreach (Order order in orders)
			{
				model.Items.Add(new OrderViewModel(traderId, order));
			}

			// Paginated and always descending by order date
			model.Page = page;
			model.Count = _ordersRepository.Count(filter);
			model.MaxPage = Math.Max((int)Math.Ceiling((double)_ordersRepository.Count(filter) / pageSize), 1);
			model.PageSize = pageSize;
			return model;
		}

        /* P R I V A T E */

		private List<int> RetrieveTraderIds(string filterByTraderIds)
		{
			ArrayList list = new ArrayList();
			list.AddRange(filterByTraderIds.Split(new char[] { ',' }));

			List<int> intTraderIds = new List<int>();
			foreach (string traderId in list)
			{
				if (traderId == "") continue;
				intTraderIds.Add(Int32.Parse(traderId));
			}

			return intTraderIds;
		}

		private bool IsInTraderList(List<int> ids, int id)
		{
			foreach (int i in ids)
			{
				if (i == id)
					return true;
			}

			return false;
		}
	}
}