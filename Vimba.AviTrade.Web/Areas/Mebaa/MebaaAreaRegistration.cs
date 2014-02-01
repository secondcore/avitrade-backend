using System.Web.Mvc;

namespace Vimba.AviTrade.Web.Areas.Mebaa
{
    public class MebaaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mebaa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mebaa_default",
                "Mebaa/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
