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
            string depart = Console.ReadLine();
            string arrival = Console.ReadLine();
            string result = service.getRoute(depart, arrival);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
