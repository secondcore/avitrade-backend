using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Security.Server;
namespace LightSwitchApplication
{
    public partial class AviTradeOLTPDataService
    {
        partial void ApprovedOrdersByTrader_PreprocessQuery(string TraderAccount, ref IQueryable<Order> query)
        {
            query = query.Take(5);
        }

        partial void SubmittedOrdersByTrader_PreprocessQuery(string TraderAccount, ref IQueryable<Order> query)
        {
            query = query.Take(5);
        }

        partial void WaitingToBeQuotedOrdersByTrader_PreprocessQuery(string TraderAccount, ref IQueryable<Order> query)
        {
            query = query.Take(5);
        }

        partial void AllTraders_PreprocessQuery(string LoggedInTrader, ref IQueryable<Trader> query)
        {
            // TODO: this.Application.LoggedInTrader.Name!!! Why doesn't work? I had to pass it as a parameter!!!
            // TODO: This should actually return the DISTINCT tarders that this trader is involved with ...whether buying or selling
            //query = query.Where(i => i.Name != LoggedInTrader).Select(i => i); 
            //This is no longer needed as I had to pass the LoggedInTrader name as a parameter. So I modified the query 
            //to exclude the logged in trader.
            //Nevertheless, I think I need to add the DISTINCT stuff...so I will keep it for a while.
        }
    }
}
