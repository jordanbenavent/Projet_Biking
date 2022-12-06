using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using static System.Net.WebRequestMethods;
using System.Device.Location;
using System.Diagnostics.Contracts;
using System.Web.Management;
using System.Runtime.CompilerServices;
using ServeurBikingExecutable;

namespace ServeurBikingExecutable
{

    public class Adress
    {

        /*public string path { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }*/

        public List<Feature> features { get; set; }

        public bool Is_Empty()
        {
            return features.Count == 0;
        }

        public string ToString()
        {
            return features[0].properties.locality;
        }

        public string GetCity()
        {
            foreach (Feature feature in features)
            {
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


    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.



    public class ServiceBiking : IServiceBiking
    {

        public static readonly HttpClient client = new HttpClient();

        public static List<MQ> ListOfQueues = new List<MQ>();
        public string nomQueueStandard = "QueueServiceBiking";
        public static int nbQueue = 0;
        private string apiKeyORS = "api_key=5b3ce3597851110001cf6248560a9124ee2b4b0591d9dcdaf3179440";
        //private string apiKeyORS = "api_key=5b3ce3597851110001cf62485cbfe0ea6a384b7c84c5fb1cdf8fc5a4";


        public string getRoute(string departure, string arrival)
        {   
            Task<Adress> departAdress = getAdress(departure);
            Task<Adress> arrivalAdress = getAdress(arrival);
            if (departAdress.Result==null || departAdress.Result.Is_Empty()) { return "Une erreur est survenue sur l'adresse de depart."; }
            if (arrivalAdress.Result.Is_Empty()) { return "Une erreur est survenue sur l'adresse d'arrivée."; }
            Task<Station> departStation = GetStationClosestToLocalisation(departAdress);
            Task<Station> arrivalStation = GetStationClosestToLocalisation(arrivalAdress);

            Position positionDepartStation = getPosisitionOfStation(departStation.Result);
            Position positionArrivalStation = getPosisitionOfStation(arrivalStation.Result);

            List<Routing> routing = calculateAllRounting(departAdress.Result, arrivalAdress.Result, positionDepartStation, positionArrivalStation);
            string directions = getDirections(routing, departStation.Result, arrivalStation.Result);
            Console.Write(directions);
            return directions;
        }

        private List<Routing> calculateAllRounting(Adress departAdress, Adress arrivalAdress, Position positionDepartStation, Position positionArrivalStation)
        {
            Task<Routing> routingwalkingonly = getRouting(departAdress.features[0].geometry, arrivalAdress.features[0].geometry, "foot-walking?");
            // Itinéraire n°1, départ - station de départ
            Task<Routing> routingwalkingdepart = getRouting(departAdress.features[0].geometry, positionDepartStation, "foot-walking?");
            // Itinéraire n°2, station de départ - station d'arrivée
            Task<Routing> routingbiking = getRouting(positionDepartStation, positionArrivalStation, "cycling-road?");
            // Itinéraire n°3, station d'arrivée - arrivée
            Task<Routing> routingwalkingarrival = getRouting(positionArrivalStation, arrivalAdress.features[0].geometry, "foot-walking?");
            /*
            if (routingwalkingonly.Result == null) { return "Une erreur est survenue dans la création de l'itinéraire : départ - arrivée à pied"; }
            if (routingwalkingdepart.Result == null) { return "Une erreur est survenue dans la création de l'itinéraire : départ - station de départ"; }
            if (routingbiking.Result == null) { return "Une erreur est survenue dans la création de l'itinéraire : station de départ - station d'arrivée"; }
            if (routingwalkingarrival.Result == null) { return "Une erreur est survenue dans la création de l'itinéraire : station d'arrivée - arrivée"; }
            */
            //possible autre routing
            double durationJCDecaux = routingwalkingdepart.Result.features[0].properties.segments[0].duration
                            + routingbiking.Result.features[0].properties.segments[0].duration
                            + routingwalkingarrival.Result.features[0].properties.segments[0].duration;
            double durationWalkingOnly = routingwalkingonly.Result.features[0].properties.segments[0].duration;
            List<Routing> routing = new List<Routing>();
            if (durationWalkingOnly < durationJCDecaux)
            {
                routing.Add(routingwalkingonly.Result);
            }
            else
            {
                routing.Add(routingwalkingdepart.Result);
                routing.Add(routingbiking.Result);
                routing.Add(routingwalkingarrival.Result);
            }
            return routing;
        }

        private Position getPosisitionOfStation(Station stationProche)
        {

            // conversion positionJCDecaux -> position
            Position pos = new Position();
            pos.coordinates = new double[] { stationProche.position.longitude, stationProche.position.latitude };
            return pos;
        }

        public bool NextStep(string queue)
        {
            MQ userQueue = getQueue(queue);
            if (userQueue == null) { return false; }

            if (userQueue.IsDone())
            {
                return false;
            }
            if (userQueue.needRecalculateRouting())
            {
                recalculateRouting(userQueue);
            }
            userQueue.PushOnQueue();
            return true;

        }

        private MQ getQueue(string userqueue)
        {
            foreach (MQ queue in ListOfQueues)
            {
                if (queue.nomQueue.Equals(userqueue))
                {
                    return queue;
                }
            }
            return null;
        }

        public async Task<Adress> getAdress(string adress)
        {
            string url = "https://api.openrouteservice.org/geocode/search?" + apiKeyORS + "&text=" + adress;
            try
            {
                HttpResponseMessage responseContract = await client.GetAsync("https://api.openrouteservice.org/geocode/search?" + apiKeyORS + "&text=" + adress);
                Console.WriteLine(responseContract);
                responseContract.EnsureSuccessStatusCode();
                string responseBody = await responseContract.Content.ReadAsStringAsync();
                Adress result = JsonSerializer.Deserialize<Adress>(responseBody);
                return result ;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        async Task<Routing> getRouting(Position start, Position end, string typeTransport)
        {
            string responseBody;
            string request = "https://api.openrouteservice.org/v2/directions/" + typeTransport + apiKeyORS + "&start=" + start.getLongitudeString() + ',' + start.getLatitudeString() + "&end=" + end.getLongitudeString() + "," + end.getLatitudeString();

            try
            {

                HttpResponseMessage responseRounting = await client.GetAsync(request);
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

        private string getDirections(List<Routing> result, Station departure, Station arrival)
        {
            List<string> instructions = new List<string>();
            List<Step> allSteps = new List<Step>();
            for (int i = 0; i < result.Count; i++)
            {
                allSteps.AddRange(result[i].features[0].properties.segments[0].steps);
            }
            MQ queue; //= new MQ(nomQueueStandard + nbQueue, allSteps);
            if (result.Count == 3)
            {
                queue = new MQ(nomQueueStandard + nbQueue, result[0], result[1], result[2], departure, arrival);
                //queue.stepsWalkingDeparture = result[0].features[0].properties.segments[0].steps;
                //queue.stepsBiking = result[1].features[0].properties.segments[0].steps;
                //queue.stepsWalkingArrival = result[2].features[0].properties.segments[0].steps;
            }
            else
            {
                queue = new MQ(nomQueueStandard + nbQueue, result[0]);
            }
            ListOfQueues.Add(queue);
            nbQueue++;
            NextStep(ListOfQueues[ListOfQueues.Count - 1].nomQueue);
            return ListOfQueues[ListOfQueues.Count - 1].nomQueue;
        }

        public async Task<Place> getAdressV2(string adress)
        {
            try
            {
                HttpResponseMessage responseContract = await client.GetAsync("https://nominatim.openstreetmap.org/search?q=" + adress + "&format=json&polygon=1&addressdetails=1");
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

        public async Task<Station> GetStationClosestToLocalisation(Task<Adress> localisation)
        {

            try
            {
                string responseBody = await client.GetStringAsync("https://api.jcdecaux.com/vls/v1/contracts?apiKey=98382454fc46549c5cdf105c9dcf4578e6cbea91");
                Contract[] contract = JsonSerializer.Deserialize<Contract[]>(responseBody);
                string chosenContract = chooseContract(localisation, contract);
                string responseContractBody = await client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations?contract=" + chosenContract + "&apiKey=98382454fc46549c5cdf105c9dcf4578e6cbea91");
                Station[] stationsOfaCity = JsonSerializer.Deserialize<Station[]>(responseContractBody);

                GeoCoordinate geoCoordinateOfLocalisation = new GeoCoordinate(localisation.Result.GetPosition().getLatitude(), localisation.Result.GetPosition().getLongitude());
                int numberproche = 0;
                double distance = double.MaxValue;

                foreach (Station station in stationsOfaCity)
                {
                    GeoCoordinate geoCoordinateOfTheStation = new GeoCoordinate(station.position.latitude, station.position.longitude);
                    if (distance > geoCoordinateOfLocalisation.GetDistanceTo(geoCoordinateOfTheStation))
                    {
                        distance = geoCoordinateOfLocalisation.GetDistanceTo(geoCoordinateOfTheStation);
                        numberproche = station.number;
                    }
                }

                string responseStationProcheBody = await client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations/" + numberproche + "?contract=" + chosenContract + "&apiKey=98382454fc46549c5cdf105c9dcf4578e6cbea91");

                Station stationProche = JsonSerializer.Deserialize<Station>(responseStationProcheBody);



                return stationProche;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        
        public string chooseContract(Task<Adress> localisation, Contract[] contracts)
        {
            string city = localisation.Result.GetCity().ToLower();
            string chosenContract = "not found";
            foreach (Contract contract1 in contracts)
            {
                if (city.Equals(contract1.name))
                {
                    chosenContract = city;
                    break;
                }
            }
            if (chosenContract.Equals("not found"))
            {
                GeoCoordinate geoCoordinateOfLocalisation = new GeoCoordinate(localisation.Result.GetPosition().getLatitude(), localisation.Result.GetPosition().getLongitude());
                double distance = double.MaxValue;
                foreach (Contract contract1 in contracts)
                {
                    Task<Adress> address = getAdress(contract1.name);
                    if (address.Result != null)
                    {
                        if (!(address.Result.Is_Empty()))
                        {
                            GeoCoordinate geoCoordinateOfTheContractCity = new GeoCoordinate(address.Result.features[0].geometry.getLatitude(), address.Result.features[0].geometry.getLongitude());
                            if (distance > geoCoordinateOfLocalisation.GetDistanceTo(geoCoordinateOfTheContractCity))
                            {
                                distance = geoCoordinateOfLocalisation.GetDistanceTo(geoCoordinateOfTheContractCity);
                                chosenContract = contract1.name;
                            }
                        }
                    }
                }
            }

            return chosenContract;
        }
        

        private void recalculateRouting(MQ userQueue)
        {
            if (userQueue.status.Equals(StatusRouting.WALKING))
            {
                this.recalculateAllRounting(userQueue);
            }
            else if (userQueue.status.Equals(StatusRouting.BIKING))
            {
                recalculateBikingRounting(userQueue);
            }
            //plus nécessaire de recalculer, l'utilisateur est à pied et proche du point d'arriver (discuter avec le prof)
        }

        private static void recalculateBikingRounting(MQ userQueue)
        {
            //throw new NotImplementedException();
        }

        private void recalculateAllRounting(MQ userQueue)
        {
            //Task<Adress> adressDepart = userQueue.stepsWalkingDeparture[userQueue.lastPushWalkingDeparture].
            //List<Routing> routing = calculateAllRounting();
        }

        private Position posisitonFromRouting(Routing routing, int numStep, int point)
        {
            Position position = new Position();
            int userPoint = routing.features[0].properties.segments[0].steps[numStep].way_points[point];
            //routing.features[0].geometry.coordinates[userPoint];
            return position;
        }
    }







}
