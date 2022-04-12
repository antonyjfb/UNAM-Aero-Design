#include <Servo.h> //libreria para usar servos
#include <Adafruit_GPS.h>
#include <Arduino_LSM9DS1.h> //libreria para usar el IMU
//Código para funcionamiento del Barómetro BMP388
#include <Wire.h>
#include <SPI.h>
#include <Adafruit_Sensor.h>
#include "Adafruit_BMP3XX.h"

//Pines Arduino Nano SPI
#define BMP_SCK 13
#define BMP_MISO 12
#define BMP_MOSI 11
#define BMP_CS 10

Adafruit_BMP3XX bmp;

#define SEALEVELPRESSURE_HPA (1013.25)

float temperatura;
float altitud;
void determinarPresionInicial(uint8_t n);
float ecuacionHipsometrica(float presionActual);
float metrosAPies(float metros);
float temperaturaVirtual = 297.89f;  // [K]
float altitud_m;  // [m]
float altitud_ft; // [ft]
float presionInicial;  // [Pa]

//Variables para la velocidad de ascenso
double z0;      // Posición inicial en el instante inicial t0
double z1;      // Posición final en el instante final t1
double t0;      // Tiempo inicial
double t1;      // Tiempo final 
double speedZ;  // Velocidad en eje Z
double vdt;      // Diferencial de tiempo (dt = t1 - t0)
double dz;      // Distancia recorrida en eje Z (dz = z1 - z0)

//Variables para giroscopio
float angX, angY, angZ;
float roll_prev = 0;
float pitch_prev = 0;
long tiempo_prev, dt;
float roll;
float pitch;

// Connect to the GPS on the hardware I2C port
Adafruit_GPS GPS(&Wire);  //Pin A4, Pin A5
// Datos a enviar del principal a la estación en tierra
    String lt;
    String ln;
    String s;
    float latitud=0, longitud=0;
    
    float timer = 0;
    bool liberacion = 0;
//  / / / / / / / / S E R V O S  / / / / / / / / / / / /
Servo servo1;

int PINSERVO1 = 2;
int PULSOMIN1 = 650;
int PULSOMAX1 = 2600;

void setup() 
{
  ////////////// BARÓMETRO ////////////////
  Serial.begin(9600);
  while (!Serial);
  Serial.println("Adafruit BMP388 / BMP390 test");
  if (!bmp.begin_SPI(BMP_CS, BMP_SCK, BMP_MISO, BMP_MOSI)) 
  { // Modo SPI
    Serial.println("Could not find a valid BMP3 sensor, check wiring!");
    Serial.println("0,0,0,0,BMP_ERROR, BMP_ERROR, 0,0,0,0");  //Poder leer por la interfaz
    while (1000);
  }

  //Sobremuestreo e inicialización del filtro
  bmp.setTemperatureOversampling(BMP3_OVERSAMPLING_8X);
  bmp.setPressureOversampling(BMP3_OVERSAMPLING_4X);
  bmp.setIIRFilterCoeff(BMP3_IIR_FILTER_COEFF_3);
  bmp.setOutputDataRate(BMP3_ODR_50_HZ);

  // Determinar presión inicial (al nivel del suelo)
    determinarPresionInicial(15);
  //Valores de posición inicial
    t0=millis();
    z0=bmp.readAltitude(presionInicial)/0.3048;  //Altitud en ft

  ///////////////////C o m u n i c a c i o n  S e r i a l   G P S ////////////////
    GPS.begin(0x10);  // The I2C address to use is 0x10
    GPS.sendCommand(PMTK_SET_NMEA_OUTPUT_RMCGGA);
    GPS.sendCommand(PMTK_SET_NMEA_UPDATE_1HZ); // 1 Hz update rate
    GPS.sendCommand(PGCMD_ANTENNA);
    delay(1000);
    GPS.println(PMTK_Q_RELEASE);

  //////// A C E L E R Ó M E T R O  Y G I R Ó S C O P I O//////////
  // Se verifica que el acelerometro este funcionando
  if (!IMU.begin())
  {
    Serial.println("Failed to initialize IMU!");
    while (1);
  }
  // / /  Á n g u l o s     i n i c i a l e s     d e     S e r v o s / / / /
  //  Definir los pines en que se encuentran los servos y sus posiciones iniciales
  servo1.attach(PINSERVO1,PULSOMIN1,PULSOMAX1);    
}

void loop() 
{
    servo1.write(180);
    delay(5000);
    servo1.write(0);
    timer = millis()/1000 ;
    readBarometer();
    readGPS();
    readGyrosc();
    showData();     
}

void readGPS()
{
    char c=GPS.read();
    if (GPS.newNMEAreceived()) 
    {
      if (!GPS.parse(GPS.lastNMEA())) // this also sets the newNMEAreceived() flag to false
      return; // we can fail to parse a sentence in which case we should just wait for another
    }
    if(GPS.fix)
    {
      latitud=(GPS.latitude)/100;
      longitud=(GPS.longitude)/100;
      lt=String(float(int(latitud))+((latitud-float(int(latitud)))*100/60), 10);
      ln="-"+String((float(int(longitud))+((longitud-float(int(longitud)))*100/60)), 10);
      s=String(GPS.speed*0.514444,3);
    }
}

