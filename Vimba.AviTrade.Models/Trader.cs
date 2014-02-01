using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class Trader
    {
        public Trader()
        {
            CreditCards = new List<CreditCard>();
        }

        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Fax { get; set; }

        // Invoice Generation Scheme
        public int CurrentInvoiceCounter { get; set; }

        // Navigational Properties
        public Country Country { get; set; }
        public List<CreditCard> CreditCards { get; set; }

        //[NotMapped]
        public string GenerateInvoiceNumber(DateTime approvedDate)
        {
            // 234-20120606-3345
            return Id + "-" + approvedDate.Year + "-" + approvedDate.Month + "-" + approvedDate.Day + "-" + CurrentInvoiceCounter++;
        }
    }
}
