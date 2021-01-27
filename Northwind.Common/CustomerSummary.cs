using System;

namespace Northwind.Common
{
    public class CustomerSummary
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int ProductTotal { get; set; }
        public int QuantityTotal { get; set; }
        public DateTime MinOrderDate { get; set; }
        public DateTime MaxOrderDate { get; set; }
    }
}
