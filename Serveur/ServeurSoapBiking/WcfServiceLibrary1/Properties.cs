namespace ServeurSoapBiking
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
