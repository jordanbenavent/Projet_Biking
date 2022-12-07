using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServeurSoapBiking
{
    internal class Program
    {
        public static readonly HttpClient client = new HttpClient();
        IServiceBiking service = new ServiceBiking();
    }
}
