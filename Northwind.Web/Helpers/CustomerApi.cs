using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Northwind.Common;
using System.Net.Http;
using System.Threading.Tasks;

namespace Northwind.Web.Helpers
{
    public interface ICustomerApi
    {
        Task<CustomerSummary[]> Get();
    }

    public class CustomerApi : ICustomerApi
    {
        private readonly AppSettings _appSettings;

        public CustomerApi(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<CustomerSummary[]> Get()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync($"{_appSettings.ServiceEndPoint}/Customers");

                return JsonConvert.DeserializeObject<CustomerSummary[]>(content);
            }
        }
    }
}
