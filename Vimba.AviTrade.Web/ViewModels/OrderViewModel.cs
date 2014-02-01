using System;
using System.Collections.Generic;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Web.ViewModels
{
	public class OrderViewModel
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime QuotationDate { get; set; }
		public DateTime ApprovalDate { get; set; }
		public DateTime FulfilmentDate { get; set; }
		public string Status { get; set; }

		// Locked per order transaction
		public double Amount { get; set; } // Amount in the global pivot currency
		public double AdminFee { get; set; } // Admin Fee the global pivot currency

		// Optional Navigational Properties
		public int TakeoffAirportId { get; set; }
		public int LandingAirportId { get; set; }
		public int AircraftId { get; set; }
		public string TakeoffAirport { get; set; }
		public string LandingAirport { get; set; }
		public string Aircraft { get; set; }
		public string Operator { get; set; }
		public string FlightNumber { get; set; }
		public DateTime EstimatedTakeoffTime { get; set; }
		public DateTime EstimatedLandingTime { get; set; }

		// Navigational Properties
		public ContractViewModel Contract { get; set; }
		public int BuyerId { get; set; }
		public string Buyer { get; set; }
		public int SellerId { get; set; }
		public string Seller { get; set; }
		public bool IsSeller { get; set; }
		public bool IsViewed { get; set; } //TODO: This cannot be retrieved without a user ...also it should dependency on the order state
		public bool IsQuoted { get; set; }
		public bool IsApproved { get; set; }
		public bool IsFulfilled { get; set; }
		public List<OrderLineItemDto> LineItems { get; set; }

		public OrderViewModel(int traderId, Order order)
		{
			Id = order.Id;
			OrderDate = order.OrderDate;
			QuotationDate = order.QuotationDate;
			ApprovalDate = order.ApprovalDate;
			FulfilmentDate = order.FulfilmentDate;

			Status = "Submitted";
			if (order.Status == Order.Submitted)
				Status = "Submitted";
			else if (order.Status == Order.Quoted)
				Status = "Quoted";
			else if (order.Status == Order.Approved)
				Status = "Approved";
			else if (order.Status == Order.Fulfilled)
				Status = "Fulfilled";

			Amount = order.Amount / order.GlobalPivotExchangeRate;
			AdminFee = order.AdminFee / order.GlobalPivotExchangeRate;

			TakeoffAirport = order.TakeoffAirport.Name;
			LandingAirport = order.LandingAirport.Name;
			Aircraft = order.Aircraft.Manufacturer;
			Operator = order.Operator;
			FlightNumber = order.FlightNumber;
			EstimatedTakeoffTime = order.EstimatedTakeoffTime;
			EstimatedLandingTime = order.EstimatedLandingTime;

			AircraftId = order.Aircraft.Id;
			TakeoffAirportId = order.TakeoffAirport.Id;
			LandingAirportId = order.LandingAirport.Id;

			BuyerId = order.Buyer.Id;
			Buyer = order.Buyer.Name;
			SellerId = order.Seller.Id;
			Seller = order.Seller.Name;

			IsSeller = order.Seller.Id == traderId;
			IsViewed = false; // Please see comments above

			IsQuoted = order.IsQuoted;
			IsApproved = order.IsApproved;
			IsFulfilled = order.IsFulfilled;

			Contract = new ContractViewModel(traderId, order.Contract);

			LineItems = new List<OrderLineItemDto>();
			foreach (OrderLineItem lineItem in order.LineItems)
			{
				var dto = new OrderLineItemDto();
				dto.ItemId = lineItem.Item.Id;
				dto.Category = lineItem.Item.Category;
				dto.SubCategory = lineItem.Item.SubCategory;
				dto.Item = lineItem.Item.Name;
				dto.Units = lineItem.Units;
				dto.Unit = lineItem.Item.Unit;
				dto.CurrencyId = lineItem.Currency.Id;
				dto.PricePerUnit = lineItem.PricePerUnit / lineItem.GlobalPivotExchangeRate;
				dto.Amount = dto.PricePerUnit * dto.Units;
				dto.Instructions = lineItem.Instructions;
				LineItems.Add(dto);
			}
		}
	}
}