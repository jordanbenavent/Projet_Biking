using ProxySOAPExecutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProxySOAPExecutable
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Proxy : IProxy
    {
        public static readonly HttpClient client = new HttpClient();
        GenericProxyCache<JCDecauxItem> cache = new GenericProxyCache<JCDecauxItem>();

        public string getResponse(string url)
        {
            Task<string> response = communicate(url);
            return response.Result;
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

        // On n'utilise pas le cache pour une station précise car on la considère comme étant un élément dynamique, en effet la station peut être disponible en fonction du temps
        public string getStation(int number, string chosenContract)
        {
            string responseStationProcheBody = getResponse("https://api.jcdecaux.com/vls/v2/stations/" + number + "?contract=" + chosenContract + "&apiKey=98382454fc46549c5cdf105c9dcf4578e6cbea91");
            return responseStationProcheBody;
        }

        // On n'utilise pas non plus le cache car notre objet JCDecaux est composé d'un contrat et de ses stations, cela n'avait pas trop de sens d'ajouter un attribut qui recense tous les contrats
        public string getContracts()
        {
            string responseAllContracts = getResponse("https://api.jcdecaux.com/vls/v3/contracts?apiKey=98382454fc46549c5cdf105c9dcf4578e6cbea91");
            return responseAllContracts;
        }

        // On utilise le cache pour récupérer toutes les stations d'un contrat dans le cas où l'on souhaite trouver la station la plus proche en effet la position d'une station ne change pas
        public string getAllStationsOfAContract(string chosenContract)
        {
            string stations = cache.Get(chosenContract).getStations();
            return stations;
        }
    }
}
