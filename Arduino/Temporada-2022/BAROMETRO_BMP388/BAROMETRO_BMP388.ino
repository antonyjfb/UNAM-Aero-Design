//Código para funcionamiento del Barómetro BMP388
//Modificado el 18/11/2021 a las 7:17 pm
/*#include <Adafruit_BMP3XX.h>
#include <bmp3.h>
#include <bmp3_defs.h>*/

#include <Wire.h>
#include <SPI.h>
#include <Adafruit_Sensor.h>
#include "Adafruit_BMP3XX.h"

//Pines Arduino Nano SPI
#define BMP_SCK 13
#define BMP_MISO 12
#define BMP_MOSI 11
#define BMP_CS 10

//Pines Arduino Micro SPI
// #define BMP_SCK 9
// #define BMP_MISO 11
// #define BMP_MOSI 10
// #define BMP_CS 8

//Pines Arduino UNO I2C
//SDA 18
//SCL 19

//Pines Arduino Micro I2C
// SDA 19
// SCL 18

//Para I2C no es necesario definir los pines en el programa, sólo asegurarse de que se hayan conectado en los pines correspondientes

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
double dt;      // Diferencial de tiempo (dt = t1 - t0)
double dz;      // Distancia recorrida en eje Z (dz = z1 - z0)

void setup() 
{
  Serial.begin(9600);
  while (!Serial);
  Serial.println("Adafruit BMP388 / BMP390 test");
  //if (!bmp.begin_I2C()) { // Modo I2C
  if (!bmp.begin_SPI(BMP_CS, BMP_SCK, BMP_MISO, BMP_MOSI)) { // Modo SPI
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
}

void loop() {

  //Medición directa del barómetro
   if (! bmp.performReading()) { //Realiza una lectura completa de todos los sensores del BMP3XX. Asigna la temperatura interna Adafruit_BMP3XX # y la presión Adafruit_BMP3XX# variables miembro. @return True en caso de éxito, falso en caso de fracas
    Serial.println("No se pudo realizar la lectura");
    return;
  }
  Serial.print("Temperature = ");
  temperatura= ((bmp.temperature)* 9/5 )+ 32; //Pasar temperatura a Fahrenheit
  Serial.print(temperatura);
  Serial.println(" °F");

  Serial.print("Pressure = ");
  /*Serial.print(bmp.pressure);  
  Serial.println(" Pa");*/
  /*Serial.print(bmp.pressure / 3386.39); //(1 pulgada de mercurio (Hg) = 3386.39 Pascal).  
  Serial.println(" pulgadas-Hg");*/
  Serial.print(bmp.pressure / 100.0); //(1 hectopascal(hPa) = 100 Pascales).  
  Serial.println(" hPa");
  

  Serial.print("Altitude = ");
  altitud= bmp.readAltitude(presionInicial*0.01);
  Serial.print(altitud/0.3048); //(1 pie = 0.3048m)
  Serial.println(" ft");
  Serial.print(altitud);
  Serial.println(" m");

  //Medición de altitud con ecuación Hipsométrica
  // Se obtiene el cambio en altitud
    /*altitud_m = ecuacionHipsometrica(bmp.pressure);  // [m]
    altitud_ft = metrosAPies(altitud_m); // [ft]

    // Print results!
    Serial.print("Cambio en altitud (ft): ");
    Serial.println(altitud_ft, 6);*/

    //////// VALORES DE POSICIÓN FINAL para aceleración////
    t1 = millis();              //[s]
    z1= altitud/0.3048 ;        //[ft] 
    
    dt = t1 - t0;               //Diferencial de tiempo
    dz = z1 - z0;               //Diferencial de desplazamiento en eje Z
    speedZ = dz/dt;             //Velocidad en eje Z [ft/s]

    t0=millis();            //Actualizar valores iniciales
    z0=z1;
    Serial.println("Velocidad de ascenso: ");
    Serial.print(speedZ, 6);
    Serial.println(" [ft/s]");

    delay(1000);
}

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
