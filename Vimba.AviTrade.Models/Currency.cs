using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class Currency
    {
        [Key, DatabaseGenerated((DatabaseGeneratedOption.None))]
        public string Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Rate { get; set; }
    }
}
