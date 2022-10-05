using NParks_Maven_II_AVS_WebApi.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NParks_Maven_II_AVS_ClassLibrary.Functions
{
    public class PostalCodeConfig
    {
        private readonly HttpClient _client = HTTPClient.getClient();

        public string getPostalCodeConfigs(string apiParams)
        {
            HttpResponseMessage res = _client.GetAsync(apiParams).Result;

            if (res.IsSuccessStatusCode)
            {
                var data = res.Content.ReadAsStream().ToString();
                return data!;
            }
            else
            {
                return null!;
            }
        }
    }
}
