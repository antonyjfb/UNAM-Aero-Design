#include <Adafruit_Sensor.h>
#include "MPU9250.h"
#include <Adafruit_BMP280.h>
#include <Servo.h>
#include <Wire.h>
#include <TinyGPS++.h>

///////////////////////////////////Objetos//////////////////////////////////

Adafruit_BMP280 bmp;    //bmp object
MPU9250 mpu;
TinyGPSPlus gps;        //gps object

/////////////////////////////////Variables/////////////////////////////////

float P0; //Atmospheric pressure
String lt;//latitud
String ln;//longitud
float alt;
float yaw;
float pitch;
float roll;
/////////////////////////////////////SETUP/////////////////////////////////////

void setup() {
  Serial.begin(9600);
  Serial1.begin(9600);       //Serial will be use by xBee communication
  Serial2.begin(9600);         //Serial will be use by GPS communication
  Wire.begin();
////////////////  Calibración de MPU /////////////
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
//////////////////////Calibración de BMP//////////////////////////////////////////

Serial.println("Calibrating BMP");

  if ( !bmp.begin() )
  {
    while (1) {
      Serial.println("0,0,0,BMP_Error,0,0,0,0,0,0,0,0"); //Serial1
      delay(1000);
    }
  }
  Serial.println("Done!");
  P0 = bmp.readPressure() / 100;
}

//////////////////////////////////////LOOP/////////////////////////////////////

void loop() {
  //GPS
  if (gps.location.isValid() && gps.altitude.isValid())
  {
    lt = String(gps.location.lat(), 8);
    ln = String(gps.location.lng(), 8);
  }
 else {
    lt = "*****";
    ln = "*****";
 }
  smartDelay(200);
  //BPM
  alt = bmp.readAltitude(P0);


  //DATA
 Serial.print("\x02");
 Serial.print(lt); 
 Serial.print(",");
 Serial.print(ln); 
 Serial.print(",");
 Serial.print("0"); //velocidad
 Serial.print(",");
 Serial.print(alt);
 Serial.print(",");
 Serial.print(roll);
 Serial.print(",");
 Serial.print(pitch);
 Serial.print(","); 
 Serial.print(yaw);
  Serial.print(",");
 Serial.print("0"); //distancia
 Serial.print(",");
 Serial.print("0"); //tiempo
 Serial.print("\x03");
 delay(100);// delay in between reads for stability

}

//////////////////////////////////Funciones//////////////////////////////////////

static void smartDelay(unsigned long ms)
{
  unsigned long start = millis();
  do
  {
    while (Serial2.available())
      gps.encode(Serial2.read());
    ////// M P U /////////////////////////////////
    if (mpu.update()) {
        static uint32_t prev_ms = millis();
        if (millis() > prev_ms + 25) {
            roll_pitch_yaw();
            prev_ms = millis();
        }
    }
  } while (millis() - start < ms);
}


void roll_pitch_yaw() {
  yaw =  mpu.getYaw();
  pitch = mpu.getPitch();
  roll = mpu.getRoll();
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
