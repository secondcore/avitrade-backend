using System;
using System.Collections.Generic;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Web.ViewModels
{
    public class AjaxOrdersViewModel
    {
        public int TraderId { get; set; }

        public int FilterByStatus { get; set; }
        public string FilterByTraderIds { get; set; }
        public DateTime FilterByOrderDate { get; set; }
        public bool FilterBySeller { get; set; }

		public int Count { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int MaxPage { get; set; }
        public List<OrderViewModel> Items { get; set; }
    }
}