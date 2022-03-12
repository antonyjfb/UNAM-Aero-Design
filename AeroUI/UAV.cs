using System;

namespace AeroUI
{
    public class UAV : SerialPortDevice
    {
        //Declarar variables del UAV con su respectiva posición del arreglo de strings obtenido en el serial
        public double Latitud
        {
            get
            {
                double latitud = 0;

                if(double.TryParse(DataStringArray[2], out latitud))
                {
                    Console.WriteLine("El dato recibido de latitud es un dato numérico.");
                }
                else
                {
                    Console.WriteLine("El dato recibido de latitud NO es un dato numérico");
                }

                return latitud;
            }
        }
        public double Longitud
        {
            get
            {
                double longitud = 0;

                if(double.TryParse(DataStringArray[3], out longitud))
                {
                    Console.WriteLine("El dato recibido de longitud es un dato numérico.");
                }
                else
                {
                    Console.WriteLine("El dato recibido de longitud NO es un dato numérico");
                }

                return longitud;
            }
        }
        public double Velocidad
        {
            get
            {
                return Convert.ToDouble(DataStringArray[1]);
            }
        }

        public double VelocidadZ
        {
            get
            {
                return Convert.ToDouble(DataStringArray[6]);
            }
        }

        public double Altura
        {
            get
            {
                return Convert.ToDouble(DataStringArray[5]);
            }
        }
        public double AceleracionX
        {
            get
            {
                return Convert.ToDouble(DataStringArray[7]);
            }
        }
        public double AceleracionY
        {
            get
            {
                return Convert.ToDouble(DataStringArray[8]);
            }
        }
        public double AceleracionZ
        {
            get
            {
                return Convert.ToDouble(DataStringArray[9]);
            }
        }
        public double Roll
        {
            get
            {
                return Convert.ToDouble(DataStringArray[10]);
            }
        }
        public double Pitch
        {
            get
            {
                return Convert.ToDouble(DataStringArray[11]);
            }
        }
        public double Yaw
        {
            get
            {
                return Convert.ToDouble(DataStringArray[12]);
            }
        }
        public double Distancia
        {
            get
            {
                return Convert.ToDouble(DataStringArray[4]);
            }
        }

        public double Tiempo
        {
            get
            {
                return Convert.ToDouble(DataStringArray[0]) / 1000;
            }
        }

        public string CSV_Line
        {
            get
            {
                return ConvertStringArrayToCSVLine(DataStringArray);
            }
        }
    }
}
