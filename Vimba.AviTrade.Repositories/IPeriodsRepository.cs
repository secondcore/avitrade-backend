using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public interface IPeriodsRepository : IGenericRepository<Period>
    {
        Period FindByYearAndMonth(int year, int month);
        Period FindByDate(DateTime date);
    }
}
