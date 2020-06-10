using Maps;
using Maps.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maps.Classes
{
    internal class AzureProvider : IMapsProvider
    {
        string SubscriptionKey;
        string Url;
        IReport ReportComparer;

        public AzureProvider(string key, IReport reportComparer)
        {
            SubscriptionKey = key;
            ReportComparer = reportComparer;
        }

        public async Task GeoPositionAsync(string query)
        {
            Url = $"https://atlas.microsoft.com/search/address/json?api-version=1.0&countrySet=ES&subscription-key={SubscriptionKey}&typeahead=true&query=";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(Url + query);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(content);

                    dynamic numResultsJson = jsonResponse.summary.numResults;
                    int numResults = Convert.ToInt32(numResultsJson);

                    ReportComparer.SaveAndDisplay($"Azure,  {numResults}, {query}");
                }
                else
                {
                    //var error = await response.Content.ReadAsAsync<ErrorResponse>();
                    Console.WriteLine("Azure HTTP Error");
                }
            }
        }
 
    }
}
