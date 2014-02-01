using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class TimeZone
    {
        [Key, DatabaseGenerated((DatabaseGeneratedOption.None))]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
    }
}
