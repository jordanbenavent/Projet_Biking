namespace ServeurSoapBiking
{
    public class Position
    {
        public double[] coordinates { get; set; }

        public string getLongitudeString()
        {
            string longitude = coordinates[0].ToString();
            return longitude.Replace(',', '.');
        }

        public string getLattitudeString()
        {
            string lattitude = coordinates[1].ToString();
            return lattitude.Replace(',', '.');
        }
    }
}