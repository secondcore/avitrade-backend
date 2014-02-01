using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
    public partial class Item
    {
        partial void Summary_Compute(ref string result)
        {
            // Set result to the desired field value
            result = this.Category + " -> " + this.SubCategory + " -> " + this.Name;
        }
    }
}
