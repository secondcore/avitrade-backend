using System;
using System.Collections.Generic;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Web.ViewModels
{
    public class ContractViewModel
    {
		public int Id { get; set; }
		public DateTime CreateDate { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		// Contract's Start and End Dates
		// Note that when contracts are renewed, they are treated like new records
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		// Contract's Approval Dates
		// A contract is only valid if both traders apporve the contract
		public DateTime MyApprovalDate { get; set; }
		public DateTime TraderApprovalDate { get; set; }
		public bool IsTraderApproved { get; set; } // TODO: Needed due to a bug in EF 4.x
		public bool IsApproved { get; set; } // TODO: Needed due to a bug in EF 4.x

		// Navigational Properties
		public TraderDto Trader { get; set; }
		public Instance Instance { get; set; }

		// I opted to include the billing currency in the contract
		public Currency BillingCurrency { get; set; }

		// I opted to include the time zone in the contract. This means that all transaction dates will be adjusted
		// based on this.
		public Vimba.AviTrade.Models.TimeZone TimeZone { get; set; }

        public ContractViewModel(int filterByTraderId, Contract contract)
		{
			this.Id = contract.Id;
			this.CreateDate = contract.CreateDate;
			this.Name = contract.Name;
			this.Description = contract.Description;

			this.StartDate = contract.StartDate;
			this.EndDate = contract.EndDate;

			this.IsApproved = contract.TraderOne.Id == filterByTraderId ? contract.IsTraderOneApproved : contract.IsTraderTwoApproved;
			this.MyApprovalDate = contract.TraderOne.Id == filterByTraderId ? contract.TraderOneApprovalDate : contract.TraderTwoApprovalDate;

			this.IsTraderApproved = contract.TraderOne.Id == filterByTraderId ? contract.IsTraderTwoApproved : contract.IsTraderOneApproved;
			this.TraderApprovalDate = contract.TraderOne.Id == filterByTraderId ? contract.TraderTwoApprovalDate : contract.TraderOneApprovalDate;

			this.Trader = new TraderDto(contract.TraderOne.Id == filterByTraderId ? contract.TraderTwo : contract.TraderOne);
			this.Instance = contract.Instance;

			this.BillingCurrency = contract.BillingCurrency;
			this.TimeZone = contract.TimeZone;
		}
	}

	public class TraderDto
	{
		public int Id { get; set; }
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
		public List<CreditCardDto> CreditCards { get; set; }

		public TraderDto(Trader trader)
		{
			this.Id = trader.Id;
			this.Account = trader.Account;
			this.Name = trader.Name;
			this.Address = trader.Address;
			this.Address2 = trader.Address2;
			this.City = trader.City;
			this.Phone = trader.Phone;
			this.Email = trader.Email;
			this.Contact = trader.Contact;
			this.Fax = trader.Fax;

			this.CurrentInvoiceCounter = trader.CurrentInvoiceCounter;
			this.Country = trader.Country;

			this.CreditCards = new List<CreditCardDto>();
			foreach (CreditCard creditCard in trader.CreditCards)
			{
				this.CreditCards.Add(new CreditCardDto(creditCard));
			}
		}
	}

	public class CreditCardDto
	{
		public int Id { get; set; }
		public string HolderName { get; set; }
		public string CardNumber { get; set; }
		public string ExpDate { get; set; } // MONTH/YEAR
		public string SecurityCode { get; set; }

		public CreditCardDto(CreditCard creditCard)
		{
			this.Id = creditCard.Id;
			this.HolderName = creditCard.HolderName;
			this.CardNumber = creditCard.CardNumber;
			this.ExpDate = creditCard.ExpDate;
			this.SecurityCode = creditCard.SecurityCode;
		}
	}
}