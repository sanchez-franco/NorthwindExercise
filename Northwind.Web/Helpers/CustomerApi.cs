using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Northwind.Common.Configuration;
using Northwind.Common.Interface;
using Northwind.Common.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Northwind.Web.Helpers
{
    public class CustomerApi : ICustomerService
    {
        private readonly AppSettings _appSettings;

        public CustomerApi(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<CustomerSummary[]> GetCustomerSummary()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync($"{_appSettings.ServiceEndPoint}/Customers");

                return JsonConvert.DeserializeObject<CustomerSummary[]>(content);
            }
        }
    }
}
