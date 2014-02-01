using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;

namespace Vimba.AviTrade.Repositories
{
    public interface ITradersRepository : IGenericRepository<Trader>
    {
        Trader FindByName(string name);
        Trader FindByRegistrationToken(string token);
        void CreateRegistrationTokens();
        void CreateRegistrationToken(int id, string role);
        Trader IsRegistrationTokenValid(string token);
        TraderStats RetrieveTraderStats(int id, int timeSlot);

        TraderExecutivesDashboard RetrieveTraderExecutivesDashboard(int traderId, int timeSlot);
        MebaaExecutivesDashboard RetrieveMebaaExecutivesDashboard(int timeSlot);
        MebaaTradersSummaries RetrieveTraderSummaries(int timeSlot);
    }
}
