using System;
using System.Collections.Generic;

namespace Vimba.AviTrade.Models.Settings
{
    public class OrderSettings
    {
        public OrderSettings()
        {
            ContractId = -1;
            BuyerIndexInContract = -1;
            BuyerCreditCard = null;
            SellerCreditCard = null;
            TakeOffAirportId = -1;
            LandingAirportId = -1;
            AircraftId = -1;
            Operateur = "";
            FlightNumber = "";
            LineItems = new List<OrderLineItemDto>();
        }

        public OrderSettings(DateTime orderDate, int contractId, int buyerIndexInContract, CreditCard buyerCreditCard, CreditCard sellerCreditCard, int takeOffAirportId, int landingAirportId, int aircraftId, string operateur, string flightNumber, DateTime estimateTakeOff, DateTime estimateLanding, List<OrderLineItemDto> lineItems)
        {
            OrderDate = orderDate;
            ContractId = contractId;
            BuyerIndexInContract = buyerIndexInContract;
            BuyerCreditCard = buyerCreditCard;
            SellerCreditCard = sellerCreditCard;
            TakeOffAirportId = takeOffAirportId;
            LandingAirportId = landingAirportId;
            AircraftId = aircraftId;
            Operateur = operateur;
            FlightNumber = flightNumber;
            EstimateTakeOff = estimateTakeOff;
            EstimateLanding = estimateLanding;
            LineItems = lineItems;
        }

        public DateTime OrderDate { get; set; }

        public int ContractId { get; set; }

        public int BuyerIndexInContract { get; set; }

        public CreditCard BuyerCreditCard { get; set; }

        public CreditCard SellerCreditCard { get; set; }

        public int TakeOffAirportId { get; set; }

        public int LandingAirportId { get; set; }

        public int AircraftId { get; set; }

        public string Operateur { get; set; }

        public string FlightNumber { get; set; }

        public DateTime EstimateTakeOff { get; set; }

        public DateTime EstimateLanding { get; set; }

        public List<OrderLineItemDto> LineItems { get; set; }
    }
}