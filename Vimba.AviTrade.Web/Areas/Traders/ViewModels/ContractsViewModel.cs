using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Web.Areas.Traders.ViewModels
{
    public class ContractsViewModel
    {
        public int TraderId { get; set; }

        public int Page { get; set; }
        public int Count { get; set; }
        public int MaxPage { get; set; }
        public IEnumerable<Contract> ActiveContracts { get; set; }
        public IEnumerable<Contract> ExpiringContracts { get; set; }
        public IEnumerable<Contract> PendingContracts { get; set; }
    }
}