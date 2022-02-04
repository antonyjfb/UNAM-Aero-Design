#include <Adafruit_Sensor.h>
#include "MPU9250.h"
#include <Adafruit_BMP280.h>
#include <Servo.h>
#include <Wire.h>
#include <TinyGPS++.h>

///////////////////////////////////Objetos//////////////////////////////////

Adafruit_BMP280 bmp;    //bmp object
MPU9250 mpu;            //mpu9250
TinyGPSPlus gps;        //gps object
Servo s1;    //Colonists release servo 1
Servo s2;    //Colonists release servo 2
Servo s3;    //Colonists release servo 3
Servo s4;    //Payload release servo

/////////////////////////////////Variables/////////////////////////////////

float P0; //Atmospheric pressure
//int status;

int posS1 = 90;
int posS2 = 90;
int posS3 = 90;
int posS4 = 110;
String lt;//latitud
String ln;//longitud
String s;//
String alt;//altuta
String SColonist = "0";
String SPayload = "0";
String data;
String coma = ",";

/////////////////////////////////////SETUP/////////////////////////////////////

void setup() {
  Serial.begin(9600);
  Wire.begin();
  Serial1.begin(9600);       //Serial will be use by xBee communication
  Serial2.begin(9600);         //Serial will be use by GPS communication
  Serial.println("Setting up");
  s1.attach(2);    //Colonist release servos in pin 2
  s1.write(90);
  s2.attach(3);
  s2.write(90);
  s3.attach(4);
  s3.write(90);
  s4.attach(5);
  s4.write(110);


//// B A R O M E T R O /////

Serial.println("Calibrating BMP");

  if ( !bmp.begin() )
  {
    while (1) {
      Serial.println("0,0,0,BMP_Error,0,0,0,0,0,0,0,0"); //Serial1
      delay(1000);
    }
  }
Serial.println("Done!");
  //while(!Serial) {} //Serial1

  P0 = bmp.readPressure() / 100;


/// GIROSCOPIO ///

if (!mpu.setup(0x68)) {  // change to your own address
        while (1) {
            Serial.println("MPU connection failed. Please check your connection with `connection_check` example.");
            delay(5000);
        }
    }

    // calibrate anytime you want to
    Serial.println("Accel Gyro calibration will start in 5sec.");
    Serial.println("Please leave the device still on the flat plane.");
    mpu.verbose(true);
    delay(5000);
    mpu.calibrateAccelGyro();

    Serial.println("Mag calibration will start in 5sec.");
    Serial.println("Please Wave device in a figure eight until done.");
    delay(5000);
    mpu.calibrateMag();

    print_calibration();
    mpu.verbose(false);  
}

//////////////////////////////////////LOOP/////////////////////////////////////

void loop() {
  data = "";
  //GPS
  if (gps.location.isValid() && gps.altitude.isValid() && gps.speed.isValid())
  {
    lt = String(gps.location.lat(), 8);
    ln = String(gps.location.lng(), 8);

    s = String(gps.speed.mps(), 2);
  }
  else {
    lt = "*****";
    ln = "*****";
    s =  "*****";
  }

  //BPM
  alt = bmp.readAltitude(P0);

  //GIROSCOPIO
      if (mpu.update()) {
        static uint32_t prev_ms = millis();
        if (millis() > prev_ms + 25) {
            prev_ms = millis();
        }
    }
  //DATA

  data = lt + coma;
  data = data + ln + coma;
  data = data + s + coma;
  data = data + alt + coma;
  data = data + SColonist + coma;
  data = data + SPayload + coma;
  data = data + mpu.getYaw() + coma;
  data = data + mpu.getPitch() + coma;
  data = data + mpu.getRoll() + coma;
  data = data + "0";
  
  //latitud,longitud,velocidad,altura,BoolPlaneadores,BoolPayload,0
  Serial.println(data); //Serial1
  smartDelay(200);

}


///////////////////////////////////////////////////////
void serialEvent() {
  switch (Serial.read()) { //Serial1
    case 'a':
      if (posS1 == 90) {
        posS1 = 120;
        posS2 = 60;
        posS3 = 60;
      } else {
        posS1 = 90;
        posS2 = 90;
        posS3 = 90;
      }
      s1.write(posS1);
      s3.write(posS2);
      s2.write(posS3);
      SColonist = "1";
      smartDelay(100);
      break;
    case 'b':
      if (posS4 == 110) {
        posS4 = 40;
      } else {
        posS4 = 110;
      }
      s4.write(posS4);
      SPayload = "1";
      smartDelay(100);
      break;
  }
}



//////////////////////////////////Funciones//////////////////////////////////////

static void smartDelay(unsigned long ms)
{
  unsigned long start = millis();
  do
  {
    while (Serial2.available())
      gps.encode(Serial2.read());
  } while (millis() - start < ms);
}

void print_calibration() {
    Serial.println("< calibration parameters >");
    Serial.println("accel bias [g]: ");
    Serial.print(mpu.getAccBiasX() * 1000.f / (float)MPU9250::CALIB_ACCEL_SENSITIVITY);
    Serial.print(", ");
    Serial.print(mpu.getAccBiasY() * 1000.f / (float)MPU9250::CALIB_ACCEL_SENSITIVITY);
    Serial.print(", ");
    Serial.print(mpu.getAccBiasZ() * 1000.f / (float)MPU9250::CALIB_ACCEL_SENSITIVITY);
    Serial.println();
    Serial.println("gyro bias [deg/s]: ");
    Serial.print(mpu.getGyroBiasX() / (float)MPU9250::CALIB_GYRO_SENSITIVITY);
    Serial.print(", ");
    Serial.print(mpu.getGyroBiasY() / (float)MPU9250::CALIB_GYRO_SENSITIVITY);
    Serial.print(", ");
    Serial.print(mpu.getGyroBiasZ() / (float)MPU9250::CALIB_GYRO_SENSITIVITY);
    Serial.println();
    Serial.println("mag bias [mG]: ");
    Serial.print(mpu.getMagBiasX());
    Serial.print(", ");
    Serial.print(mpu.getMagBiasY());
    Serial.print(", ");
    Serial.print(mpu.getMagBiasZ());
    Serial.println();
    Serial.println("mag scale []: ");
    Serial.print(mpu.getMagScaleX());
    Serial.print(", ");
    Serial.print(mpu.getMagScaleY());
    Serial.print(", ");
    Serial.print(mpu.getMagScaleZ());
    Serial.println();
}
