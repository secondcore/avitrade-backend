using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    /// <summary>
    /// It is assumed that:
    /// => Every order mandates an invoice to be issued from seller to buyer
    /// => Traders can assumes different roles (i.e. buyers or sellers) at the time of order
    /// => The instance associates this order with a bigger entity that defines, among other things, the pivot currency 
    /// => The buyer and seller references are included in addition to the contract to determine the buyer and the seller for this patrticular order
    /// => The payment terms and method are locked in this order transaction
    /// => The exchange rate (against the pivot and global pivot currencies) and the total amount ate also locked
    /// => The order is per a single flight 
    /// => The airport, aircraft and flight information are locked. This will allow for great insights!!!
    /// => All dates are adjusted per the contract's time zone
    /// </summary>
    public class Order
    {
        // Possible order statuses
        public const int Submitted = 0;
        public const int Quoted = 1;
        public const int Approved = 2;
        public const int Fulfilled = 3;

        public Order()
        {
            LineItems = new List<OrderLineItem>();
            Status = Submitted;
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime QuotationDate { get; set; }
        public bool IsQuoted { get; set; } // TODO: Needed due to a bug in EF 4.x
        public DateTime ApprovalDate { get; set; }
        public bool IsApproved { get; set; } // TODO: Needed due to a bug in EF 4.x
        public DateTime FulfilmentDate { get; set; }
        public bool IsFulfilled { get; set; } // TODO: Needed due to a bug in EF 4.x

        public int Status { get; set; }

        // Locked per order transaction
        public double Amount { get; set; } // Amount in the billing currency as defined in the contract
        public double PivotExchangeRate { get; set; } // To the Pivot Currency as defined in the contract's instance
        public double GlobalPivotExchangeRate { get; set; } // To Global Pivot Currency as defined in the contract's instance

        //TODO: Both the amount and admin fee are charged to the buyer
        public double AdminFee { get; set; } // Same as Amount, the Admin Fee in the billing currency as defined in the contract
        
        public string BuyerCardHolderName { get; set; }
        public string BuyerCardNumber { get; set; }
        public string BuyerCardExpDate { get; set; }
        public string BuyerCardSecurityCode { get; set; }
        public string BuyerPaypalUserId { get; set; }
        public string BuyerPaypalPassword { get; set; }
        public string BuyerReferenceNumber { get; set; }

        public string SellerCardHolderName { get; set; }
        public string SellerCardNumber { get; set; }
        public string SellerCardExpDate { get; set; }
        public string SellerCardSecurityCode { get; set; }
        public string SellerPaypalUserId { get; set; }
        public string SellerPaypalPassword { get; set; }
        public string SellerReferenceNumber { get; set; }

        // Optional Navigational Properties
        public Airport TakeoffAirport { get; set; }
        public Airport LandingAirport { get; set; }
        public Aircraft Aircraft { get; set; }
        public string Operator { get; set; }
        public string FlightNumber { get; set; }
        public DateTime EstimatedTakeoffTime { get; set; }
        public DateTime EstimatedLandingTime { get; set; }

        // Navigational Properties
        public Contract Contract { get; set; }
        public Trader Buyer { get; set; }
        public Trader Seller { get; set; }
        //public Invoice Invoice { get; set; }
        public List<OrderLineItem> LineItems { get; set; }
    }
}
