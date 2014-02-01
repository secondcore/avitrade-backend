using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class ContentHelper
    {
        public static MvcHtmlString Static(this WebViewPage page, string url)
        {
            string resultUrl = page.Href(url);
            var staticServer = (string)System.Configuration.ConfigurationManager.AppSettings["StaticServer"];
            if (!string.IsNullOrWhiteSpace(staticServer))
            {
                var contentUrl = new Uri(page.Context.Request.Url, resultUrl);
                var server = new Uri(staticServer);
                resultUrl = GetStaticUrl(server, contentUrl).ToString();
            }
            return MvcHtmlString.Create(resultUrl);
        }

        private static Uri GetStaticUrl(Uri server, Uri contentUrl)
        {
            return new Uri(server, string.Format("/{0}{1}", contentUrl.GetSubDomain(), contentUrl.AbsolutePath));
        }

        private static string GetSubDomain(this Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }

            return string.Empty;
        }
    }
}