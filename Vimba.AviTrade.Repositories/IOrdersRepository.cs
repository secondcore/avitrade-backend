using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Settings;
using Vimba.AviTrade.Models.Stats;

namespace Vimba.AviTrade.Repositories
{
    public interface IOrdersRepository : IGenericRepository<Order>
    {
        Order Create(OrderSettings orderSettings);

        void Quote(int id, DateTime quoteDate, List<OrderLineItemDto> lineItems);
        void Approve(int id, DateTime approveDate);
        void Fulfill(int id, DateTime flflDate);
        void Ignore(int id, string userId);
        void ResetIgnore(int id, string userId);
        bool IsIgnored(int id, string userId);
        void View(int id, string userId);
        void ResetView(int id, string userId);
        bool IsViewed(int id, string userId);
        void Archive(int id, string userId);
        void ResetArchive(int id, string userId);
        bool IsArchived(int id, string userId);

        // Query Methods
        List<GenericIntegerIdStringName> RetrievePartnerTraders(int partnerId, DateTime filterByTransactionDate);
    }
}
