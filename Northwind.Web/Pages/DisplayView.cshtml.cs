using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Common.Interface;
using Northwind.Common.Model;

namespace Northwind.Web.Pages
{
    public class DisplayViewModel : PageModel
    {
        private readonly ICustomerService _customerApi;

        public DisplayViewModel(ICustomerService customerApi)
        {
            _customerApi = customerApi;
        }

        public IList<CustomerSummary> CustomerSummarys { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            CustomerSummarys = (await _customerApi.GetCustomerSummary()).Where(c =>
                string.IsNullOrEmpty(SearchString)
                    ||
               (c.CompanyName.ToLower().Contains(SearchString?.ToLower()) || c.ProductName.ToLower().Contains(SearchString?.ToLower()))).ToList();
        }
    }
}
