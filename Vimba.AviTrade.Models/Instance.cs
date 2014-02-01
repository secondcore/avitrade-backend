using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class Instance
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Industry { get; set; }

        //Additional Attributes to control features
        public double AdminFeePercentage { get; set; }

        // The pivot currency refers to this particular instance  
        public Currency PivotCurrency { get; set; }
        // The global pivot currency refers to how we want to view amounts globally
        public Currency GlobalPivotCurrency { get; set; }
    }
}
