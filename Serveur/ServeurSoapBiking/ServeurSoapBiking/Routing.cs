using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServeurSoapBiking
{
    internal class Routing
    {
        public List<FeatureRouting> features { get; set; }
    }

    class FeatureRouting
    {
        public PropertiesRounting properties { get; set; }
    }

    public class PropertiesRounting
    {
        public List<Segment> segments { get; set; }
    }

    public class Segment
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public List<Step> steps { get; set; }

        public CoordonateRouting geometry { get; set; }
    }

    public class CoordonateRouting
    {
        public double[][] coordinates { get; set; }
    }

    public class Step
    {
        public int id { get; set; }
        public double distance { get; set; }
        public double duration { get; set; }
        public string instruction { get; set; }
        public int[] way_points { get; set; }
    }
}
