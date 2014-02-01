using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vimba.AviTrade.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "AviTrade Rocks!!!!";
            return View();
        }

        public ActionResult Features()
        {
            ViewBag.Message = "Your features page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TimeSavingsInnovationArticle()
        {
            return View();
        }

        public ActionResult TakeOffArticle()
        {
            return View();
        }

    }
}
