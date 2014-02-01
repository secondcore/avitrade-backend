using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Web.Areas.Mebaa.ViewModels;
using Vimba.AviTrade.Web.Areas.Traders.ViewModels;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class SessionHelper
    {
        public static User Authenticated
        {
            get
            {
                var actualUser = (User)HttpContext.Current.Session["AuthenticatedUser"];
                if (actualUser == null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var repository = DependencyResolver.Current.GetService(typeof(IUsersRepository)) as IUsersRepository;
                    actualUser = repository.FindByLogin(HttpContext.Current.User.Identity.Name);
                    Authenticated = actualUser;
                }
                return actualUser;
            }
            set
            {
                HttpContext.Current.Session["AuthenticatedUser"] = value;
            }
        }

        public static ContractsViewModel ContractsViewModel
        {
            get
            {
                var model = (ContractsViewModel)HttpContext.Current.Session["ContractsViewModel"];
                var authUser = Authenticated;
                if (model == null)
                {
                    var tradersRepository = DependencyResolver.Current.GetService(typeof(ITradersRepository)) as ITradersRepository;
                    if (tradersRepository != null)
                    {
                        var trader = tradersRepository.FindByRegistrationToken(authUser.TraderCode);
                        if (trader != null)
                        {
                            model = new ContractsViewModel()
                                        {
                                            TraderId = trader.Id,
                                            Page = 1,
                                            Count = 20,
                                            MaxPage = 1,
                                            ActiveContracts = new List<Contract>(),
                                            ExpiringContracts = new List<Contract>(),
                                            PendingContracts = new List<Contract>()
                                        };
                        }
                    }
                }

                return model;
            }
            set
            {
                HttpContext.Current.Session["ContractsViewModel"] = value;
            }
        }

        public static OrdersExecutivesDashboardViewModel TraderExecutivesDashboardModel
        {
            get
            {
                var model = (OrdersExecutivesDashboardViewModel)HttpContext.Current.Session["TraderExecutivesDashboardViewModel"];
                var authUser = Authenticated;
                if (model == null)
                {
                    var tradersRepository = DependencyResolver.Current.GetService(typeof(ITradersRepository)) as ITradersRepository;
                    if (tradersRepository != null)
                    {
                        var trader = tradersRepository.FindByRegistrationToken(authUser.TraderCode);
                        if (trader != null)
                        {
                            model = new OrdersExecutivesDashboardViewModel();
                            model.Trader = trader;
                            model.Dashboard = tradersRepository.RetrieveTraderExecutivesDashboard(trader.Id, TimeSlotsEquates.TimeSlotLast12Months); // Default the time slot 
                        }
                    }
                }

                return model;
            }
            set
            {
                HttpContext.Current.Session["TraderExecutivesDashboardViewModel"] = value;
            }
        }

        public static OrdersMebaaExecutivesDashboardViewModel MebaaExecutivesDashboardViewModel
        {
            get
            {
                var model = (OrdersMebaaExecutivesDashboardViewModel)HttpContext.Current.Session["MebaaExecutivesDashboardViewModel"];
                var authUser = Authenticated;
                if (model == null)
                {
                    var tradersRepository = DependencyResolver.Current.GetService(typeof(ITradersRepository)) as ITradersRepository;
                    if (tradersRepository != null)
                    {
                        model = new OrdersMebaaExecutivesDashboardViewModel();
                        model.Dashboard = tradersRepository.RetrieveMebaaExecutivesDashboard(TimeSlotsEquates.TimeSlotLast12Months); // Default the time slot 
                    }
                }

                return model;
            }
            set
            {
                HttpContext.Current.Session["MebaaExecutivesDashboardViewModel"] = value;
            }
        }

        public static MebaaTradersSummariesViewModel MebaaTradersSummariesViewModel
        {
            get
            {
                var model = (MebaaTradersSummariesViewModel)HttpContext.Current.Session["MebaaTradersSummariesViewModel"];
                var authUser = Authenticated;
                if (model == null)
                {
                    var tradersRepository = DependencyResolver.Current.GetService(typeof(ITradersRepository)) as ITradersRepository;
                    if (tradersRepository != null)
                    {
                        model = new MebaaTradersSummariesViewModel();
                        model.Summaries = tradersRepository.RetrieveTraderSummaries(TimeSlotsEquates.TimeSlotLast12Months); // Default the time slot 
                    }
                }

                return model;
            }
            set
            {
                HttpContext.Current.Session["MebaaTradersSummariesViewModel"] = value;
            }
        }
    }
}