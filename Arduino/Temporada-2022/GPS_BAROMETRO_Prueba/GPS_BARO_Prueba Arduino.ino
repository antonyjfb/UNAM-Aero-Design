// ------ BIBLIOTECAS --------
#include <Adafruit_GPS.h>
#include <Wire.h>
#include <SPI.h>
#include <Adafruit_Sensor.h>
#include "Adafruit_BMP3XX.h"

// ---------- Pines Arduino Nano SPI BARÓMETRO -----------
#define BMP_SCK 13
#define BMP_MISO 12
#define BMP_MOSI 11
#define BMP_CS 10

// --------- Conectar el GPS en el puerto I2C ---------
Adafruit_GPS GPS(&Wire);  //Pin A4, Pin A5
uint32_t timer = millis();

// ---------- Exclusivamente para altura con presión atmosférica ---------
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

// --------- Exclusivamente para distancia entre el objetivo ------------
//  Estos valores se van a modificar eventualmente *Latitud, Longitud*
const float DESTINATION_LAT = 19.507148;
const float DESTINATION_LNG = -99.3174798;
float distancia, radio, latActual, longActual, a, dlat, dlong, latDestino, longDestino, latDestinoDEC, longDestinoDEC;
float R=6378; //Radio de la tierra en Km
const float pi=3.141592653589793238462643383279502884197169399375105820974944;

// ---------- Datos a enviar del principal a la estación en tierra ----------
String alt;
String lt;
String ln;
String s;
String distanceft;
String velZ;
String datas;             // Cadena de texto final
String coma = ",";        // Separador de datos

// ------------- Variables iniciales -------------
float latitud=0, longitud=0;
// ----------- Variables para la velocidad de ascenso ---------
double z0;      // Posición inicial en el instante inicial t0
double z1;      // Posición final en el instante final t1
double t0;      // Tiempo inicial
double t1;      // Tiempo final 
double speedZ;  // Velocidad en eje Z
double dt;      // Diferencial de tiempo (dt = t1 - t0)
double dz;      // Distancia recorrida en eje Z (dz = z1 - z0)

void setup() {
  Serial.begin(9600);

  // ---------------- Comunicación Serial Barómetro -------------------
  while (!Serial);
  Serial.println("Adafruit BMP388 / BMP390 test");
  if (!bmp.begin_SPI(BMP_CS, BMP_SCK, BMP_MISO, BMP_MOSI)) { // Modo SPI
    Serial.println("Could not find a valid BMP3 sensor, check wiring!");
    Serial.println("0,0,0,0,BMP_ERROR, BMP_ERROR, 0,0,0,0");  //Poder leer por la interfaz
  while (1000);
  }

  // ---------------- Sobremuestreo e inicialización del filtro ------------
  bmp.setTemperatureOversampling(BMP3_OVERSAMPLING_8X);
  bmp.setPressureOversampling(BMP3_OVERSAMPLING_4X);
  bmp.setIIRFilterCoeff(BMP3_IIR_FILTER_COEFF_3);
  bmp.setOutputDataRate(BMP3_ODR_50_HZ);

  // ------------ Determinar presión inicial (al nivel del suelo) ----------
    determinarPresionInicial(15);
  // ------------ Valores de posición inicial ---------------
    t0=millis();
    z0=bmp.readAltitude(presionInicial)/0.3048;  //Altitud en ft

  // ----------------- Comunicación Serial GPS ---------------------
    GPS.begin(0x10);  // The I2C address to use is 0x10
    GPS.sendCommand(PMTK_SET_NMEA_OUTPUT_RMCGGA);
    GPS.sendCommand(PMTK_SET_NMEA_UPDATE_1HZ); // 1 Hz update rate
    GPS.sendCommand(PGCMD_ANTENNA);
    delay(1000);
    GPS.println(PMTK_Q_RELEASE);

}

void loop() {
  char c=GPS.read();

      if (GPS.newNMEAreceived()) {
    
        if (!GPS.parse(GPS.lastNMEA())) // this also sets the newNMEAreceived() flag to false
            
          return; // we can fail to parse a sentence in which case we should just wait for another
      }

  // approximately every 2 seconds or so, print out the current stats
  if (millis() - timer > 2000) {
    timer = millis(); // reset the timer
    if(GPS.fix){
      latitud=(GPS.latitude)/100;
      longitud=(GPS.longitude)/100;

      lt=String(float(int(latitud))+((latitud-float(int(latitud)))*100/60), 10);
      ln="-"+String((float(int(longitud))+((longitud-float(int(longitud)))*100/60)), 10);
      s=String(GPS.speed*0.514444,3);

      latActual=(float(int(latitud))+((latitud-float(int(latitud)))*100/60))*(pi/180);
      longActual=(float(int(longitud))+((longitud-float(int(longitud)))*100/60))*(- 1)*(pi/180);
      latDestino=DESTINATION_LAT*(pi/180);
      longDestino=DESTINATION_LNG*(pi/180);

      dlat=(latDestino-latActual);
      dlong=(longDestino-longActual);

      a=(pow(sin(dlat/2),2))+(cos(latActual)*cos(latDestino))*(pow(sin(dlong/2),2));
      distancia= (2*R*asin(sqrt(a)))*1000; //Distancia en metros
      distanceft=String((2*R*asin(sqrt(a)))*1000*3.28,3); //Distancia en ft
    }

    //Medición directa del barómetro
   if (! bmp.performReading()) { //Realiza una lectura completa de todos los sensores del BMP3XX. Asigna la temperatura interna Adafruit_BMP3XX # y la presión Adafruit_BMP3XX# variables miembro. @return True en caso de éxito, falso en caso de fracas
    Serial.println("No se pudo realizar la lectura");
    return;
  }
  temperatura= ((bmp.temperature)* 9/5 )+ 32; 

  altitud= bmp.readAltitude(presionInicial*0.01)/0.3048;

  //////// VALORES DE POSICIÓN FINAL para aceleración////
    t1 = millis();              //[s]
    z1= altitud;                //[ft] 
    
    dt = t1 - t0;               //Diferencial de tiempo
    dz = z1 - z0;               //Diferencial de desplazamiento en eje Z
    speedZ = dz/dt;             //Velocidad en eje Z [ft/s]

    t0=millis();            //Actualizar valores iniciales
    z0=z1;

    alt=String(altitud,3);
    velZ=String(speedZ,6);

    datas = alt + coma;
    datas = s + coma;
    datas = velZ + coma;
    datas = datas + lt + coma;
    datas = datas + ln + coma;
    datas = datas + distanceft + coma;
    Serial.println(datas);

    //altitud,speed,velZ,lt,ln,distanceft,
  }
  
}

//------------ Función para determinar la presión inicial del avión en tierra -----------------------------
void determinarPresionInicial(uint8_t n) {
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

float ecuacionHipsometrica(float presionActual) {
    float R = 287.0f;  // Gas const. [J / kg.K]
    float g = 9.81f;  // Grav. accel. [m/s/s]
    return ((R * temperaturaVirtual) / g) * log(presionInicial / presionActual);  // [m]
}

float metrosAPies(float metros) {
    float factorDeConversion = 3.28084f;
    float pies = metros * factorDeConversion;
    return pies;
}
