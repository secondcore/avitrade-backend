using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class InvoicesRepository : GenericRepository<Invoice>, IInvoicesRepository
    {
        private IPeriodsRepository _periodsRepository;
        protected override IQueryable<Invoice> DefaultSet
        {
            get
            {
                return _Context.Invoices.Include("BillingPeriod").Include("Order.Buyer").Include("Order.Seller");
            }
        }

        protected override Func<Invoice, object> DefaultOrderBy
        {
            get
            {
                return x => x.CreateDate;
            }
        }

        public InvoicesRepository(AviTradeContext context, 
                                  IPeriodsRepository perRep) 
            : base(context)
        {
            _periodsRepository = perRep;
        }

        public override Invoice FindById(int id)
        {
            var query = (from i in DefaultSet
                         where i.Id == id
                         select i).SingleOrDefault();

            return query;
        }

        public Invoice FindByOrderId(int orderId)
        {
            var query = (from i in DefaultSet
                         where i.Order.Id == orderId
                         select i).SingleOrDefault();

            return query;
        }

        public Invoice Create(Order order, string invoiceNumber)
        {
            Period billingPeriod = _periodsRepository.FindByDate(order.ApprovalDate);

            Invoice invoice = new Invoice();
            invoice.CreateDate = order.ApprovalDate;
            invoice.InvoiceNumber = invoiceNumber;
            invoice.Order = order;
            invoice.BillingPeriod = billingPeriod;
            _Context.Invoices.Add(invoice);
            _Context.SaveChanges();
            return invoice;
        }

        /* P R I V A T E  M E T H O D S */
    }
}
