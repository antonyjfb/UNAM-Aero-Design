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

                if (!String.IsNullOrEmpty(DataStringArray[2]))
                {
                    latitud = Convert.ToDouble(DataStringArray[2]);
                }
                else
                {
                    Console.WriteLine("DataStringArray[0] es nulo o está vacío");
                }

                return latitud;
            }
        }
        public double Longitud
        {
            get
            {
                double longitud = 0;

                if (!String.IsNullOrEmpty(DataStringArray[3]))
                {
                    longitud = Convert.ToDouble(DataStringArray[3]);
                }
                else
                {
                    Console.WriteLine("DataStringArray[1] es nulo o está vacío");
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
