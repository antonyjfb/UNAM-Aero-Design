namespace AeroUI
{
    public class DataLog
    {
        private double latitud, longitud, velocidad, altura, roll, pitch, yaw, distancia, tiempo; //Debe tener las mismas variables que UAV
        private string csv_line;

        public double Latitud
        { get { return latitud; } }
        public double Longitud
        { get { return longitud; } }
        public double Velocidad
        { get { return velocidad; } }
        public double Altura
        { get { return altura; } }
        public double Roll
        { get { return roll; } }
        public double Pitch
        { get { return pitch; } }
        public double Yaw
        { get { return yaw; } }
        public double Distancia
        { get { return distancia; } }
        public double Tiempo
        { get { return tiempo; } set { tiempo = value; } }
        public string CSV_Line
        { get 
            {
                string[] parameters = new string[]
                {
                    string.Format("{0:0.00}", tiempo),
                    latitud.ToString(),
                    longitud.ToString(),
                    velocidad.ToString(),
                    altura.ToString(),
                    roll.ToString(),
                    pitch.ToString(),
                    yaw.ToString(),
                    distancia.ToString()
                };

                csv_line = string.Join(",", parameters);

                return csv_line; 
            } 
        }


        public DataLog(UAV dataDevice)
        {
            latitud = dataDevice.Latitud;
            longitud = dataDevice.Longitud;
            velocidad = dataDevice.Velocidad;
            altura = dataDevice.Altura;
            roll = dataDevice.Roll;
            pitch = dataDevice.Pitch;
            yaw = dataDevice.Yaw;
            distancia = dataDevice.Distancia;
            tiempo = dataDevice.Tiempo;
            // csv_line = dataDevice.CSV_Line;
        }


    }
}
