using Customers.Entities;

namespace Customers.Entities
{

    public class PaymentTerms
    {
        public int PaymentTermsId { get; set; }

        public string Description { get; set; } = null!;

        public int DueDays { get; set; }


        // FK:
        //public int? InvoiceId { get; set; }

        // A nav prop to all the invoices applied for this paymentTems:
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
