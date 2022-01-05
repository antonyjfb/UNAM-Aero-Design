namespace AeroUI
{
    public class DataLog
    {
        private double posx, posy, posz, alerIzq, alerDer; //Debe tener las mismas variables que UAV
        private string csv_line;

        public double PosX
        { get { return posx; } }
        public double PosY
        { get { return posy; } }
        public double PosZ
        { get { return posz; } }
        public double AlerIzq
        { get { return alerIzq; } }
        public string CSV_Line
        { get { return csv_line; } }


        public DataLog(UAV dataDevice)
        {
            posx = dataDevice.PosX;
            posy = dataDevice.PosY;
            posz = dataDevice.PosZ;
            alerIzq = dataDevice.AlerIzq;
            alerDer = dataDevice.AlerDer;
            csv_line = dataDevice.CSV_Line;
        }


    }
}
