using CustomerManagerApp_Ejo_8820817.Services;
using Customers.Entities;
using Customers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerTest
{
    public class ServiceUnitTest
    {
        private readonly IProcessDataService _pDataService;

        public ServiceUnitTest()
        {
            _pDataService = new ProcessDataService(); ;
        }

        [Fact]
        public void GetBoundsForGroup()
        {
            // Arrange (our data/objects for the test):
            string companyName = "A-E";

            var (lowerbound, upperbound) = _pDataService.GetBoundsForGroup(companyName);


            // Assert (i.e. asserting that the result is what we expected):
            Assert.Equal(new[] { "A", "E" }, new[] { lowerbound, upperbound });
        }

        [Fact]
        public void FintGroup()
        {
            // Arrange (our data/objects for the test):
            string companyName = "Conestoga";

            string group = _pDataService.FindGroup(companyName);


            // Assert (i.e. asserting that the result is what we expected):
            Assert.Equal(group, "A-E");
        }
    }
}