void readBarometer()
{
    //Medición directa del barómetro
  //Realiza una lectura completa de todos los sensores del BMP3XX. Asigna la temperatura interna
  //Adafruit_BMP3XX # y la presión Adafruit_BMP3XX# variables miembro. @return True en caso de éxito
  //,falso en caso de fracas
   if (! bmp.performReading()) 
   {  
    Serial.println("No se pudo realizar la lectura");
    return;
   }
  temperatura= ((bmp.temperature)* 9/5 )+ 32; //Pasar temperatura a Fahrenheit
  altitud= bmp.readAltitude(presionInicial*0.01);


    //////// VALORES DE POSICIÓN FINAL para aceleración////
    t1 = millis();              //[s]
    z1= altitud/0.3048 ;        //[ft] 
    
    vdt = t1 - t0;               //Diferencial de tiempo
    dz = z1 - z0;               //Diferencial de desplazamiento en eje Z
    speedZ = dz/vdt;             //Velocidad en eje Z [ft/s]

    t0=millis();            //Actualizar valores iniciales
    z0=z1;
  
}


void readGyrosc()
{
      ////////// Á N G U L O S     D E     R O T A C I Ó N ////////
    
    float gX, gY, gZ;  //Variables que guardará el giroscopio
    float aX, aY, aZ;  //Variables que guardará el acelerómetro

    if (IMU.gyroscopeAvailable() && IMU.accelerationAvailable()) 
    {
      IMU.readGyroscope(gX, gY, gZ);     //leer las velocidades angulares
      IMU.readAcceleration(aX, aY, aZ);  //leer las aceleraciones
           
      dt = millis()-tiempo_prev;
      tiempo_prev=millis();
      
      //Calcular los ángulos X y Y con acelerometro
      float accel_ang_x = atan(aY/sqrt(pow(aX,2) + pow(aZ,2)))*(180.0/3.14);
      float accel_ang_y = atan(-aX/sqrt(pow(aY,2) + pow(aZ,2)))*(180.0/3.14);
      
      //Calcular angulo de rotación con giroscopio y filtro complemento  
      //Coeficientes alfa y beta pueden variar pero siempre sumar 1.0
      angX = 0.85*((gX)*dt/1000.0 + roll_prev) + 0.15*accel_ang_x ;   //alabeo
      angY = 0.85*((gY)*dt/1000.0 + pitch_prev) + 0.15*accel_ang_y ;  //cabeceo
      //angZ= (gZ)*dt/1000.0 + yaw_prev;                              //guiñada 
      //Filtro solo puede utilizarse para ángulos en X y Y
      
      roll = angX;
      pitch = angY;

      roll_prev = angX;
      pitch_prev = angY;

    } 
}

void showData()
{
    Serial.print(timer); 
    Serial.print(",");
    Serial.print(ln);   
    Serial.print(",");
    Serial.print(lt);
    Serial.print(",");
    Serial.print(altitud);              //Altitude [m]
    Serial.print(",");
    Serial.print(roll);
    Serial.print(",");
    Serial.print(pitch);
    Serial.print(",");
    Serial.print(s);
    Serial.print(",");
    Serial.print(speedZ, 6);          // Velocidad de ascenso [ft/s]
    Serial.print(",");
    Serial.println(liberacion);
    delay(1000); 

  //  Serial.print(bmp.pressure / 100.0); // Pressure [hPa] (1 hectopascal(hPa) = 100 Pascales).
  //    Serial.print(altitud/0.3048);   // Altitude [ft](1 pie = 0.3048m)
  
}

void determinarPresionInicial(uint8_t n) 
{
    uint8_t i;
    // Se realizan algunas lecturas previas
    for (i = 0; i < 3; i++) {
        if (bmp.performReading() == false) {
            Serial.println("El sensor falla.");
            return;
        }
        delay(100);
    }
    // Se realizan n lecturas
    Serial.println("Obteniendo presión al nivel del suelo (...)");
    presionInicial = 0.0f;
    for (i = 0; i < n; i++) {
        if (bmp.performReading() == false) {
            Serial.println("El sensor falla.");
            return;
        }
        if(i>5){
          presionInicial += bmp.pressure;  // [Pa]
        }
        delay(330);
    }
    // Se promedian las lecturas
    presionInicial /= (float)(n-6);  // [Pa]
}

float ecuacionHipsometrica(float presionActual) 
{
    float R = 287.0f;  // Gas const. [J / kg.K]
    float g = 9.81f;  // Grav. accel. [m/s/s]
    return ((R * temperaturaVirtual) / g) * log(presionInicial / presionActual);  // [m]
}

float metrosAPies(float metros) 
{
    float factorDeConversion = 3.28084f;
    float pies = metros * factorDeConversion;
    return pies;
}
