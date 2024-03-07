using System.Net.Mail;
using System.Net;
using Customers.Entities;
using Customers.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagerApp_Ejo_8820817.Services
{
    /// <summary>
    /// the class to get the Group Info and calculate the total of items on a invoice. 
    /// </summary>
    public class ProcessDataService : IProcessDataService
    {
        public string[] Groups = { "A-E", "F-K", "L-R", "S-Z" };

        public string FindGroup(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "Unknown";
            }

            char firstLetter = name.ToUpper()[0];


            foreach (string group in Groups)
            {
                char lowerBound = group.Split('-')[0].ToUpper()[0];
                char upperBound = group.Split('-')[1].ToUpper()[0];

                if (firstLetter >= lowerBound && firstLetter <= upperBound)
                {
                    return group;
                }
            }

            return "Unknown";

        }

        public (string lower, string upper) GetBoundsForGroup(string group)
        {
            char lowerBound = group.Split('-')[0].ToUpper()[0];
            char upperBound = group.Split('-')[1].ToUpper()[0];

            return (lowerBound.ToString(), upperBound.ToString());

        }


        public double? GetTotalAmount(ICollection<InvoiceLineItem>? InvoiceLineItems)
        {
            double? total = 0;

            foreach (InvoiceLineItem item in InvoiceLineItems)
            {
                if (item != null)
                {
                    total += item.Amount;
                }

            }

            return total;
        }


    }
}
