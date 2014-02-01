using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        User FindByLogin(string login);
    }
}