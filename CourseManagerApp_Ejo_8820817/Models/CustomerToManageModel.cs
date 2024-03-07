using Customers.Entities;

namespace CustomerManagerApp_Ejo_8820817.Models
{
    public class CustomerToManageModel
    {
        // the customer selected or the new customer:
        public Customer? ActiveCustomer { get; set; }
        public Invoice? NewInvoice { get; set; }

    }
}
