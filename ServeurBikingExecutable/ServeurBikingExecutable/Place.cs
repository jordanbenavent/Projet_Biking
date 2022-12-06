using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServeurBikingExecutable
{
    public class Adress2
    {
        public string city;
        public string town;
        public string village;
    }
    public class Place
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public Adress2 adress { get; set; }
    }
}
