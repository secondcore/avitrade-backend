using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
	public class OrderLineItem
	{
		public int Id { get; set; }

		public DateTime FulfilmentDate { get; set; }

		public int Units { get; set; }

		public double PricePerUnit { get; set; }

		public double Amount { get; set; }

		public double AdminFee { get; set; }

		public double PivotExchangeRate { get; set; } // To the pivot currency - defined in the contract's instance

		public double GlobalPivotExchangeRate { get; set; } // To the global pivot currency - defined in the contract's instance

	    public string Instructions { get; set; }

        //Navigational Properties
		public Currency Currency { get; set; } // The line item currency

		public Item Item { get; set; }

		public Order Order { get; set; }
	}

	public class OrderLineItemDto
	{
		public int ItemId { get; set; }

		public string Category { get; set; }

		public string SubCategory { get; set; }

		public string Item { get; set; }

		public int Units { get; set; }

		public string CurrencyId { get; set; }

		public double PricePerUnit { get; set; }

		public string Unit { get; set; }

		public double Amount { get; set; }

	    public string Instructions { get; set; }
	}
}