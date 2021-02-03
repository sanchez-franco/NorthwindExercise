using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Northwind.Common;
using Northwind.Common.Interface;
using Northwind.Common.Model;
using Northwind.Data.Entities;
using Northwind.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Services
{
    public class MemoryCustomerService : ICustomerService
    {
        public async Task<CustomerSummary[]> GetCustomerSummary()
        {
            Task<CustomerSummary[]> task = Task<CustomerSummary[]>.Factory.StartNew(() =>
            {
                var retValue = new List<CustomerSummary>();
                var random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    retValue.Add(new CustomerSummary
                    {
                        CustomerId = $"CustomerId {i}",
                        CompanyName = $"Customer Name {i}",
                        ProductName = $"Product {i}",
                        ProductTotal = random.Next(),
                        OrderId = random.Next(),
                        MinOrderDate = new DateTime(2000, 1 + i, 1),
                        MaxOrderDate = new DateTime(2000 + i, 1 + i, 1)
                    });

                }

                return retValue.ToArray();
            });

            return await task;
        }
    }

    public class EFCoreCustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public EFCoreCustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerSummary[]> GetCustomerSummary()
        {
            var result = await _customerRepository.GetCustomerOrdersInformation();

            IList<CustomerSummary> retValue = new List<CustomerSummary>(result.Count);
            foreach (var customer in result)
            {
                var customerSummary = new CustomerSummary
                {
                    CustomerId = customer.CustomerId,
                    CompanyName = customer.CompanyName,
                };

                foreach (var o in customer.Orders)
                {
                    customerSummary.OrderId = o.OrderId;
                    customerSummary.MaxOrderDate = (DateTime)customer.Orders.Max(x => x.OrderDate);
                    customerSummary.MinOrderDate = (DateTime)customer.Orders.Min(x => x.OrderDate);
                    foreach(var od in o.OrderDetails)
                    {
                        customerSummary.ProductTotal = od.Quantity;
                        customerSummary.ProductName = od.Product.ProductName;
                        retValue.Add((CustomerSummary)customerSummary.Clone());
                    }
                }
            }

            return retValue.ToArray();
        }
    }

    public class ADONetCustomerService : ICustomerService
    {
        private readonly IConfiguration _configuration;

        public ADONetCustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CustomerSummary[]> GetCustomerSummary()
        {
            Task<CustomerSummary[]> task = Task<CustomerSummary[]>.Factory.StartNew(() =>
            {
                var retValue = new List<CustomerSummary>();
                using (var connectionString = new SqlConnection(_configuration.GetConnectionString("NorthwindDb")))
                using (var sqlCommnad = connectionString.CreateCommand())
                {
                    sqlCommnad.CommandText = @"SELECT
	                                        c.[CustomerID]
	                                        ,[CompanyName]
	                                        ,p.ProductName
	                                        ,o.OrderID
	                                        ,od.Quantity AS ProductTotal
	                                        ,MIN(o.OrderDate) AS MinOrderDate
	                                        ,MAX(o.OrderDate) AS MaxOrderDate
                                        FROM [dbo].[Customers] c
	                                        INNER JOIN dbo.Orders o ON c.CustomerID = o.CustomerID
	                                        INNER JOIN dbo.[Order Details] od ON od.OrderID = o.OrderID
	                                        INNER JOIN dbo.Products p ON p.ProductID = od.ProductID
                                        GROUP BY
	                                        c.[CustomerID]
	                                        ,[CompanyName]
	                                        ,o.OrderID
	                                        ,p.ProductName
	                                        ,od.Quantity";
                    using (var adapter = new SqlDataAdapter(sqlCommnad))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            retValue.Add(new CustomerSummary
                            {
                                CustomerId = row["CustomerId"].ToString(),
                                CompanyName = row["CompanyName"].ToString(),
                                ProductName = row["ProductName"].ToString(),
                                ProductTotal = int.Parse(row["ProductTotal"].ToString()),
                                OrderId = int.Parse(row["OrderID"].ToString()),
                                MinOrderDate = (DateTime)row["MinOrderDate"],
                                MaxOrderDate = (DateTime)row["MaxOrderDate"]
                            });
                        }

                        connectionString.Close();
                    }
                }

                return retValue.ToArray();
            });

            return await task;
        }
    }
}
