using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Common;
using Northwind.Web.Helpers;

namespace Northwind.Web.Pages
{
    public class DisplayViewModel : PageModel
    {
        private readonly ICustomerApi _customerApi;

        public DisplayViewModel(ICustomerApi customerApi)
        {
            _customerApi = customerApi;
        }

        public IList<CustomerSummary> CustomerSummarys { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            CustomerSummarys = (await _customerApi.Get()).Where(c =>
                string.IsNullOrEmpty(SearchString)
                    ||
               (c.CustomerName.ToLower().Contains(SearchString?.ToLower()) || c.ProductName.ToLower().Contains(SearchString?.ToLower()))).ToList();
        }
    }
}
