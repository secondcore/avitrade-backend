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
using System.ServiceModel.Web;
using System.Web;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Repositories;

namespace Vimba.AviTrade.Web.Services
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)] // Allow me to see the request errors if any
    public class AviTradeODataService : DataService<AviTradeContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true; // Allows me to make the errors verbose
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            //config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        [WebGet]
        public IQueryable<Order> TopOrdersByYear(int year, int max)
        {
            var result = (from o in CurrentDataSource.Orders
                          select o).Take(max);
            return result;
        }
    }
}
