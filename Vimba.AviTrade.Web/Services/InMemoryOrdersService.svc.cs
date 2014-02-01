//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using System.Web;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.ServiceModels;
using Vimba.AviTrade.Repositories.DataSources;

namespace Vimba.AviTrade.Web.Services
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)] // Allow me to see the request errors if any
    public class InMemoryOrdersService : DataService<OrdersDataSource>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true; // Allows me to make the errors verbose
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            config.SetEntitySetAccessRule("Orders", EntitySetRights.AllRead);
            // Examples:
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            //config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;

            // Read the orders in memory - while I do have access to the 'CurrentDataSource', I can't use it because this is a static method 
            // So the only way to do this would be in the constructor of the 'OrdersDataSource'...yach!
            //if (CurrentDataSource.Orders == null)
            //{

            //}
        }

        [QueryInterceptor("Orders")]
        public Expression<Func<OrderServiceModel, bool>> OnQueryOrders()
        {
            return order => order.AirplaneManufacturer == "Boeing";
        }

        [WebGet]
        public IQueryable<OrderServiceModel> TopOrdersByYear(int year, int max)
        {
            var result = (from o in CurrentDataSource.Orders
                          select o).Take(max);
            return result;
        }
    }
}
