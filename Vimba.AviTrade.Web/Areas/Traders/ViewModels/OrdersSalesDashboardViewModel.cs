using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Web.Areas.Traders.ViewModels
{
    public class OrdersSalesDashboardViewModel
    {
        public List<Order> SubmittedByMeAndWaitingToBeQuotedOrders { get; set; }
        public List<Order> SubmittedByOthersAndWaitingOnMeToQuote { get; set; }
        public List<Order> ApprovedOrdersByMeOrOthers { get; set; }
    }
}