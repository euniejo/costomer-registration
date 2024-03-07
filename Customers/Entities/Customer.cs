using Customers.Entities;
using System.ComponentModel.DataAnnotations;

namespace Customers.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }


        [Required(ErrorMessage = "Please add a name for the customer")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please add a address  for the customer")]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "Please add a city  for the customer")]
        public string? City { get; set; } = null!;


        [Required(ErrorMessage = "Please add a province  for the customer")]
        [RegularExpression(@"^[A-Z][A-Z]$",ErrorMessage = "State/Prov must be 2 letter code")]
 
        public string? ProvinceOrState { get; set; } = null!;

        [Required(ErrorMessage = "Please add Zip/Postal code for the customer")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip/Postal code must be in a valid US or Canadian format. Digit 5")]

        public string? ZipOrPostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Please add a phone  for the customer")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "The phone number must be in the format 123-123-1234.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please add a contact last name  for the customer")]
        public string? ContactLastName { get; set; }

        [Required(ErrorMessage = "Please add a contact frist name  for the customer")]
        public string? ContactFirstName { get; set; }

        //[DataType(DataType.EmailAddress)]
       // [DataType(DataType.EmailAddress, ErrorMessage = "Email must be in a valid format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be in a valid format")]
        public string? ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        // A nav prop to all the invoices for this customer:
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
