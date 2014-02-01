using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    /// <summary>
    /// This has a compound primary key consisting of Order and User. In EF, it is strange, the foreign keys have to specified so they can be used in the key.
    /// </summary>
    public class OrderArchive
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime ArchiveDate { get; set; }
    }
}
