using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models.ServiceModels;

namespace Vimba.AviTrade.Repositories.DataSources
{
    public class OrdersDataSource
    {
        public OrdersDataSource()
        {
        }

        public IQueryable<OrderServiceModel> Orders
        {
            //TODO: Do a huge query to populate the in-memory orders - yach!!!
            get { return new List<OrderServiceModel>().AsQueryable(); }
        }
    }
}
