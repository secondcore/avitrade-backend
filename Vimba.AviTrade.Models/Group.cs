using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class Group
    {
        public const string AviTrade = "AviTrade";
        public const string Mebaa = "Mebaa";
        public const string Traders = "Traders";

        [Key, DatabaseGenerated((DatabaseGeneratedOption.None))]
        public string Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
