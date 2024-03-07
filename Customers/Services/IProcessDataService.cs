using Customers.Entities;

namespace Customers.Services
{
    public interface IProcessDataService
    {
         public (string lower, string upper) GetBoundsForGroup(string group);
        public string FindGroup(string name);
    }
}
