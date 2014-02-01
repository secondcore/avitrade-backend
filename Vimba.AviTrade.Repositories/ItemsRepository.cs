using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class ItemsRepository : GenericRepository<Item>, IItemsRepository
    {
        protected override IQueryable<Item> DefaultSet
        {
            get
            {
                return _Context.Items;
            }
        }

        protected override Func<Item, object> DefaultOrderBy
        {
            get
            {
                return x => x.SubCategory;
            }
        }

        public ItemsRepository(AviTradeContext context) : base(context) { }

        public override Item FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public List<string> RetrieveCategories()
        {
            var query = (from i in DefaultSet
                         select i.Category).Distinct();

            return query.ToList();
        }

        /* P R I V A T E  M E T H O D S */
    }
}
