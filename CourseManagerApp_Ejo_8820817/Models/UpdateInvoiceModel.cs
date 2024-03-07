using Customers.Entities;
using System;
using System.Collections.Generic;

namespace CustomerManagerApp_Ejo_8820817.Models
{
    public class UpdateInvoiceModel
    {
        // the selected customers for management:
        public Customer? Customer { get; set; }

        public string? Group { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public Invoice? SelectedInvoice { get; set; }
        public int? SelectedInvoiceNumber { get; set; }

        public Invoice? NewInvoice { get; set; }

        public InvoiceLineItem? NewLineItem { get; set; }

        public List<PaymentTerms>? PaymentTermsList { get; set; }

    }
}
