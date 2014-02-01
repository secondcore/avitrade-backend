using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
    public partial class OrderLineItem
    {
        partial void Summary_Compute(ref string result)
        {
            // Set result to the desired field value
            result = this.Item.Name + " - " + this.Units;
        }

        partial void GlobalPivotAmount_Compute(ref double result)
        {
            // Set result to the desired field value
            result = this.Amount / this.GlobalPivotExchangeRate;
        }

        partial void GlobalPivotAdminFee_Compute(ref double result)
        {
            // Set result to the desired field value
            result = this.AdminFee / this.GlobalPivotExchangeRate;
        }
    }
}
