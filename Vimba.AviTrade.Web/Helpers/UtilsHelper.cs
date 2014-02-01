using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class UtilsHelper
    {
        public static string Stringfy(this IEnumerable<string> list)
        {
            return string.Join(",", list);
        }

        public static void Add(this RouteValueDictionary dictionary, object obj)
        {
            obj.GetType().GetProperties().ToList()
                .ForEach(p => dictionary.Add(p.Name, p.GetValue(obj, null)));
        }
    }
}