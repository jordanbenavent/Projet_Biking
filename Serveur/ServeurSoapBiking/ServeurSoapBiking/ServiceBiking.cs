using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ServeurSoapBiking
{

    public class Adress
    {

        public string path { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }

        public List<Feature> features { get; set; }

        public bool Is_Empty()
        {
            return features.Count == 0;
        }

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

        public Position GetPosition()
        {
            foreach (Feature feature in features)
            {
                return feature.geometry;
            }
            return null;
        }
    }

    public class Feature
    {
        public Properties properties { get; set; }

        public Position geometry { get; set; }
    }

    public class Contract
    {
        public string name { get; set; }
        public string commercial_name { get; set; }
        public IList<string> cities { get; set; }

        public string country_code { get; set; }

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
            
            public static List<MQ> ListOfQueues = new List<MQ>();
            public string nomQueueStandard = "QueueServiceBiking";
            public static int nbQueue = 0;
            private string apiKeyORS = "api_key=5b3ce3597851110001cf6248560a9124ee2b4b0591d9dcdaf3179440";

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
            Task<Adress> arrivalAdress = getAdress(arrival);
            if(departAdress.Result.Is_Empty()) { return "Une erreur est survenue sur l'adresse de depart."; }
            if(arrivalAdress.Result.Is_Empty()) { return "Une erreur est survenue sur l'adresse d'arrivée."; }
            
            Task<Routing> routingwalking = getRouting(departAdress.Result.features[0].geometry, arrivalAdress.Result.features[0].geometry, "foot-walking?");
            //possible autre routing
            if(routingwalking.Result == null) { return "Une erreur est survenue dans la création de l'itinéraire"; }
            double duration = routingwalking.Result.features[0].properties.segments[0].duration;
            string directions = getDirections(routingwalking.Result);

            return directions;
            }

            public bool NextStep(string queue)
            {
            MQ userQueue = getQueue(queue);
            if(userQueue == null) { return true; }
            
            if(userQueue.steps.Count == userQueue.lastPush)
            {
                return false;
            }
            userQueue.PushOnQueue();
            return true;
            
            }

            private MQ getQueue(string userqueue)
            {
                foreach(MQ queue in ListOfQueues)
                {
                if (queue.nomQueue.Equals(userqueue))
                {
                    return queue;
                }
                }
            return null;
            }

        public static async Task<Adress> getAdress(string adress)
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
                return null;
                }
            }

            async Task<Routing> getRouting(Position start, Position end, string typeTransport)
            {
            string responseBody;
            string request = "https://api.openrouteservice.org/v2/directions/"+typeTransport + apiKeyORS  + "&start=" + start.getLongitudeString() +',' + start.getLattitudeString()+ "&end=" + end.getLongitudeString() +","+ end.getLattitudeString();
            try
            {
                
                HttpResponseMessage responseRounting = await Program.client.GetAsync(request);
                Console.WriteLine(responseRounting);
                responseRounting.EnsureSuccessStatusCode();
                responseBody = await responseRounting.Content.ReadAsStringAsync();
                
            }
            catch (Exception e)
            {
                return null;
            }
            Routing result = JsonSerializer.Deserialize<Routing>(responseBody);
            int id = 1;
            foreach (Step step in result.features[0].properties.segments[0].steps)
            {
                step.id = id;
                id++;
            }
            return result;
            }

        private string getDirections(Routing result)
        {
            List<string> instructions = new List<string>();
            ListOfQueues.Add(new MQ(nomQueueStandard + nbQueue, result.features[0].properties.segments[0].steps));
            nbQueue++;
            /*
            foreach (Step step in result.features[0].properties.segments[0].steps)
            {
                MyQueue.PushOnQueue(result.features[0].properties.segments[0].steps, step.id);
                instructions.Add(step.instruction);
            }*/
            //MyQueue.PushOnQueue(result.features[0].properties.segments[0].steps, id); //faux lancer activemq
            NextStep(ListOfQueues[ListOfQueues.Count-1].nomQueue);
            return ListOfQueues[ListOfQueues.Count - 1].nomQueue;
        }

        public async Task<Place> getAdressV2(string adress)
            {
                try
                {
                    HttpResponseMessage responseContract = await Program.client.GetAsync("https://nominatim.openstreetmap.org/search?q="+adress+"&format=json&polygon=1&addressdetails=1");
                    Console.WriteLine(responseContract);
                    responseContract.EnsureSuccessStatusCode();
                    string responseBody = await responseContract.Content.ReadAsStringAsync();
                    Place result = JsonSerializer.Deserialize<Place>(responseBody);
                    return result;
                }
                catch (Exception e)
                {

                }
                return null;
            }
        }



}
