namespace ServeurSoapBiking
{
    public class Position
    {
        public double[] coordinates { get; set; }

        public string getLongitudeString()
        {
            int partEnt = (int)coordinates[0];
            return partEnt + "." + (coordinates[0] * 10000) % (partEnt * 10000);
        }

        public string getLattitudeString()
        {
            int partEnt = (int)coordinates[1];
            return partEnt + "." + (coordinates[1] * 10000) % (partEnt * 10000);
        }
    }
}