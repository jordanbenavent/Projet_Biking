using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServeurSoapBiking
{
    public class PositionJCDecaux
    {
        public double latitude { get; set; }
        public double longitude { get; set;}
    }

    public class Station
    {
        public int number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public PositionJCDecaux position { get; set; }
    }
}
