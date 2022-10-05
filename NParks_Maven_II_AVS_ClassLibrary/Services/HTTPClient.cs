using System.Net.Http.Headers;

namespace NParks_Maven_II_AVS_WebApi.Services
{
    public class HTTPClient
    {
        //Not working
        private static readonly string? RESOURCE = System.Environment.GetEnvironmentVariable("API_RESOURCE");
        private static readonly string? BASE = System.Environment.GetEnvironmentVariable("API_BASE");

        public static HttpClient getClient()
        {
            return prepareClient();
        }

        private static HttpClient prepareClient()
        {
            //New Client
            HttpClient client = new HttpClient
            {
                //BaseAddress = new Uri($"{RESOURCE}${BASE}"),
                BaseAddress = new Uri("https://org7f3624ff.api.crm5.dynamics.com/api/data/v9.2/"),
                Timeout = new TimeSpan(0, 2, 0)
            };

            //Default Headers
            HttpRequestHeaders headers = client.DefaultRequestHeaders;
            headers.Add("OData-MaxVersion", "4.0");
            headers.Add("OData-Version", "4.0");
            headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
