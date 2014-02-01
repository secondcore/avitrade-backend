using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class AirportsRepository : GenericRepository<Airport>, IAirportsRepository
    {
        protected override IQueryable<Airport> DefaultSet
        {
            get
            {
                return _Context.Airports.Include("Country.Region");
            }
        }

        protected override Func<Airport, object> DefaultOrderBy
        {
            get
            {
                return x => x.City;
            }
        }

        public AirportsRepository(AviTradeContext context) : base(context) { }

        public override Airport FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            if (query == null)
            {
                query = (from i in DefaultSet
                         where i.Name == "Unknown"
                         select i).SingleOrDefault();
            }

            return query;
        }

        /* P R I V A T E  M E T H O D S */
    }
}
