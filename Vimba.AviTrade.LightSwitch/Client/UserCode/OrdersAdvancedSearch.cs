using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
namespace LightSwitchApplication
{
    public partial class OrdersAdvancedSearch
    {
        partial void OrdersAdvancedSearch_InitializeDataWorkspace(List<IDataService> saveChangesTo)
        {
            // Write your code here.
            this.OrderFromDate = DateTime.Now.AddDays(-30);
            this.OrderToDate = DateTime.Now;
            this.OrderStatus = 0;
            this.IsBuyer = true;
            this.FindControl("SelectedSellerName").IsVisible = true;
            this.FindControl("SelectedBuyerName").IsVisible = false;
            this.LoggedInTrader = this.Application.LoggedInTrader.Name;
        }

        partial void IsBuyer_Changed()
        {
            if (this.IsBuyer)
            {
                this.FindControl("SelectedSellerName").IsVisible = true;
                this.FindControl("SelectedBuyerName").IsVisible = false;
            }
            else
            {
                this.FindControl("SelectedSellerName").IsVisible = false;
                this.FindControl("SelectedBuyerName").IsVisible = true;
            }
        }

        partial void OrderToDate_Changed()
        {
            this.OrderFromDate = ((DateTime)this.OrderToDate).AddDays(-30);
        }

        partial void SelectedBuyerName_Changed()
        {
            this.OrderBuyerName = this.SelectedBuyerName.Name;
            this.OrderSellerName = this.Application.LoggedInTrader.Name;
        }

        partial void SelectedSellerName_Changed()
        {
            this.OrderSellerName = this.SelectedSellerName.Name;
            this.OrderBuyerName = this.Application.LoggedInTrader.Name;
        }
    }
}
