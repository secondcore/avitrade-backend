using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class AircraftsRepository : GenericRepository<Aircraft>, IAircraftsRepository
    {
        protected override IQueryable<Aircraft> DefaultSet
        {
            get
            {
                return _Context.Aircrafts;
            }
        }

        protected override Func<Aircraft, object> DefaultOrderBy
        {
            get
            {
                return x => x.Manufacturer;
            }
        }

        public AircraftsRepository(AviTradeContext context) : base(context) { }

        public override Aircraft FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            if (query == null)
            {
                query = (from i in DefaultSet
                         where i.Manufacturer == "Unknown"
                         select i).SingleOrDefault();
            }

            return query;
        }

        /* P R I V A T E  M E T H O D S */
    }
}
