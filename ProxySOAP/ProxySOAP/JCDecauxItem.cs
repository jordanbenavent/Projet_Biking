using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ProxySOAP
{
    // Contient un objet composé d'un nom de contrat (souvent une ville) et des stations associées à ce contrat
    class JCDecauxItem
    {
        public string url = "https://api.jcdecaux.com/vls/v3/";
        public string apiKey = "98382454fc46549c5cdf105c9dcf4578e6cbea91";
        public static string allStationsOfAContract;
        public string contractSelected;

        public JCDecauxItem(string contractSelected)
        {
            this.contractSelected = contractSelected;
            allStationsOfAContract = communicate(url + "stations?contract=" + contractSelected + "&apiKey=" + apiKey).Result;
            
        }

        public static async Task<string> communicate(string request)
        {
            try
            {
                string responseBody = await Program.client.GetStringAsync(request);
                return responseBody;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public string getStations()
        {
            return allStationsOfAContract;
        }
    }
}
