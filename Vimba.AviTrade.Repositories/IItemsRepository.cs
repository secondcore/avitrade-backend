using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public interface IItemsRepository : IGenericRepository<Item>
    {
        List<string> RetrieveCategories();
    }
}
