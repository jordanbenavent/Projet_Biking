using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ProxySOAPExecutable

{
    // Contient un objet composé d'un nom de contrat (souvent une ville) et des stations associées à ce contrat
    class JCDecauxItem
    {
        public static readonly HttpClient client = new HttpClient();
        public string url = "https://api.jcdecaux.com/vls/v2/";
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
                string responseBody = await client.GetStringAsync(request);
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

        public string getContract()
        {
            return contractSelected;
        }
    }
}
