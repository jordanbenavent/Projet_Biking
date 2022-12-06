﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executable
{
    public class Routing
    {
        public List<FeatureRouting> features { get; set; }
    }

    public class FeatureRouting
    {
        public PropertiesRounting properties { get; set; }

        public CoordonateRouting geometry { get; set; }
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

        //public double Get
    }

    public class Step
    {
        public Step(string instruction)
        {

            this.instruction = instruction;

        }

        public int id { get; set; }
        public double distance { get; set; }
        public double duration { get; set; }
        public string instruction { get; set; }
        public int[] way_points { get; set; }
    }
}