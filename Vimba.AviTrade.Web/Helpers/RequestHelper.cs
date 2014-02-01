using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class RequestHelper
    {
        public static CultureInfo GetClientCulture(this HttpRequestBase request)
        {
            //since globalzation is auto in web.config, it return Curretn threadUI culture
            return System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        private const string TimeZoneKey = "___CLientTimeZone___";
        public static TimeZoneInfo GetClientTimeZone(this HttpRequestBase request)
        {
            var contextItems = request.RequestContext.HttpContext.Items;
            var clientTimeZone = contextItems[TimeZoneKey] as TimeZoneInfo;
            if (clientTimeZone == null)
            {
                var cookie = request.Cookies["TimeZoneOffset"];
                if (cookie != null)
                {
                    var offset = new TimeSpan(0, Int32.Parse(cookie.Value), 0);
                    clientTimeZone = TimeZoneInfo.CreateCustomTimeZone("Browser", offset, "Browser", "Browser");
                }
                else
                {
                    clientTimeZone = TimeZoneInfo.Local;
                }
                contextItems.Add(TimeZoneKey, clientTimeZone);
            }
            return clientTimeZone;
        }

        public static string GetAbsoluteUrl(this HttpRequestBase request, string relative)
        {
            var separator = relative.Length > 0 ? (relative.First() == '/' ? string.Empty : "/") : "/";
            return string.Format("{0}://{1}{2}{3}", request.Url.Scheme, request.Url.Host, separator, relative);
        }
    }
}