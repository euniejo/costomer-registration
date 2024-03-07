using Customers.Entities;
using System.ComponentModel.DataAnnotations;

namespace Customers.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

       

        [Required(ErrorMessage = "Please add date for the invoice")]

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate
        {
            get
            {
                return InvoiceDate?.AddDays(Convert.ToDouble(PaymentTerm?.DueDays));
            }
        }

        public double? PaymentTotal { get; set; } = 0.0;

     
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Please add payment term for the invoice")]

        // FK:
        public int PaymentTermsId { get; set; }

        // FK:
        public int CustomerId { get; set; }


        // Nav props:
        public Customer? Customer { get; set; }

        public PaymentTerms? PaymentTerm { get; set; }


        // A nav prop to all the invoicelineitem for this invoice:
        public ICollection<InvoiceLineItem>? InvoiceLineItems { get; set; }
    }
}
