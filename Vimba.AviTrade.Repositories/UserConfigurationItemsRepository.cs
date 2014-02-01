using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class UserConfigurationItemsRepository : GenericRepository<UserConfigurationItem>, IUserConfigurationItemsRepository
    {
        protected override IQueryable<UserConfigurationItem> DefaultSet
        {
            get
            {
                return _Context.UserConfigurationItems
                    .Include("User");
            }
        }

        protected override Func<UserConfigurationItem, object> DefaultOrderBy
        {
            get
            {
                return x => x.Key;
            }
        }

        public UserConfigurationItemsRepository(AviTradeContext context)
            : base(context)
        {
        }

        public override UserConfigurationItem FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public UserConfigurationItem FindByKeyAndValue(string key, string value)
        {
            var query = (from i in DefaultSet
                         where (i.Key == key && i.Value == value)
                         select i).SingleOrDefault();

            return query;
        }
    }
}
