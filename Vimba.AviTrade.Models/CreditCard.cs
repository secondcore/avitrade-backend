using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string HolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpDate { get; set; } // MONTH/YEAR
        public string SecurityCode { get; set; }

        // Navigational Properties
        public Trader Trader { get; set; }
    }
}
