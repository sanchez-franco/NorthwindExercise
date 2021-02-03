using System;

namespace Northwind.Common.Model
{
    public class CustomerSummary : ICloneable
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public int ProductTotal { get; set; }
        public int OrderId { get; set; }
        public DateTime MinOrderDate { get; set; }
        public DateTime MaxOrderDate { get; set; }

        public object Clone()
        {
            return new CustomerSummary
            {
                CustomerId = this.CustomerId,
                CompanyName = this.CompanyName,
                ProductName = this.ProductName,
                ProductTotal = this.ProductTotal,
                OrderId = this.OrderId,
                MinOrderDate = this.MinOrderDate,
                MaxOrderDate = this.MaxOrderDate,
            };
        }
    }
}
