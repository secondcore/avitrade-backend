using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;
using Vimba.AviTrade.Repositories.Helpers;

namespace Vimba.AviTrade.Repositories
{
    public class TradersRepository : GenericRepository<Trader>, ITradersRepository
    {
        protected override IQueryable<Trader> DefaultSet
        {
            get
            {
                return _Context.Traders;
            }
        }

        protected override Func<Trader, object> DefaultOrderBy
        {
            get
            {
                return x => x.Name;
            }
        }

        public TradersRepository(AviTradeContext context) : base(context) { }

        public override Trader FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public Trader FindByName(string name)
        {
            var query = (from i in DefaultSet
                         where i.Name == name
                         select i).SingleOrDefault();

            return query;
        }

        public Trader FindByRegistrationToken(string token)
        {
            var query = (from i in _Context.TraderRegistrationTokens.Include("Trader")
                         where i.Token == token
                         select i).FirstOrDefault();

            if (query != null)
                return query.Trader;
            else
                return null;
        }

		public void CreateRegistrationTokens()
        {
            var traders = (from i in DefaultSet
                             select i).ToList();

            foreach (Trader mer in traders)
            {
                // Create one token for each role
                string[] roles =
                    {
                        Role.Admins, Role.Executives, Role.Finance, Role.IT, Role.Marketing, Role.Pilot, Role.Sales,
                        Role.Support
                    };

                foreach (string role in roles)
                {
                    CreateRegistrationToken(mer, role);
                }
            }
        }

        public void CreateRegistrationToken(int id, string role)
        {
            Trader trader = FindById(id);
            if (trader != null)
            {
                CreateRegistrationToken(trader, role);
                // TODO: Send an email to the merchant letting him know that the registration token changed
            }
        }

        public Trader IsRegistrationTokenValid(string token)
        {
            var existingToken = (from i in _Context.TraderRegistrationTokens.Include("Trader").Include("Role")
                                 where i.Token == token
                                 select i).FirstOrDefault();

            if (existingToken == null)
                return null;

            var lastToken = (from i in _Context.TraderRegistrationTokens.Include("Trader").Include("Role")
                             where (i.Trader.Id == existingToken.Trader.Id && i.Role.Id == existingToken.Role.Id)
                             orderby i.CreateDate descending
                             select i).FirstOrDefault();

            if (lastToken != null && lastToken.Token == token)
                return lastToken.Trader;
            else
                return null;
        }

