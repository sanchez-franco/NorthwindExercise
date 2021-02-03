using Northwind.Common.Model;
using System.Threading.Tasks;

namespace Northwind.Common.Interface
{
    public interface ICustomerService
    {
        Task<CustomerSummary[]> GetCustomerSummary();
    }
}
