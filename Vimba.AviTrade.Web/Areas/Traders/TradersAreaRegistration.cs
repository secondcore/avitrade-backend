using System.Web.Mvc;

namespace Vimba.AviTrade.Web.Areas.Traders
{
    public class TradersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Traders";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Traders_default",
                "Traders/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
