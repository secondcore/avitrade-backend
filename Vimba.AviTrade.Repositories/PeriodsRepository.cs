using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class PeriodsRepository : GenericRepository<Period>, IPeriodsRepository
    {
        protected override IQueryable<Period> DefaultSet
        {
            get
            {
                return _Context.Periods;
            }
        }

        protected override Func<Period, object> DefaultOrderBy
        {
            get
            {
                return x => x.Name;
            }
        }

        public PeriodsRepository(AviTradeContext context) : base(context) { }

        public override Period FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public Period FindByYearAndMonth(int year, int month)
        {
            var query = (from i in DefaultSet
                         where (i.Year == year && i.Month == month)
                         select i).SingleOrDefault();

            return query;
        }

        public Period FindByDate(DateTime date)
        {
            int month = date.Month;
            int year = date.Year;
            Period period = null;

            try
            {
                period = (from c in _Context.Periods
                          where (c.Month == date.Month && c.Year == date.Year)
                          select c).FirstOrDefault();

                if (period == null)
                {
                    period = new Period();
                    period.Month = month;
                    period.Year = year;
                    period.Name = date.ToString("MM yyyy");
                    _Context.Periods.Add(period);
                    _Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception("FindPeriodByDate caused an exception: " + e.Message);
            }

            return period;
        }
    }
}
