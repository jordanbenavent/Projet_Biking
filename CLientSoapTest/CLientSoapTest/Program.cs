using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLientSoapTest.ServiceReference1;

namespace CLientSoapTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceBikingClient service = new ServiceBikingClient();
            Console.WriteLine("De où voulez-vous partir ?");
            // Place du Général-de-Gaulle, Rouen
            string depart = Console.ReadLine();
            Console.WriteLine("Où voulez-vous aller ?");
            // Avenue Martyrs de la Résistance, Rouen
            string arrival = Console.ReadLine();
            string result = service.getRoute(depart, arrival);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
