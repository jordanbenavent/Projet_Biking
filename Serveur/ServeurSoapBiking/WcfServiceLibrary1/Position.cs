namespace ServeurSoapBiking
{
    public class Position
    {
        public double[] coordinates { get; set; }

        public string getLatitudeString()
        {
            string latitude = coordinates[1].ToString();
            return latitude.Replace(',', '.');
        }

        public string getLongitudeString()
        {
            string longitude = coordinates[0].ToString();
            return longitude.Replace(',', '.');
        }

        public double getLatitude()
        {
            return coordinates[1];
        }

        public double getLongitude()
        {
            return coordinates[0];
        }


    }
}
