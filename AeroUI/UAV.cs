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
                return Convert.ToDouble(DataStringArray[0]);
            }
        }
        public double Longitud
        {
            get
            {
                return Convert.ToDouble(DataStringArray[1]);
            }
        }
        public double Velocidad
        {
            get
            {
                return Convert.ToDouble(DataStringArray[2]);
            }
        }
        public double Altura
        {
            get
            {
                return Convert.ToDouble(DataStringArray[3]);
            }
        }
        public double Roll
        {
            get
            {
                return Convert.ToDouble(DataStringArray[4]);
            }
        }
        public double Pitch
        {
            get
            {
                return Convert.ToDouble(DataStringArray[5]);
            }
        }
        public double Yaw
        {
            get
            {
                return Convert.ToDouble(DataStringArray[6]);
            }
        }
        public double Distancia
        {
            get
            {
                return Convert.ToDouble(DataStringArray[7]);
            }
        }

        public double Tiempo
        {
            get
            {
                return Convert.ToDouble(DataStringArray[7]) / 1000;
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
