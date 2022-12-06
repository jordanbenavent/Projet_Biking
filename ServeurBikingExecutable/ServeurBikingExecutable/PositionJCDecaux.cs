using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServeurBikingExecutable
{
    public class PositionJCDecaux
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public enum StatusStation
    {
        OPEN, CLOSE
    }

    public class Station
    {
        public int number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public PositionJCDecaux position { get; set; }
        public int available_bike_stands { get; set; }

        public int available_bikes { get; set; }

        public string status { get; set; }
    }
}
