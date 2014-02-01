using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
    public partial class Order
    {
        partial void State_Compute(ref string result)
        {
            // Set result to the desired field value
            if (Status == 0)
                result = "Submitted";
            else if (Status == 1)
                result = "Quoted";
            else if (Status == 2)
                result = "Approved";
            else if (Status == 3)
                result = "Fulfilled";
            else
                result = "Unknown";
        }

        partial void GlobalPivotAmount_Compute(ref double result)
        {
            // Set result to the desired field value
            result = this.Amount/this.GlobalPivotExchangeRate;
        }

        partial void GlobalPivotAdminFee_Compute(ref double result)
        {
            // Set result to the desired field value
            result = this.AdminFee / this.GlobalPivotExchangeRate;
        }

        partial void ItemsCount_Compute(ref int result)
        {
            // Set result to the desired field value
            result = this.OrderLineItems.Count();
        }
    }
}
