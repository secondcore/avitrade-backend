using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class RolesRepository : GenericRepository<Role>, IRolesRepository
    {
        protected override IQueryable<Role> DefaultSet
        {
            get { return _Context.Roles;}
        }

        protected override Func<Role, object> DefaultOrderBy
        {
            get
            {
                return x => x.Name;
            }
        }

        public RolesRepository(AviTradeContext context)
            : base(context)
        {
        }

        public override Role FindById(string id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public Role FindByRegistrationToken(string token)
        {
            var query = (from i in _Context.TraderRegistrationTokens.Include("Role")
                         where i.Token == token
                         select i).FirstOrDefault();

            if (query != null)
                return query.Role;
            else
                return null;
        }
    }
}
