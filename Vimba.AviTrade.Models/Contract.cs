using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class Contract
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Contract's Start and End Dates
        // Note that when contracts are renewed, they are treated like new records
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Contract's Approval Dates
        // A contract is only valid if both traders apporve the contract
        public DateTime TraderOneApprovalDate { get; set; }
        public DateTime TraderTwoApprovalDate { get; set; }
        public bool IsTraderOneApproved { get; set; } // TODO: Needed due to a bug in EF 4.x
        public bool IsTraderTwoApproved { get; set; } // TODO: Needed due to a bug in EF 4.x

        // Navigational Properties
        public Trader TraderOne { get; set; }
        public Trader TraderTwo { get; set; }
        public Instance Instance{ get; set; }
        // I opted to include the billing currency in the contract
        public Currency BillingCurrency { get; set; }
        // I opted to include the time zone in the contract. This means that all transaction dates will be adjusted
        // based on this.
        public Vimba.AviTrade.Models.TimeZone TimeZone { get; set; }
    }
}
