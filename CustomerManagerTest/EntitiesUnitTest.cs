using Customers.Entities;

namespace CustomerManagerTest
{
    public class EntitiesUnitTest
    {
        [Fact]
        public void TotalListItmesAmount()
        {
            // Arrange (our data/objects for the test):
            Invoice invoice = new Invoice() 
            { InvoiceId = 1, 
              InvoiceDate = new DateTime(2022, 8, 5), 
              PaymentTermsId = 3, 
              CustomerId = 1,
              InvoiceLineItems = new List<InvoiceLineItem>(),
            };


            invoice.InvoiceLineItems.Add(new InvoiceLineItem() { Description = "Part1", Amount = 1000 });
            invoice.InvoiceLineItems.Add(new InvoiceLineItem() { Description = "Part2", Amount = 2000 });

            // Act (i.e. acting on the data we arranged):
            double? totalAmount = invoice.InvoiceLineItems.Sum(r => r.Amount);


            // Assert (i.e. asserting that the result is what we expected):
            Assert.Equal(totalAmount, 3000);
        }
   
    }
}