
#include <Adafruit_GPS.h>

// Connect to the GPS on the hardware I2C port
Adafruit_GPS GPS(&Wire);  //Pin A4, Pin A5

uint32_t timer = millis();

//  Exclusivamente para distancia entre el objetivo
    //  Estos valores se van a modificar eventualmente *Latitud, Longitud*
    const float DESTINATION_LAT = 19.507148;
    const float DESTINATION_LNG = -99.3174798;
    float distancia, radio, latActual, longActual, a, dlat, dlong, latDestino, longDestino, latDestinoDEC, longDestinoDEC;
    float R=6378; //Radio de la tierra en Km
    const float pi=3.141592653589793238462643383279502884197169399375105820974944;

// Datos a enviar del principal a la estación en tierra
    String lt;
    String ln;
    String s;
    String distanceft;
    String datas;             // Cadena de texto final
    String coma = ",";        // Separador de datos

float latitud=0, longitud=0;

void setup() {
  Serial.begin(9600);

  //  / / / / / /  C o m u n i c a c i o n    S e r i a l    G P S / / / / / / / /
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
    datas = s + coma;
    datas = datas + lt + coma;
    datas = datas + ln + coma;
    datas = datas + distanceft + coma;
    Serial.println(datas);
  }
  
    
    //delay(2000);
}
