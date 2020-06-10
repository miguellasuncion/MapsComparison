using Maps.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Maps.Report;

namespace Maps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start - GeoPosition comparison");

            string bingKey = "bing-key";
            string azureKey = "azure-key";

            List<string> addressList = DataTest.GetAddress();
            IReport reportComparer = new ReportComparer();

            AzureProvider azureMaps = new AzureProvider(azureKey, reportComparer);
            BingProvider bingMaps = new BingProvider(bingKey, reportComparer);

            foreach (string address in addressList)
            {
                azureMaps.GeoPositionAsync(address).Wait();
                bingMaps.GeoPositionAsync(address).Wait();
            }

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