        public TraderStats RetrieveTraderStats(int id, int timeSlot = TimeSlotsEquates.TimeSlotLast12Months)
        {
            TraderStats stats = new TraderStats();

            DateTime[] dates = RetrieveTimSlotDates(timeSlot);
            DateTime fromDate = dates[0];
            DateTime toDate = dates[1];


            // As a buyer
            var sql = " SELECT o.IsApproved, COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount " +
                      " FROM Orders o, Traders t " +
                      " WHERE o.BuyerId = t.Id " +
                      " AND t.Id = " + id +
                      " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                      " GROUP BY o.IsApproved " +
                      " ORDER BY o.IsApproved";
            var buyerStats = _Context.Database.SqlQuery<CountAndAmount>(sql);

            foreach (CountAndAmount amt in buyerStats)
            {
                if (!amt.IsApproved)
                {
                    stats.BuyerPendingOrdersCount = amt.Count;
                    stats.BuyerPendingOrdersAmount = amt.Amount;
                }
                else
                {
                    stats.BuyerApprovedOrdersCount = amt.Count;
                    stats.BuyerApprovedOrdersAmount = amt.Amount;
                }
            }

            // As a seller
            sql = " SELECT o.IsApproved, COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount " +
                  " FROM Orders o, Traders t " +
                  " WHERE o.SellerId = t.Id " +
                  " AND t.Id = " + id +
                  " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                  " GROUP BY o.IsApproved " +
                  " ORDER BY o.IsApproved";
            var sellerStats = _Context.Database.SqlQuery<CountAndAmount>(sql);

            foreach (CountAndAmount amt in sellerStats)
            {
                if (!amt.IsApproved)
                {
                    stats.SellerPendingOrdersCount = amt.Count;
                    stats.SellerPendingOrdersAmount = amt.Amount;
                }
                else
                {
                    stats.SellerApprovedOrdersCount = amt.Count;
                    stats.SellerApprovedOrdersAmount = amt.Amount;
                }
            }

            // Top 3 sold items
            sql = " SELECT TOP 3 i.Category, i.SubCategory, i.Name as Item, " + 
                  " COUNT(*) as Count, SUM(ol.Amount / ol.GlobalPivotExchangeRate) as Amount " + 
                  " FROM Orders o, OrderLineItems ol, Traders t, Items i " +  
                  " WHERE ol.OrderId = o.Id " + 
                  " AND o.Status >= " + Order.Approved +  
                  " AND o.SellerId = t.Id " + 
                  " AND t.Id = " + id +  
                  " AND ol.ItemId = i.Id " +
                  " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                  " GROUP BY i.Category, i.SubCategory, i.Name " + 
                  " ORDER BY count(*) DESC ";
            stats.TopSoldItems = _Context.Database.SqlQuery<TraderItemStats>(sql).ToList();

            // Top 3 bought items
            sql = " SELECT TOP 3 i.Category, i.SubCategory, i.Name as Item, " +
                  " COUNT(*) as Count, SUM(ol.Amount / ol.GlobalPivotExchangeRate) as Amount " +
                  " FROM Orders o, OrderLineItems ol, Traders t, Items i " +
                  " WHERE ol.OrderId = o.Id " +
                  " AND o.Status >= " + Order.Approved +
                  " AND o.BuyerId = t.Id " +
                  " AND t.Id = " + id +
                  " AND ol.ItemId = i.Id " +
                  " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                  " GROUP BY i.Category, i.SubCategory, i.Name " +
                  " ORDER BY count(*) DESC ";
            stats.TopBoughtItems = _Context.Database.SqlQuery<TraderItemStats>(sql).ToList();

            return stats;
        }

