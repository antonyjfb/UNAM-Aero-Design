using System;

namespace AeroUI
{
    public class UAV : SerialPortDevice
    {
        //Declarar variables del UAV con su respectiva posición del arreglo de strings obtenido en el serial
        public double PosX
        {
            get
            {
                return Convert.ToDouble(DataStringArray[0]);
            }
        }
        public double PosY
        {
            get
            {
                return Convert.ToDouble(DataStringArray[1]);
            }
        }
        public double PosZ
        {
            get
            {
                return Convert.ToDouble(DataStringArray[2]);
            }
        }
        public double AlerIzq
        {
            get
            {
                return Convert.ToDouble(DataStringArray[3]);
            }
        }
        public double AlerDer
        {
            get
            {
                return Convert.ToDouble(DataStringArray[4]);
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
