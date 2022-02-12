using System;
using System.IO.Ports;

namespace AeroUI
{
    public class SerialPortDevice
    {
        private SerialPort board = new SerialPort();
        public EventHandler NewDataPacketReceived;

        //Cadena de datos entera obtenida de cada loop
        private string dataPacket = "";

        //Arreglo de datos individuales
        private string[] dataArray;

        //Arreglo de datos individuales público
        public string[] DataStringArray
        {
            get
            {
                return dataArray;
            }
        }

        //Establece un nuevo baudRate, debe ser coincidente con el utilizado por el dispositivo serial
        public void SetBaudRate(int baudRateValue)
        {
            board.BaudRate = baudRateValue;
        }

        //Manda una cadena al dispositivo para que este empiece a escribir datos en el serial
        public void BeginDataFlow()
        {
            try
            {
                if (board.IsOpen)
                {
                    board.Write("1#");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Manda una cadena al dispositivo para que este deje de escribir datos en el serial
        public void StopDataFlow()
        {
            try
            {
                if (board.IsOpen)
                {
                    board.Write("0#");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Inicia el enlace con el dispositivo serial
        public void OpenConnection(string port)
        {
            try
            {
                if (!board.IsOpen)
                {
                    if (port != null || port != "")
                    {
                        board.DataReceived += board_DataReceived;
                        board.PortName = port;
                        board.Open();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Detiene el enlace con el dispositivo serial
        public void StopConnection()
        {
            try
            {
                board.DiscardInBuffer();
                board.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Proceso a ejecutar cuando recibe una cadena, el inicio de la cadena debe ser un \x02 y el final un \x03 se identifica cada valor individual con coma
        //Ejemplo de cadena que debe ser enviada por el dispositivo serial  "\x0224.5,43.6,5.21\x03" Salida: dateArray[0]=24.5,dateArray[1]=43.6,dateArray[2]=5.21
        private void board_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                dataPacket = board.ReadTo("\x03");
                string[] serialString = dataPacket.Split(new string[] { "\x02" }, StringSplitOptions.RemoveEmptyEntries);
                dataPacket = serialString[0];

                if (NewDataPacketReceived != null)
                {
                    dataArray = dataPacket.Split(',');
                    Console.WriteLine("dataArray: ");
                    Console.WriteLine("[{0}]", string.Join(",", dataArray));
                    NewDataPacketReceived(this, new EventArgs());
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        public static string ConvertStringArrayToCSVLine(string[] array)
        {
            string result = string.Join(",", array);
            return result;
        }
    }
}
