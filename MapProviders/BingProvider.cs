using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Maps.Report;
using Maps.BingMapsRESTService.Common.JSON;

namespace Maps
{
    internal class BingProvider : IMapsProvider
    {
        string BingMapsAPIKey;
        IReport ReportComparer;

        public BingProvider(string key, IReport reportComparer)
        {
            BingMapsAPIKey = key;
            ReportComparer = reportComparer; 
        }

        public async Task GeoPositionAsync(string query)
        {

            Uri geocodeRequest = new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&key={1}", query, BingMapsAPIKey));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(geocodeRequest);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Response responseBing = JsonConvert.DeserializeObject<Response>(content);
                    dynamic numResults = responseBing.ResourceSets[0].Resources.Length;

                    ReportComparer.SaveAndDisplay($"Bing, {numResults}, {query}");
                }
                else
                {
                    //var error = await response.Content.ReadAsAsync<ErrorResponse>();
                    Console.WriteLine("Bing HTTP Error");
                }
            }
        }

        private void GetResponse(Uri uri, Action<Response> callback)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (o, a) =>
            {
                if (callback != null)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                    callback(ser.ReadObject(a.Result) as Response);
                }
            };
            wc.OpenReadAsync(uri);
        }

        private void GetPOSTResponse(Uri uri, string data, Action<Response> callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            request.Method = "POST";
            request.ContentType = "text/plain;charset=utf-8";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(data);

            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                // Send the data.  
                requestStream.Write(bytes, 0, bytes.Length);
            }

            request.BeginGetResponse((x) =>
            {
                using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x))
                {
                    if (callback != null)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                        callback(ser.ReadObject(response.GetResponseStream()) as Response);
                    }
                }
            }, null);
        }
    }
}