        public TraderExecutivesDashboard RetrieveTraderExecutivesDashboard(int traderId, int timeSlot)
        {
            TraderExecutivesDashboard stats = new TraderExecutivesDashboard();

            DateTime[] dates = RetrieveTimSlotDates(timeSlot);
            DateTime fromDate = dates[0];
            DateTime toDate = dates[1];

            // As a buyer
            // Get the top trading partners
            var sql = " SELECT TOP 3 p.Id, p.Name, COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount " +
                      " FROM Orders o, Traders t, Traders p " +
                      " WHERE o.BuyerId = t.Id " +
                      " AND t.Id = " + traderId +
                      " AND o.SellerId = p.Id " +
                      " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                      " GROUP BY p.Id, p.Name " +
                      " ORDER BY SUM(o.Amount / o.GlobalPivotExchangeRate) DESC  ";
            stats.TopTradingPartnersAsBuyer = _Context.Database.SqlQuery<TradingPartnerStats>(sql).ToList();

            // Get the orders by status
            sql =   " SELECT o.Status as StatusId, Status = CASE o.Status WHEN 0 THEN 'Submitted' WHEN 1 THEN 'Quoted' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Fulfilled' END," +
                    " COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, " +
                    " AvgDays = CASE o.Status " + 
			        "   WHEN 0 THEN AVG(DATEDIFF(dd, o.OrderDate, o.OrderDate)) " +
			        "   WHEN 1 THEN AVG(DATEDIFF(dd, o.OrderDate, o.QuotationDate)) " +
			        "   WHEN 2 THEN AVG(DATEDIFF(dd, o.QuotationDate, o.ApprovalDate)) " +
			        "   WHEN 3 THEN AVG(DATEDIFF(dd, o.ApprovalDate, o.FulfilmentDate)) " +
		            " END " + 
                    " FROM Orders o, Traders t " +
                    " WHERE o.BuyerId = t.Id " +
                    " AND t.Id = " + traderId +
                    " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY o.Status " +
                    " ORDER BY o.Status  ";
            stats.OrdersByStatusAsBuyer = _Context.Database.SqlQuery<TraderStatusCountAmountAndDays>(sql).ToList();

            // Get the conversion ratio by status
            sql =   " SELECT o.Status as StatusId, Status = CASE o.Status WHEN 0 THEN 'Submitted' WHEN 1 THEN 'Quoted' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Fulfilled' END," +
                    " COUNT(*) as Count " +
                    " FROM Orders o, Traders t " +
                    " WHERE o.BuyerId = t.Id " +
                    " AND t.Id = " + traderId +
                    " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY o.Status " +
                    " ORDER BY o.Status  ";
            stats.OrdersByConversionRateAsBuyer = _Context.Database.SqlQuery<TraderStatusAndCount>(sql).ToList();
            foreach (TraderStatusAndCount sts in stats.OrdersByConversionRateAsBuyer)
                stats.TotalOrdersAsBuyer += sts.Count;

            // As a seller
            // Get the top trading partners
            sql = " SELECT TOP 3 p.Id, p.Name, COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount " +
                    " FROM Orders o, Traders t, Traders p " +
                    " WHERE o.SellerId = t.Id " +
                    " AND t.Id = " + traderId +
                    " AND o.BuyerId = p.Id " +
                    " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY p.Id, p.Name " +
                    " ORDER BY SUM(o.Amount / o.GlobalPivotExchangeRate) DESC  ";
            stats.TopTradingPartnersAsSeller = _Context.Database.SqlQuery<TradingPartnerStats>(sql).ToList();

            // Get the orders by status
            sql =   " SELECT o.Status as StatusId, Status = CASE o.Status WHEN 0 THEN 'Submitted' WHEN 1 THEN 'Quoted' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Fulfilled' END," +
                    " COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, " +
                    " AvgDays = CASE o.Status " + 
			        "   WHEN 0 THEN AVG(DATEDIFF(dd, o.OrderDate, o.OrderDate)) " +
			        "   WHEN 1 THEN AVG(DATEDIFF(dd, o.OrderDate, o.QuotationDate)) " +
			        "   WHEN 2 THEN AVG(DATEDIFF(dd, o.QuotationDate, o.ApprovalDate)) " +
			        "   WHEN 3 THEN AVG(DATEDIFF(dd, o.ApprovalDate, o.FulfilmentDate)) " +
		            " END " + 
                    " FROM Orders o, Traders t " +
                    " WHERE o.SellerId = t.Id " +
                    " AND t.Id = " + traderId +
                    " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY o.Status " +
                    " ORDER BY o.Status  ";
            stats.OrdersByStatusAsSeller = _Context.Database.SqlQuery<TraderStatusCountAmountAndDays>(sql).ToList();

            // Get the conversion ratio by status
            sql =   " SELECT o.Status as StatusId, Status = CASE o.Status WHEN 0 THEN 'Submitted' WHEN 1 THEN 'Quoted' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Fulfilled' END," +
                    " COUNT(*) as Count " +
                    " FROM Orders o, Traders t " +
                    " WHERE o.SellerId = t.Id " +
                    " AND t.Id = " + traderId +
                    " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY o.Status " +
                    " ORDER BY o.Status  ";
            stats.OrdersByConversionRateAsSeller = _Context.Database.SqlQuery<TraderStatusAndCount>(sql).ToList();
            foreach (TraderStatusAndCount sts in stats.OrdersByConversionRateAsSeller)
                stats.TotalOrdersAsSeller += sts.Count;

            return stats;
        }

