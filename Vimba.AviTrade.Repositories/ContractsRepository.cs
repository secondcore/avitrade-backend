using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class ContractsRepository : GenericRepository<Contract>, IContractsRepository
    {
        protected override IQueryable<Contract> DefaultSet
        {
            get
            {
                return _Context.Contracts
                    .Include("TraderOne.CreditCards")
                    .Include("TraderOne.Country.Region")
                    .Include("TraderTwo.CreditCards")
                    .Include("TraderTwo.Country.Region")
                    .Include("Instance.PivotCurrency")
                    .Include("Instance.GlobalPivotCurrency")
                    .Include("BillingCurrency")
                    .Include("TimeZone");
            }
        }

        protected override Func<Contract, object> DefaultOrderBy
        {
            get
            {
                return x => x.CreateDate;
            }
        }

        public ContractsRepository(AviTradeContext context) : base(context) { }

        public override Contract FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public List<Contract> RetrievePartnerTraders(int traderId, DateTime boundaryDate)
        {
            return (from t in _Context.Contracts
                    where (
                           (t.TraderOne.Id == traderId || t.TraderTwo.Id == traderId) &&
                           t.EndDate >= boundaryDate
                           )
                    orderby t.Name
                    select t).ToList();
        }

        /* P R I V A T E  M E T H O D S */
    }
}
