using Customers.Entities;
using System.ComponentModel.DataAnnotations;

namespace Customers.Entities
{
    public class InvoiceLineItem
    {
        public int InvoiceLineItemId { get; set; }
        [Required(ErrorMessage = "Please add amount for the item")]

        public double? Amount { get; set; }

        [Required(ErrorMessage = "Please add description for the item")]
        public string? Description { get; set; }

        // FK:
        public int InvoiceId { get; set; }

        // A nav prop to all the invoice for this item:
        public Invoice? Invoice { get; set; }
    }
}