        public MebaaExecutivesDashboard RetrieveMebaaExecutivesDashboard(int timeSlot)
        {
            MebaaExecutivesDashboard stats = new MebaaExecutivesDashboard();

            DateTime[] dates = RetrieveTimSlotDates(timeSlot);
            DateTime fromDate = dates[0];
            DateTime toDate = dates[1];

            // Get the top buyers
            var sql = " SELECT TOP 3 t.Id, t.Name, COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, SUM(o.AdminFee / o.GlobalPivotExchangeRate) as AdminFee " +
                      " FROM Orders o, Traders t " +
                      " WHERE o.BuyerId = t.Id " +
                      " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                      " GROUP BY t.Id, t.Name " +
                      " ORDER BY SUM(o.Amount / o.GlobalPivotExchangeRate) DESC  ";
            stats.TopBuyers = _Context.Database.SqlQuery<MebaaTopTrader>(sql).ToList();

            // Get the top sellers
            sql =     " SELECT TOP 3 t.Id, t.Name, COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, SUM(o.AdminFee / o.GlobalPivotExchangeRate) as AdminFee " +
                      " FROM Orders o, Traders t " +
                      " WHERE o.SellerId = t.Id " +
                      " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                      " GROUP BY t.Id, t.Name " +
                      " ORDER BY SUM(o.Amount / o.GlobalPivotExchangeRate) DESC  ";
            stats.TopSellers = _Context.Database.SqlQuery<MebaaTopTrader>(sql).ToList();

            // Top Items
            sql = " SELECT TOP 5 i.Category, i.SubCategory, i.Name as Item, " +
                  " COUNT(*) as Count, SUM(ol.Amount / ol.GlobalPivotExchangeRate) as Amount, SUM(ol.AdminFee / ol.GlobalPivotExchangeRate) as AdminFee " +
                  " FROM Orders o, OrderLineItems ol, Items i " +
                  " WHERE ol.OrderId = o.Id " +
                  " AND o.Status >= " + Order.Approved +
                  " AND ol.ItemId = i.Id " +
                  " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                  " GROUP BY i.Category, i.SubCategory, i.Name " +
                  " ORDER BY count(*) DESC ";
            stats.TopItems = _Context.Database.SqlQuery<MebaaItemStats>(sql).ToList();

            // Get the orders by status
            sql = " SELECT o.Status as StatusId, Status = CASE o.Status WHEN 0 THEN 'Submitted' WHEN 1 THEN 'Quoted' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Fulfilled' END," +
                    " COUNT(*) as Count, SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, SUM(o.AdminFee / o.GlobalPivotExchangeRate) as AdminFee, " +
                    " AvgDays = CASE o.Status " +
                    "   WHEN 0 THEN AVG(DATEDIFF(dd, o.OrderDate, o.OrderDate)) " +
                    "   WHEN 1 THEN AVG(DATEDIFF(dd, o.OrderDate, o.QuotationDate)) " +
                    "   WHEN 2 THEN AVG(DATEDIFF(dd, o.QuotationDate, o.ApprovalDate)) " +
                    "   WHEN 3 THEN AVG(DATEDIFF(dd, o.ApprovalDate, o.FulfilmentDate)) " +
                    " END " +
                    " FROM Orders o " +
                    " WHERE o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY o.Status " +
                    " ORDER BY o.Status  ";
            stats.OrdersByStatus = _Context.Database.SqlQuery<MebaaStatusCountAmountAndDays>(sql).ToList();

            // Get the conversion rates by status
            sql = " SELECT o.Status as StatusId, Status = CASE o.Status WHEN 0 THEN 'Submitted' WHEN 1 THEN 'Quoted' WHEN 2 THEN 'Approved' WHEN 3 THEN 'Fulfilled' END," +
                    " COUNT(*) as Count " +
                    " FROM Orders o " +
                    " WHERE o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                    " GROUP BY o.Status " +
                    " ORDER BY o.Status  ";
            stats.OrdersByConversionRates = _Context.Database.SqlQuery<TraderStatusAndCount>(sql).ToList();
            foreach (TraderStatusAndCount sts in stats.OrdersByConversionRates)
                stats.TotalOrders += sts.Count;

            return stats;
        }

