using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
    public partial class Contract
    {
        partial void Summary_Compute(ref string result)
        {
            // Set result to the desired field value
            result = this.TraderOne.Name + " - " + this.TraderTwo.Name;
        }

        partial void Status_Compute(ref string result)
        {
            // Set result to the desired field value
            result = "Pending";
            if (this.IsTraderTwoApproved && this.IsTraderTwoApproved)
                result = "Approved";
        }
    }
}
