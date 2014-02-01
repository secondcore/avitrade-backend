using System;
using System.Collections.Generic;

namespace Vimba.AviTrade.Models.ServiceModels
{
    public class OrderServiceModel
    {
        //[DataServiceKey]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime FulfillmentDate { get; set; }
        public string OrderStatus { get; set; }
        public double Amount { get; set; }
        public string Buyer { get; set; }
        public string Seller { get; set; }
        public string TakeoffAirport { get; set; }
        public string LandingAirport { get; set; }
        public string AirplaneManufacturer { get; set; }
        public string AirplaneModel { get; set; }
        public string FlightNumber { get; set; }
        public string Contract { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }

        public List<OrderLineItemServiceModel> LineItems { get; set; }
    }

    public class OrderLineItemServiceModel
    {
        //[DataServiceKey]
        public int Id { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Item { get; set; }
        public int Units { get; set; }
        public double PricePerUnit { get; set; }
    }
}