        public MebaaTradersSummaries RetrieveTraderSummaries(int timeSlot)
        {
            MebaaTradersSummaries stats = new MebaaTradersSummaries();

            DateTime[] dates = RetrieveTimSlotDates(timeSlot);
            DateTime fromDate = dates[0];
            DateTime toDate = dates[1];

            // Get the buyers summaries 
            var sql = " SELECT t.Id, t.Name, " + 
                      " count(*) as Orders, " + 
                      " SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, " + 
                      " SUM(o.AdminFee / o.GlobalPivotExchangeRate) as AdminFee " + 
                      " FROM Orders o, Traders t " + 
                      " WHERE o.BuyerId = t.Id " +
                      " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                      " GROUP BY t.Id, t.Name " +
                      " ORDER BY SUM(o.Amount / o.GlobalPivotExchangeRate) DESC ";
            stats.Buyers = _Context.Database.SqlQuery<TraderSummary>(sql).ToList();

            // Get the sellers summaries 
            sql =     " SELECT t.Id, t.Name, " +
                      " count(*) as Orders, " +
                      " SUM(o.Amount / o.GlobalPivotExchangeRate) as Amount, " +
                      " SUM(o.AdminFee / o.GlobalPivotExchangeRate) as AdminFee " +
                      " FROM Orders o, Traders t " +
                      " WHERE o.SellerId = t.Id " +
                      " AND o.OrderDate BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                      " GROUP BY t.Id, t.Name " +
                      " ORDER BY SUM(o.Amount / o.GlobalPivotExchangeRate) DESC ";
            stats.Sellers = _Context.Database.SqlQuery<TraderSummary>(sql).ToList();

            return stats;
        }

        /* P R I V A T E  M E T H O D S */

        private void CreateRegistrationToken(Trader mer, string roleId)
        {
            Role role = FindRoleById(roleId);
            if (role == null)
                role = FindRoleById(Role.Sales); // Default to the minimum security

            TraderRegistrationToken token = new TraderRegistrationToken();
            token.CreateDate = DateTime.Now;
            token.Trader = mer;
            token.Role = role;
            token.Token = mer.Id + "-" + Utilities.GenerateRandomNumber(8) + "-" + roleId.ToUpper();
            _Context.TraderRegistrationTokens.Add(token);
            _Context.SaveChanges();
        }

        private Role FindRoleById(string id)
        {
            var query = (from i in _Context.Roles
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        private DateTime [] RetrieveTimSlotDates(int timeSlot)
        {
            DateTime[] dates = new DateTime[2];

            if (timeSlot == TimeSlotsEquates.TimeSlotLast7Days) // Last 7 days
            {
                dates[0] = DateTime.Now.AddDays(-7);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotLast30Days) // Last 30 days
            {
                dates[0] = DateTime.Now.AddDays(-30);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotLast60Days) // Last 60 days
            {
                dates[0] = DateTime.Now.AddDays(-60);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotLast90Days) // Last 90 days
            {
                dates[0] = DateTime.Now.AddDays(-90);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotLast180Days) // Last 180 days
            {
                dates[0] = DateTime.Now.AddDays(-180);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotLast12Months) // Last 12 months
            {
                dates[0] = DateTime.Now.AddDays(-30 * 12);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotYearToDate) // YTD
            {
                //TODO: Year To Date in not calculated properly
                dates[0] = DateTime.Now.AddDays(-30 * 12);
                dates[1] = DateTime.Now;
            }
            else if (timeSlot == TimeSlotsEquates.TimeSlotMonthToDate) // MTD
            {
                //TODO: Month To Date in not calculated properly
                dates[0] = DateTime.Now.AddDays(-30 * 12);
                dates[1] = DateTime.Now;
            }

            return dates;
        }
    }

    class CountAndAmount
    {
        public bool IsApproved { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
    }
}
