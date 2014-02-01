using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;

namespace Vimba.AviTrade.Web.Areas.Traders.ViewModels
{
    public class TraderDetailViewModel
    {
        public Trader Trader { get; set; }
        public TraderStats Stats { get; set; }
    }
}