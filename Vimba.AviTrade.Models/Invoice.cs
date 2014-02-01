using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    /// <summary>
    /// => Every order mandates an invoice to be issued from seller to buyer
    /// => The billing period is abstracted just in case the accounting systems require a non-gregorian calender based billing
    /// </summary>
    public class Invoice
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreateDate;

        // Navigational Properties
        public Period BillingPeriod { get; set; }
        public Order Order { get; set; }
    }
}
