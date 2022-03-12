using System;

namespace AeroUI
{
    public class DataLog
    {
        private double latitud, longitud, velocidad, velocidadZ, altura, roll, pitch, yaw, distancia, tiempo, aceleracionX, aceleracionY, aceleracionZ; //Debe tener las mismas variables que UAV
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


            // csv_line = dataDevice.CSV_Line;
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
