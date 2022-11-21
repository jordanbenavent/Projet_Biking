using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServeurSoapBiking
{

    public class Adress
    {

        public string path { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }

        public List<Feature> features { get; set; }

        public string ToString()
        {
            return path + " " + city + " " + postalCode + features[0].properties.locality;
        }

        public string GetCity()
        {
            foreach(Feature feature in features){
                return feature.properties.locality;
            }
            return "";
        }
    }

    public class Feature
    {
        public Properties properties { get; set; }
    }

    public class Contract
    {
        public string name { get; set; }
        public string commercial_name { get; set; }
        public IList<string> cities { get; set; }

        public string country_code { get; set; }

    }

    public class Position
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Station
    {
        public int number { get; set; }
        public string contract_name { get; set; }

        public string name { get; set; }

        public Position position { get; set; }
    }


    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    
    
        
        public class ServiceBiking : IServiceBiking
        {

            public CompositeType GetDataUsingDataContract(CompositeType composite)
            {
                if (composite == null)
                {
                    throw new ArgumentNullException("composite");
                }
                if (composite.BoolValue)
                {
                    composite.StringValue += "Suffix";
                }
                return composite;
            }

            public string getRoute(string departure, string arrival)
            {
                Task<Adress> departAdress = getAdress(departure);
                //Task<string> arrivalAdress = getAdress(arrival);
                return departAdress.Result.GetCity();
            }

            static async Task<Adress> getAdress(string adress)
            {   
                try
                {
                    HttpResponseMessage responseContract = await Program.client.GetAsync("https://api.openrouteservice.org/geocode/search?" + "api_key=5b3ce3597851110001cf6248560a9124ee2b4b0591d9dcdaf3179440&text=" + adress);
                    Console.WriteLine(responseContract);
                    responseContract.EnsureSuccessStatusCode();
                    string responseBody = await responseContract.Content.ReadAsStringAsync();
                    Adress result = JsonSerializer.Deserialize<Adress>(responseBody);
                    return result;
            } catch (Exception e)
                {

                }
                return null;
            }
        }

}
