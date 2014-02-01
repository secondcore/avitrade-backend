using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public interface IInvoicesRepository : IGenericRepository<Invoice>
    {
        Invoice FindByOrderId(int orderId);
        Invoice Create(Order order, string invoiceNumber);
    }
}
