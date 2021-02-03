using Microsoft.EntityFrameworkCore;
using Northwind.Common.Interface;
using Northwind.Data.Context;
using Northwind.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Data.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IList<Customer>> GetCustomerOrdersInformation();
    }

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(NorthwindContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Customer>> GetCustomerOrdersInformation()
        {
            return await DbContext.Set<Customer>()
                .Include(r => r.Orders)
                    .ThenInclude(r => r.OrderDetails)
                    .ThenInclude(r => r.Product)
                    .ToListAsync();
        }
    }
}
