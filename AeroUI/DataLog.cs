using System;

namespace AeroUI
{
    public class DataLog
    {
        private double latitud, longitud, velocidad, velocidadZ, altura, roll, pitch, yaw, distancia, tiempo, aceleracionX, aceleracionY, aceleracionZ; //Debe tener las mismas variables que UAV
        private int liberacion;
        private string csv_line;

        public double Tiempo
        { get { return tiempo; } set { tiempo = value; } }
        public double Latitud
        { get { return latitud; } }
        public double Longitud
        { get { return longitud; } }
        public double Velocidad
        { get { return velocidad; } }
        public double VelocidadZ
        { get { return velocidadZ; } }
        public double Altura
        { get { return altura; } }
        public double AceleracionX
        { get { return aceleracionX; } }
        public double AceleracionY
        { get { return aceleracionX; } }
        public double AceleracionZ
        { get { return aceleracionX; } }
        public double Roll
        { get { return roll; } }
        public double Pitch
        { get { return pitch; } }
        public double Yaw
        { get { return yaw; } }
        public double Distancia
        { get { return distancia; } }

        public int Liberacion
        { get { return liberacion; } set { liberacion = value; } }

        public string CSV_Line
        {
            get
            {
                string[] parameters = new string[]
                {
                    string.Format("{0:0.00}", tiempo),
                    velocidad.ToString(),
                    latitud.ToString(),
                    longitud.ToString(),
                    distancia.ToString(),
                    altura.ToString(),
                    velocidadZ.ToString(),
                    aceleracionX.ToString(),
                    aceleracionY.ToString(),
                    aceleracionZ.ToString(),
                    roll.ToString(),
                    pitch.ToString(),
                    yaw.ToString(),
                    liberacion.ToString()
                };

                csv_line = string.Join(",", parameters);

                return csv_line;
            }
        }

        public DataLog(UAV dataDevice)
        {
            tiempo = dataDevice.Tiempo;
            velocidad = dataDevice.Velocidad;
            latitud = dataDevice.Latitud;
            longitud = dataDevice.Longitud;
            distancia = dataDevice.Distancia;
            altura = dataDevice.Altura;
            velocidadZ = dataDevice.VelocidadZ;
            aceleracionX = dataDevice.AceleracionX;
            aceleracionY = dataDevice.AceleracionY;
            aceleracionZ = dataDevice.AceleracionZ;
            roll = dataDevice.Roll;
            pitch = dataDevice.Pitch;
            yaw = dataDevice.Yaw;
            liberacion = 0;

            // csv_line = dataDevice.CSV_Line;
        }

        public DataLog(string loadedData)
        {
            string[] arrayOfLoadedData = loadedData.Split(',');

            Double.TryParse(arrayOfLoadedData[0], out tiempo);
            Double.TryParse(arrayOfLoadedData[1], out velocidad);
            Double.TryParse(arrayOfLoadedData[2], out latitud);
            Double.TryParse(arrayOfLoadedData[3], out longitud);
            Double.TryParse(arrayOfLoadedData[4], out distancia);
            Double.TryParse(arrayOfLoadedData[5], out altura);
            Double.TryParse(arrayOfLoadedData[6], out velocidadZ);
            Double.TryParse(arrayOfLoadedData[7], out aceleracionX);
            Double.TryParse(arrayOfLoadedData[8], out aceleracionY);
            Double.TryParse(arrayOfLoadedData[9], out aceleracionZ);
            Double.TryParse(arrayOfLoadedData[10], out roll);
            Double.TryParse(arrayOfLoadedData[11], out pitch);
            Double.TryParse(arrayOfLoadedData[12], out yaw);
            Int32.TryParse(arrayOfLoadedData[13], out liberacion);
  
        }

        public double getDistanceToTarget(double targetLatitude, double targetLongitude)
        {
            double distance = -1;
            double aircraftLatitude = this.latitud;
            double aircraftLongitude = this.Longitud;
            bool aircraftLocationIsValid = aircraftLatitude != 0 && aircraftLongitude != 0;

            if(aircraftLocationIsValid)
            {
                double d_latitude = targetLatitude - aircraftLatitude;
                double d_longitude = targetLongitude - aircraftLongitude;
                distance = Math.Acos(Math.Sin(aircraftLatitude) * Math.Sin(targetLatitude) + Math.Cos(aircraftLatitude) * Math.Cos(targetLatitude) * Math.Cos(targetLongitude - aircraftLongitude)) * 111180;
            }

            return distance;
        }

    }
}
