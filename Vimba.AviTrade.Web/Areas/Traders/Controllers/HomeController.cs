using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Web.Areas.Traders.ViewModels;
using Vimba.AviTrade.Web.Helpers;
using System.Linq.Expressions;
using Vimba.AviTrade.Web.ViewModels;

namespace Vimba.AviTrade.Web.Areas.Traders.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ITradersRepository _tradersRepository;
        private IOrdersRepository _ordersRepository;
        private IInvoicesRepository _invoicesRepository;
        private IContractsRepository _contractsRepository;

        public HomeController(ITradersRepository trdRep, IOrdersRepository ordRep, IInvoicesRepository invRep, IContractsRepository cntrRep)
        {
            _tradersRepository = trdRep;
            _ordersRepository = ordRep;
            _invoicesRepository = invRep;
            _contractsRepository = cntrRep;
        }

        /// <summary>
        /// ChildActionOnly prevents direct URL into this method i.e. /Traders/Home/TraderDetail
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult TraderDetail()
        {
            var authUser = SessionHelper.Authenticated;
            if (authUser == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

            var trader = _tradersRepository.FindByRegistrationToken(authUser.TraderCode);
            if (trader == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user trader code is not valid!");

            // Call upon the traders repository to return the trader stats
            TraderStats stats = _tradersRepository.RetrieveTraderStats(trader.Id, TimeSlotsEquates.TimeSlotLast12Months);
            if (stats == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user trader returned bad stats!");

            TraderDetailViewModel model = new TraderDetailViewModel()
            {
                Trader = trader,
                Stats = stats
            };

            return PartialView("TraderDetail", model);
        }

        public ActionResult Index()
        {
            var authUser = SessionHelper.Authenticated;
            if (authUser == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

            var trader = _tradersRepository.FindByRegistrationToken(authUser.TraderCode);
            if (trader == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user trader code is not valid!");

            if (authUser.Role.Id == Role.Executives)
            {
                var model = SessionHelper.TraderExecutivesDashboardModel;
                if (model == null)
                    throw new HttpException((int)HttpStatusCode.BadRequest, "The trader executives dashboard model cannot be instantiated!");

                return View("ExecutivesDashboard", model);
            }
            else
            {
                return View("SalesDashboard", BuildOrdersSalesDashboard(trader.Id));
            }
        }

        // The Orders page is managed via the Ext JS script.... and it communicates wit the DataServiceController to 
        // handle its pagination and other requirements. Hence we are only fetching the Trader Id for the Orders page.
        public ActionResult Orders()
        {
            var authUser = SessionHelper.Authenticated;
            if (authUser == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

            var trader = _tradersRepository.FindByRegistrationToken(authUser.TraderCode);
            if (trader == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user trader code is not valid!");

            return View(trader);
        }

        // Handle Executives Dashboard Time Slot Change Request
        [HttpPost]
        public ActionResult ExecutivesDashboard(int timeSlotId)
        {
            // TODO: Since the Form in the view does not surround the entire model, we are not getting the entire model data!!!!
            var model = SessionHelper.TraderExecutivesDashboardModel;
            if (ModelState.IsValid)
            {
                //var authUser = SessionHelper.Authenticated;
                //if (authUser == null)
                //    throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

                //var trader = _tradersRepository.FindByRegistrationToken(authUser.TraderCode);
                //if (trader == null)
                //    throw new HttpException((int)HttpStatusCode.BadRequest, "The user trader code is not valid!");

                //model.Trader = trader;

                foreach (GenericIntegerIdStringName timeSlot in model.TimeSlots)
                {
                    if (timeSlot.Id == timeSlotId)
                        model.SelectedTimeSlot = timeSlot;
                }

                // Build the executives dashboard
                model = BuildOrdersExecutivesDashboard(model);
                // Store the model in the session
                SessionHelper.TraderExecutivesDashboardModel = model;
            }
            else
            {
                ModelState.AddModelError("", "The executives dashboard View Model is not valid!!");
            }

            return View(model);
        }

        public ActionResult Contracts(int page = 1, int count = 20)
        {
            var authUser = SessionHelper.Authenticated;
            if (authUser == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

            var model = SessionHelper.ContractsViewModel;
            if (model == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The contracts model cannot be instantiated!");

            if (page < 1 || count < 1)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Page or Count parameter has to be greater than 1.");

            model.Page = page;
            model.Count = count;
            model = QueryContracts(model);

            return View(model);
        }

        // Handle Contracts Pagination Request
        [HttpPost]
        public ActionResult Contracts(ContractsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Perhaps we should reset the page to 1
                model.Page = 1;
                // Query the Contracts
                model = QueryContracts(model);
                // Store the model in the session
                SessionHelper.ContractsViewModel = model;
            }
            else
            {
                ModelState.AddModelError("", "The contracts View Model is not valid!!");
            }

            return View(model);
        }
        
        // P R I V A T E   M E T H O D S
        private ContractsViewModel QueryContracts(ContractsViewModel model)
        {
            DateTime expDate = DateTime.Now.AddDays(30);
            Expression<Func<Contract, bool>> filter = x => (x.TraderOne.Id == model.TraderId || x.TraderTwo.Id == model.TraderId) && x.EndDate > DateTime.Now;
            model.ActiveContracts = _contractsRepository.Search(filter, model.Page, model.Count).Select(x => x);
            filter = x => (x.TraderOne.Id == model.TraderId || x.TraderTwo.Id == model.TraderId) && x.EndDate > DateTime.Now && x.EndDate <= expDate;
            model.ExpiringContracts = _contractsRepository.Search(filter, model.Page, model.Count).Select(x => x);
            filter = x => (x.TraderOne.Id == model.TraderId || x.TraderTwo.Id == model.TraderId) && x.EndDate > DateTime.Now && (x.IsTraderOneApproved == false || x.IsTraderOneApproved == false);
            model.PendingContracts = _contractsRepository.Search(filter, model.Page, model.Count).Select(x => x);
            model.MaxPage = Math.Max((int)Math.Ceiling((double)_contractsRepository.Count(filter) / model.Count), 1);
            return model;
        }

        private OrdersSalesDashboardViewModel BuildOrdersSalesDashboard(int traderId, int count = 5)
        {
            List<Order> submittedByMeAndWaitingToBeQuotedOrders = BuildOrders(traderId, Order.Submitted, 1, count, true);
            List<Order> submittedByOthersAndWaitingOnMeToQuote = BuildOrders(traderId, Order.Submitted, 0, count, true);
            List<Order> approvedOrdersByMeOrOthers = BuildOrders(traderId, Order.Approved, 2, count, true);

            var model = new OrdersSalesDashboardViewModel();
            model.SubmittedByMeAndWaitingToBeQuotedOrders = submittedByMeAndWaitingToBeQuotedOrders;
            model.SubmittedByOthersAndWaitingOnMeToQuote = submittedByOthersAndWaitingOnMeToQuote;
            model.ApprovedOrdersByMeOrOthers = approvedOrdersByMeOrOthers;

            return model;
        }

        private OrdersExecutivesDashboardViewModel BuildOrdersExecutivesDashboard(OrdersExecutivesDashboardViewModel model)
        {
            model.Dashboard = _tradersRepository.RetrieveTraderExecutivesDashboard(model.Trader.Id, model.SelectedTimeSlot.Id); // Uisng the time slot in model
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="status"></param>
        /// <param name="traderPartnerType">0 = seller, 1 = buyer, 2 = either</param>
        /// <param name="numberOfItems"></param>
        /// <param name="orderBy">true = desc, false = asc</param>
        /// <returns></returns>
        public List<Order> BuildOrders(int traderId, int status, int traderPartnerType, int numberOfItems, bool orderBy)
        {
            List<Order> orders = new List<Order>();

            Expression<Func<Order, bool>> filter = null;
            if (traderPartnerType == 0)
                filter = o => o.Seller.Id == traderId && o.Status == status;
            else if (traderPartnerType == 1)
                filter = o => o.Buyer.Id == traderId && o.Status == status;
            else if (traderPartnerType == 2)
                filter = o => (o.Buyer.Id == traderId || o.Seller.Id == traderId) && o.Status == status;

            orders = _ordersRepository.Search(filter, 1, numberOfItems, o => o.OrderDate, orderBy).ToList();
            return orders;
        }
    }
}
