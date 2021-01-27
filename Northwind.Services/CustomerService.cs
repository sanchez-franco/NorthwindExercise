using Microsoft.Extensions.Options;
using Northwind.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Northwind.Services
{
    public interface ICustomerService
    {
        CustomerSummary[] GetCustomerSummary();
    }

    public class MemoryCustomerService : ICustomerService
    {
        public CustomerSummary[] GetCustomerSummary()
        {
            var retValue = new List<CustomerSummary>();
            var random = new Random();

            for(int i = 0; i < 10; i++)
            {
                retValue.Add(new CustomerSummary
                {
                    CustomerId = $"CustomerId {i}",
                    CustomerName = $"Customer Name {i}",
                    ProductName = $"Product {i}",
                    ProductTotal = random.Next(),
                    QuantityTotal = random.Next(),
                    MinOrderDate = new DateTime(2000, 1 + i, 1),
                    MaxOrderDate = new DateTime(2000 + i, 1 + i, 1)
                });

            }

            return retValue.ToArray();
        }
    }

    public class CustomerService : ICustomerService
    {
        private readonly AppSettings _appSettings;

        public CustomerService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public CustomerSummary[] GetCustomerSummary()
        {
            var retValue = new List<CustomerSummary>();
            using(var connectionString = new SqlConnection(_appSettings.ConnectionString))
            using (var sqlCommnad = new SqlCommand("dbo.SelectAllCustomerData"))
            using(var adapter = new SqlDataAdapter(sqlCommnad))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach(DataRow row in dt.Rows)
                {
                    retValue.Add(new CustomerSummary
                    {
                        CustomerId = row["CustomerId"].ToString(),
                        CustomerName = row["CustomerName"].ToString(),
                        ProductName = row["ProductName"].ToString(),
                        ProductTotal = int.Parse(row["ProductTotal"].ToString()),
                        QuantityTotal = int.Parse(row["QuantityTotal"].ToString()),
                        MinOrderDate = (DateTime)row["MinOrderDate"],
                        MaxOrderDate = (DateTime)row["MaxOrderDate"]
                    });
                }
            }

            return retValue.ToArray();
        }
    }
}
