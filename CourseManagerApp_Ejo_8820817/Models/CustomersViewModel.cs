using Customers.Entities;

namespace CustomerManagerApp_Ejo_8820817.Models
{
    public class CustomersViewModel
    {
        // the selected customers for management:
        public List<Customer>? Customers { get; set; }
        //public Student? NewStudent { get; set; } = null;
        public string Group { get; set; }


    }
}
