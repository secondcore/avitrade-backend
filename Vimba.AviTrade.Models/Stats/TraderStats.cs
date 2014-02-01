using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models.Stats
{
    public class TimeSlotsEquates
    {
        public const int TimeSlotLast7Days = 0;
        public const int TimeSlotLast30Days = 1;
        public const int TimeSlotLast60Days = 2;
        public const int TimeSlotLast90Days = 3;
        public const int TimeSlotLast180Days = 4;
        public const int TimeSlotLast12Months = 5;
        public const int TimeSlotYearToDate = 6;
        public const int TimeSlotMonthToDate = 7;
    }

    public class TraderStats
    {
        public int BuyerPendingOrdersCount { get; set; }
        public int SellerPendingOrdersCount { get; set; }
        public double BuyerPendingOrdersAmount { get; set; }
        public double SellerPendingOrdersAmount { get; set; }

        public int BuyerApprovedOrdersCount { get; set; }
        public int SellerApprovedOrdersCount { get; set; }
        public double BuyerApprovedOrdersAmount { get; set; }
        public double SellerApprovedOrdersAmount { get; set; }

        public List<TraderItemStats> TopSoldItems { get; set; }
        public List<TraderItemStats> TopBoughtItems { get; set; }

        public TraderStats()
        {
            BuyerPendingOrdersCount = 0;
            SellerPendingOrdersCount = 0;
            BuyerPendingOrdersAmount = 0;
            BuyerPendingOrdersCount = 0;

            BuyerApprovedOrdersCount = 0;
            SellerApprovedOrdersCount = 0;
            BuyerApprovedOrdersAmount = 0;
            SellerApprovedOrdersAmount = 0;

            TopSoldItems = new List<TraderItemStats>();
            TopBoughtItems = new List<TraderItemStats>();
        }
    }

    public class TraderExecutivesDashboard
    {
        public List<TradingPartnerStats> TopTradingPartnersAsBuyer { get; set; }
        public List<TraderStatusCountAmountAndDays> OrdersByStatusAsBuyer { get; set; }
        public List<TraderStatusAndCount> OrdersByConversionRateAsBuyer { get; set; }
        public int TotalOrdersAsBuyer { get; set; }

        public List<TradingPartnerStats> TopTradingPartnersAsSeller { get; set; }
        public List<TraderStatusCountAmountAndDays> OrdersByStatusAsSeller { get; set; }
        public List<TraderStatusAndCount> OrdersByConversionRateAsSeller { get; set; }
        public int TotalOrdersAsSeller { get; set; }
    }

    public class MebaaExecutivesDashboard
    {
        public List<MebaaItemStats> TopItems { get; set; }
        public List<MebaaTopTrader> TopBuyers { get; set; }
        public List<MebaaTopTrader> TopSellers { get; set; }
        public List<MebaaStatusCountAmountAndDays> OrdersByStatus { get; set; }
        public List<TraderStatusAndCount> OrdersByConversionRates { get; set; }
        public int TotalOrders { get; set; }
    }

    public class MebaaTradersSummaries
    {
        public List<TraderSummary> Buyers { get; set; }
        public List<TraderSummary> Sellers { get; set; }
    }

    public class TraderItemStats
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Item { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
    }

    public class MebaaItemStats
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Item { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
        public double AdminFee { get; set; }
    }

    public class TradingPartnerStats
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
    }

    public class MebaaTopTrader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
        public double AdminFee { get; set; }
    }

    public class TraderStatusAndCount
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class TraderStatusCountAmountAndDays
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
        public int AvgDays { get; set; }
    }

    public class MebaaStatusCountAmountAndDays
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
        public double AdminFee { get; set; }
        public int AvgDays { get; set; }
    }

    public class TraderSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
        public double Amount { get; set; }
        public double AdminFee { get; set; }
    }
}
