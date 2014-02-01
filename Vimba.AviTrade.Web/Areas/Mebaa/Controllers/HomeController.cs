using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Web.Areas.Mebaa.ViewModels;
using Vimba.AviTrade.Web.Areas.Traders.ViewModels;
using Vimba.AviTrade.Web.Helpers;

namespace Vimba.AviTrade.Web.Areas.Mebaa.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ITradersRepository _tradersRepository;

        public HomeController(ITradersRepository trdRep)
        {
            _tradersRepository = trdRep;
        }

        public ActionResult Index()
        {
            var authUser = SessionHelper.Authenticated;
            if (authUser == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

            var model = SessionHelper.MebaaExecutivesDashboardViewModel;
            if (model == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The mebaa executives dashboard model cannot be instantiated!");

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(int timeSlotId)
        {
            // TODO: Since the Form in the view does not surround the entire model, we are not getting the entire model data!!!!
            var model = SessionHelper.MebaaExecutivesDashboardViewModel;
            if (ModelState.IsValid)
            {
                foreach (GenericIntegerIdStringName timeSlot in model.TimeSlots)
                {
                    if (timeSlot.Id == timeSlotId)
                        model.SelectedTimeSlot = timeSlot;
                }

                // Build the executives dashboard
                model = BuildMebaaDashboard(model);
                // Store the model in the session
                SessionHelper.MebaaExecutivesDashboardViewModel = model;
            }
            else
            {
                ModelState.AddModelError("", "The mebaa dashboard View Model is not valid!!");
            }

            return View(model);
        }

        public ActionResult TradersSummaries()
        {
            var authUser = SessionHelper.Authenticated;
            if (authUser == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The user is not authenticated!");

            var model = SessionHelper.MebaaTradersSummariesViewModel;
            if (model == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The mebaa traders summaries model cannot be instantiated!");

            return View(model);
        }

        [HttpPost]
        public ActionResult TradersSummaries(int timeSlotId)
        {
            // TODO: Since the Form in the view does not surround the entire model, we are not getting the entire model data!!!!
            var model = SessionHelper.MebaaTradersSummariesViewModel;
            if (ModelState.IsValid)
            {
                foreach (GenericIntegerIdStringName timeSlot in model.TimeSlots)
                {
                    if (timeSlot.Id == timeSlotId)
                        model.SelectedTimeSlot = timeSlot;
                }

                // Build the executives dashboard
                model = BuildMebaaTradersSummaries(model);
                // Store the model in the session
                SessionHelper.MebaaTradersSummariesViewModel = model;
            }
            else
            {
                ModelState.AddModelError("", "The mebaa traders summaries View Model is not valid!!");
            }

            return View(model);
        }

        // P R I V A T E   M E T H O D S
        private OrdersMebaaExecutivesDashboardViewModel BuildMebaaDashboard(OrdersMebaaExecutivesDashboardViewModel model)
        {
            model.Dashboard = _tradersRepository.RetrieveMebaaExecutivesDashboard(model.SelectedTimeSlot.Id); // Uisng the time slot in model
            return model;
        }

        private MebaaTradersSummariesViewModel BuildMebaaTradersSummaries(MebaaTradersSummariesViewModel model)
        {
            model.Summaries = _tradersRepository.RetrieveTraderSummaries(model.SelectedTimeSlot.Id); // Uisng the time slot in model
            return model;
        }
    }
}
