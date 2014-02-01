using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class UserConfigurationItem
    {
        // Some configuration item keys
        public const string TraderCode = "TraderCode";
        public const string AviTradeCode = "AviTradeCode";
        public const string MebaaCode = "MebaaCode";

        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
    }
}
