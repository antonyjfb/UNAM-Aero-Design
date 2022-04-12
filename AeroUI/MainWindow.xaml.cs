using System;
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
using Microsoft.Maps.MapControl.WPF;

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

        // When recording
        bool recordingIsAvaible = false;
        bool initialTimeHasBeenSet = false;
        double initialTimeWhenRecording = 0;

        // When saving flight data
        int numberOfFlight = 0;

        // When calculating distance from aircraft to target
        string targetLatitude_String = "19.424184";
        string targetLongitude_String = "-99.134937";

        //variable para el control del boton de Record Flight Data
        bool ON = true;

        //private Thread threadUI;

        // GPS
        bool firstLocationDataHasBeenSet = false;
        bool targetLocationHasBeenSet = false;
        double center_latitude = 0;
        double center_longitude = 0;
        double targetLatitude;
        double targetLongitude;
        string movingMapDirection = "Up";
        Location aircraftLocation = new Location(19.424184, -99.134937);
        Location centerLocation = new Location(19.424184, -99.134937);
        Location targetLocation;
        Pushpin targetPin;
        LocationCollection aircraftLocationsCollection = new LocationCollection();

        //Programa principal
        public MainWindow()
        {
            InitializeComponent();
            OcultarLabel();
            PnTakeOff.Visibility = Visibility.Hidden;
            PnFlightData.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
            SearchPorts();
            device.NewDataPacketReceived += device_NewDataPacketReceived; //Declaracion evento que se ejecuta cada vez que se recibe un dato
            device.SetBaudRate(9600);
            //RealTimeUI_Setup();
            setLastNumberOfFlight();
            AeroMap.Mode = new AerialMode();

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

                if(!firstLocationDataHasBeenSet)
                {
                    if(!(log.Latitud == 0 || log.Longitud == 0))
                    {
                        center_latitude = log.Latitud;
                        center_longitude = log.Longitud;
                        centerLocation.Latitude = center_latitude;
                        centerLocation.Longitude = center_longitude;
                        firstLocationDataHasBeenSet = true;
                        Console.WriteLine("Se ha establecido la ubicación central");
                    }
                }

                Console.WriteLine("Latitud del centro: " + centerLocation.Latitude);
                Console.WriteLine("Latitud del centro: " + centerLocation.Longitude);

                // Si se está grabando, se guardan los datos en logUAV
                if (recordingIsAvaible)
                {
                    if (!initialTimeHasBeenSet)
                    {
                        initialTimeWhenRecording = log.Tiempo;
                        initialTimeHasBeenSet = true;
                    }

                    // Se calcula el tiempo de acuerdo con lo enviado por Arduino
                    log.Tiempo = log.Tiempo - initialTimeWhenRecording;

                    //Se agrega el nuevo dato recibido al registro completo de datos
                    logUAV.Add(log);
                }

                // GPS
                aircraftLocation.Latitude = log.Latitud;
                aircraftLocation.Longitude = log.Longitud;

                Console.WriteLine("Latitude: " + aircraftLocation.Latitude);
                Console.WriteLine("Longitude: " + aircraftLocation.Longitude);

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
            //Label de prueba para ver distancia con respecto al objetivo

            if(targetLocationHasBeenSet)
            {
                lblDistanceToTarget.Content = "Distancia: " + log.getDistanceToTarget(targetLatitude, targetLongitude);
                drawLineFromAircraftToTarget();
            }

            if (firstLocationDataHasBeenSet)
            {
                AeroMap.Center = centerLocation;
                aircraft_pin.Location = aircraftLocation;
                jiggleMap();
                drawAircraftRoute();
            }

            if (recordingIsAvaible)
            {
                lblRecTime.Content = string.Format("{0:0.00}", log.Tiempo);
            }
        }

        private void jiggleMap()
        {
            double increment = 0.00000001;
            Location location;

            if (movingMapDirection == "Up")
            {
                location = new Location(center_latitude + increment, center_longitude);
                AeroMap.Center = location;
                movingMapDirection = "Down";
            }
            else if(movingMapDirection == "Down")
            {
                location = new Location(center_latitude - increment, center_longitude);
                AeroMap.Center = location;
                movingMapDirection = "Up";
            }
            else
            {
                Console.WriteLine("Incorrect value for movingMapDirection");
            }
        }

        private void drawAircraftRoute()
        {
            aircraftLocationsCollection.Add(new Location(aircraftLocation.Latitude, aircraftLocation.Longitude));

            aircraftRoute.Locations = aircraftLocationsCollection;
        }

        private void drawLineFromAircraftToTarget()
        {
            lineFromAircraftToTarget.Locations = new LocationCollection()
            {
                targetLocation,
                aircraftLocation
            };
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
                string puerto = CmbPorts.SelectedItem.ToString();
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

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            SearchPorts();
        }
        private void SearchPorts()
        {
            CmbPorts.Items.Clear();
            string[] puertos = SerialPort.GetPortNames();
            foreach (string puerto in puertos)
            {
                CmbPorts.Items.Add(puerto);
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
                tt_take_off.Visibility = Visibility.Collapsed;
                tt_Flight_Data.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
                tt_exit.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_take_off.Visibility = Visibility.Visible;
                tt_Flight_Data.Visibility = Visibility.Visible;
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

        private void LvTakeOff_Selected(object sender, RoutedEventArgs e)
        {
            PnTakeOff.Visibility = Visibility.Visible;
            PnFlightData.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
        }

        private void LvFlightData_Selected(object sender, RoutedEventArgs e)
        {
            PnFlightData.Visibility = Visibility.Visible;
            PnTakeOff.Visibility = Visibility.Hidden;
            PnSettings.Visibility = Visibility.Hidden;
        }


        private void LvSettings_Selected(object sender, RoutedEventArgs e)
        {
            PnSettings.Visibility = Visibility.Visible;
            PnFlightData.Visibility = Visibility.Hidden;
            PnTakeOff.Visibility = Visibility.Hidden;
        }

        private void LvSettings1_Selected(object sender, RoutedEventArgs e)
        {
            CerrarConexion();
            Close();
        }

        private void startRecording(object sender, RoutedEventArgs e)
        {
            if (ON)
            {
                recordingIsAvaible = true;
                btnRecord.Content = "Stop";
                ON = false;
            }
            else
            {
                btnRecord.Content = "Record";
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
                ON = true;
            }
            
        }

        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnConexion_Click(object sender, RoutedEventArgs e)
        {
            if (!toogle)
            {
                AbrirConexion();
                BtnConexion.Content = "Disconnect";
                txtConnection.Text = "Connected";
                MostrarLabel();
            }
            else
            {
                BtnConexion.Content = "Connect";
                txtConnection.Text = "Disconnected";
                CerrarConexion();
            }
            toogle = !toogle;
        }

        private void btnSetTarget_Click(object sender, RoutedEventArgs e)
        {
            targetLatitude_String = LatitudeTextBox.Text;
            targetLongitude_String = LongitudeTextBox.Text;
            bool targetLatitudeIsValid = double.TryParse(targetLatitude_String, out targetLatitude);
            bool targetLongitudeIsValid = double.TryParse(targetLongitude_String, out targetLongitude);
            bool targetLocationIsValid = targetLatitudeIsValid && targetLongitudeIsValid;

            if (targetLocationIsValid)
            {
                if (AeroMap.Children.Contains(targetPin))
                {
                    Console.WriteLine("== El mapa ya tiene un pin para el objetivo, que se procede a eliminar para colocar uno nuevo ==");
                    AeroMap.Children.Remove(targetPin);
                }

                targetLocation = new Location(targetLatitude, targetLongitude);
                targetPin = new Pushpin();
                targetPin.Location = targetLocation;
                AeroMap.Children.Add(targetPin);
                targetLocationHasBeenSet = true;

                MessageBox.Show("Target's location has been saved", "Location saved", MessageBoxButton.OK);
            }
            else
            {
                Console.WriteLine("Hubo un problema al establecer la ubicación del objetivo");

                MessageBox.Show("Target's location cannot be saved", "Unable to save", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
