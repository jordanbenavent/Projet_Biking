using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProxySOAP
{
    internal class Program
    {
        public static readonly HttpClient client = new HttpClient();
        IProxy service = new Proxy();
    }
}
