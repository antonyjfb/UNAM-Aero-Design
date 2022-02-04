#include <Adafruit_Sensor.h>

#include <Adafruit_BMP280.h>
#include <Servo.h>
#include <Wire.h>
#include <TinyGPS++.h>

///////////////////////////////////Objetos//////////////////////////////////

Adafruit_BMP280 bmp;    //bmp object
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

  //DATA

  data = lt + coma;
  data = data + ln + coma;
  data = data + s + coma;
  data = data + alt + coma;
  data = data + SColonist + coma;
  data = data + SPayload + coma;
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
