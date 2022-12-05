using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executable
{
    public class Properties
    {
        public int housenumber { get; set; }
        public string name { get; set; }
        public string street { get; set; }
        public string postalcode { get; set; }
        public string locality { get; set; }

        public string ToString()
        {
            return "housenumber " + housenumber;
        }

    }
}
