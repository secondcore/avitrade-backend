using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class GroupsRepository : GenericRepository<Group>, IGroupsRepository
    {
        protected override IQueryable<Group> DefaultSet
        {
            get { return _Context.Groups;}
        }

        protected override Func<Group, object> DefaultOrderBy
        {
            get
            {
                return x => x.Name;
            }
        }

        public GroupsRepository(AviTradeContext context)
            : base(context)
        {
        }

        public override Group FindById(string id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }
    }
}
