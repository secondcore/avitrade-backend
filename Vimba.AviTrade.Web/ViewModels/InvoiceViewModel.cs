using System;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Web.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreateDate;
        public int BuyerId { get; set; }
        public string Buyer { get; set; }
        public int SellerId { get; set; }
        public string Seller { get; set; }
        public double Amount { get; set; }
        public double AdminFee { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }

        public InvoiceViewModel(Invoice invoice)
        {
            Id = invoice.Id;
            InvoiceNumber = invoice.InvoiceNumber;
            CreateDate = invoice.CreateDate;
            BuyerId = invoice.Order.Buyer.Id;
            Buyer = invoice.Order.Buyer.Name;
            SellerId = invoice.Order.Seller.Id;
            Seller = invoice.Order.Seller.Name;
            Amount = invoice.Order.Amount / invoice.Order.PivotExchangeRate;
            AdminFee = invoice.Order.AdminFee / invoice.Order.PivotExchangeRate;
            Tax = 0; // TODO: taxes are not available in in invoices
            Total = Amount + AdminFee;
        }
    }
}