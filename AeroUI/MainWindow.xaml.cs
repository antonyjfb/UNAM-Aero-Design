﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace AeroUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UAV device = new UAV();
        // Lista completa de los datos 
        private List<DataLog> logUAV = new List<DataLog>();
        private bool toogle = false;

        bool recordingIsAvaible = false;
        bool initialTimeHasBeenSet = false;

        double initialTimeWhenRecording = 0;

        int numberOfFlight = 0;

        //private Thread threadUI;

        //Programa principal
        public MainWindow()
        {
            InitializeComponent();
            OcultarLabel();
            PnHome.Visibility = Visibility.Hidden;
            PnTakeOff.Visibility = Visibility.Hidden;
            PnFlightData.Visibility = Visibility.Hidden;
            PnMaps.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
            SearchPorts();
            device.NewDataPacketReceived += device_NewDataPacketReceived; //Declaracion evento que se ejecuta cada vez que se recibe un dato
            device.SetBaudRate(9600);
            //RealTimeUI_Setup();
            setLastNumberOfFlight();
        }
        private void setLastNumberOfFlight()
        {

            // Console.WriteLine("Current directory" + Directory.GetCurrentDirectory());

            string currentDate = DateTime.Now.ToString("yy_MM_dd");

            string path = @"../../Flights/" + currentDate;

            if (Directory.Exists(path))
            {
                Console.WriteLine("The directory " + currentDate + " exists");

                string[] fileEntries = Directory.GetFiles(path);

                if(fileEntries.Length != 0)
                {
                    foreach (string file in fileEntries)
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file);

                        int lastCharIndex = fileName.Length - 1;

                        char lastChar = fileName[lastCharIndex];

                        int numberOfLastFlight = Int32.Parse(lastChar.ToString());

                        if(numberOfFlight < numberOfLastFlight)
                        {
                            numberOfFlight = numberOfLastFlight;
                        }
                    }

                    numberOfFlight++;

                    Console.WriteLine("The last flight was:" + numberOfFlight);
                }
                else
                {
                    Console.WriteLine("There aren't any files");

                    numberOfFlight = 1;
                }

            }
            else
            {
                Console.WriteLine("The directory doesn't exist but It has just been created.");

                Directory.CreateDirectory(path);

                numberOfFlight = 1;
            }
        }

        //Código ejecutado cuando se recibe un nuevo dato
        private void device_NewDataPacketReceived(object sender, EventArgs e)
        {
            try
            {
                //UI_Update(device); COMENTADA HASTA NUEVO AVISO
                DataLog log = new DataLog(device);

                // Si se está grabando, se guardan los datos en logUAV
                if (recordingIsAvaible)
                {
                    if (!initialTimeHasBeenSet)
                    {
                        initialTimeWhenRecording = log.Tiempo;
                        initialTimeHasBeenSet = true;
                    }

                    // Se calcula el tiempo de acuerdo con lo recibido por el Arduino
                    log.Tiempo = log.Tiempo - initialTimeWhenRecording;

                    //Se agrega el nuevo dato recibido al registro completo de datos
                    logUAV.Add(log);
                }

                //Línea que contiene toda la información recopilada por los sensores
                Console.WriteLine(log.CSV_Line);

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Dentro de esta función se actualizan todos los elementos visuales que requieran de información de los sensores
                    actualizarValores(log);
                }));
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        private void actualizarValores(DataLog log)
        {
            //Cada vez que se agregue un tipo de visualización compleja Ej: Gráfica
            //se añadirá como una función Ej: ModificarGrafica(parametros)
            //Para obtener la información usar la sintaxis log.datoRequerido Ej: log.PosX devuelve la posición en X
            lblLat.Content = log.Latitud;
            lblLong.Content = log.Longitud;
            lblSpeed.Content = log.Velocidad;
            lblAlt.Content = log.Altura;
            
            if (recordingIsAvaible)
            {
                lblRecTime.Content = string.Format("{0:0.00}", log.Tiempo);
            }
        }

        private void Conexion_Click(object sender, RoutedEventArgs e)
        {
            if(!toogle)
            {
                AbrirConexion();
                MostrarLabel();
            }
            else
            {
                CerrarConexion();
            }
            toogle = !toogle;
        }
        private void OcultarLabel()
        {
            lblLat.Visibility = Visibility.Hidden;
            lblLong.Visibility = Visibility.Hidden;
            lblSpeed.Visibility = Visibility.Hidden;
            lblAlt.Visibility = Visibility.Hidden;
        }
        private void MostrarLabel()
        {
            lblLat.Visibility = Visibility.Visible;
            lblLong.Visibility = Visibility.Visible;
            lblSpeed.Visibility = Visibility.Visible;
            lblAlt.Visibility = Visibility.Visible;
        }
        private void CerrarConexion()
        {
            try
            {
                device.StopDataFlow();
                //threadUI.Abort();
                device.StopConnection();
                //threadUI.Abort();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void AbrirConexion()
        {
            try
            {
                string puerto = comboBox1.SelectedItem.ToString();
                device.OpenConnection(puerto);
                device.BeginDataFlow();

                //Funciones comentadas hasta nuevo aviso

                //ThreadStart methodUI = new ThreadStart(UI_Thread);
                //threadUI = new Thread(methodUI);
                //threadUI.Start();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            SearchPorts();
        }
        private void SearchPorts()
        {
            comboBox1.Items.Clear();
            string[] puertos = SerialPort.GetPortNames();
            foreach (string puerto in puertos)
            {
                comboBox1.Items.Add(puerto);
            }
        }

        //COMENTADO HASTA NUEVO AVISO

        //Proceso en el hilo, independiente de la ejecución del resto del programa
        /*private void RealTimeUI_Setup()
        {
            Thread.Sleep(100);
        }
        delegate void UI_UpdateDelegate(UAV Mobula); //Debe recibir mismos argumentos que la función a delegar
        private void UI_Update(UAV Mobula)
        {
            if (!Dispatcher.CheckAccess())
            {
                UI_UpdateDelegate threadMethod = new UI_UpdateDelegate(UI_Update);
                Dispatcher.Invoke(threadMethod, Mobula);
            }
            else //Actualiza los valores de la UI
            {
                ModifyChart(Mobula);
            }
        }
        private void ModifyChart(UAV Mobula)
        {
            Xpos_Label.Content = Mobula.PosX;
        }

        private void UI_Thread()
        {

            Thread.Sleep(1500);
            for (; ; )
            {
                Thread.Sleep(100);
                UI_Update(device);
            }
        }*/

        // Se comentó el método Save_Click porque, en su lugar, se utiliza el método stopRecording

        /*
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string CSVheader = "";
            string CSVJoin = String.Join("\n", logUAV.Select(m => m.CSV_Line)); //Concatena las CSVLine de todos los objetos de la lista
            string CSVLog = CSVheader + CSVJoin;
            System.IO.File.WriteAllText(@"..\..\log.csv", CSVLog);
        }*/

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            CerrarConexion();
            Environment.Exit(Environment.ExitCode);
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_take_off.Visibility = Visibility.Collapsed;
                tt_Flight_Data.Visibility = Visibility.Collapsed;
                tt_location.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
                tt_exit.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_take_off.Visibility = Visibility.Visible;
                tt_Flight_Data.Visibility = Visibility.Visible;
                tt_location.Visibility = Visibility.Visible;
                tt_settings.Visibility = Visibility.Visible;
                tt_exit.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void LvHome_Selected(object sender, RoutedEventArgs e)
        {
            PnHome.Visibility = Visibility.Visible;
            PnTakeOff.Visibility = Visibility.Hidden;
            PnFlightData.Visibility = Visibility.Hidden;
            PnMaps.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
        }

        private void LvTakeOff_Selected(object sender, RoutedEventArgs e)
        {
            PnTakeOff.Visibility = Visibility.Visible;
            PnHome.Visibility = Visibility.Hidden;
            PnFlightData.Visibility = Visibility.Hidden;
            PnMaps.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
        }

        private void LvFlightData_Selected(object sender, RoutedEventArgs e)
        {
            PnFlightData.Visibility = Visibility.Visible;
            PnTakeOff.Visibility = Visibility.Hidden;
            PnHome.Visibility = Visibility.Hidden;
            PnMaps.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
        }

        private void LvMaps_Selected(object sender, RoutedEventArgs e)
        {
            PnMaps.Visibility = Visibility.Visible;
            PnFlightData.Visibility = Visibility.Hidden;
            PnTakeOff.Visibility = Visibility.Hidden;
            PnHome.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
        }

        private void LvSettings_Selected(object sender, RoutedEventArgs e)
        {
            PnSettings.Visibility = Visibility.Visible;
            PnMaps.Visibility = Visibility.Hidden;
            PnFlightData.Visibility = Visibility.Hidden;
            PnTakeOff.Visibility = Visibility.Hidden;
            PnHome.Visibility = Visibility.Hidden;
        }

        private void LvSettings1_Selected(object sender, RoutedEventArgs e)
        {
            CerrarConexion();
            Close();
        }

        private void startRecording(object sender, RoutedEventArgs e)
        {
            recordingIsAvaible = true;
        }

        private void stopRecording(object sender, RoutedEventArgs e)
        {
            recordingIsAvaible = false;
            initialTimeHasBeenSet = false;
            lblRecTime.Content = 0;

            string currentDate = DateTime.Now.ToString("yy_MM_dd");
            string fileName = currentDate + "-" + "flight" + numberOfFlight + ".csv";

            string CSVheader = "";
            string CSVJoin = String.Join("\n", logUAV.Select(m => m.CSV_Line)); //Concatena las CSVLine de todos los objetos de la lista
            string CSVLog = CSVheader + CSVJoin;

            string path = @"../../Flights/" + currentDate + "/";
            System.IO.File.WriteAllText(path + fileName, CSVLog);

            numberOfFlight++;
        }
    }
}
