 // Programa del arduino Nano 33 BLE
// Avión Principal con cámara runcam
// Última modificación hecha: 17 de abril de 2021

#include <Adafruit_GPS.h>
#include <Servo.h>
#include <math.h>
#include <Arduino_LSM9DS1.h> //libreria para usar el IMU
#include <Arduino_LPS22HB.h> //librería para usar el barómetro

// Connect to the GPS on the hardware I2C port
Adafruit_GPS GPS(&Wire);  //Pin A4, Pin A5

//  / / / / / / / / S E R V O S  / / / / / / / / / / / /
Servo s1;    //Colonists release servo 1, SERVO DERECHO
Servo s2;    //Colonists release servo 2, SERVO IZQUIERDO
Servo s3;    //Payload release servo
Servo s4;    //Payload release servo
Servo sCam; //Servo para mover cámara fpv

//  / / / / V A R I A B L E S / / / / / /
    double P0;  // Presion atmosferica inicial debe estar en mb 

    //  Angulos iniciales de servos *Mecanismo cerrado de liberación*
    //  Estos valores se van a modificar eventualmente
    int posS1 = 1;  //
    int posS2 = 30;
    int posS3 = 180;
    int posS4 = 170;
    int angCam = 90;
    
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
    String alt;
    String roll;
    String pitch;
    String yaw;
    String SColonist = "0";
    String SPayload = "0";
    String datas;             // Cadena de texto final
    String coma = ",";        // Separador de datos

    long tiempo_prev, dt;
    float latitud=0, longitud=0;
    float angX, angY, angZ;
    float roll_prev = 0;
    float pitch_prev = 0;
    float yaw_prev = 0;
    float roll_inicial;
    float pitch_inicial;

void setup() {
    //Serial.begin(9600);
    Serial1.begin(9600);  //Serial para Xbee
  
    //  / / / / / /  C o m u n i c a c i o n    S e r i a l    G P S / / / / / / / /
    GPS.begin(0x10);  // The I2C address to use is 0x10
    GPS.sendCommand(PMTK_SET_NMEA_OUTPUT_RMCGGA);
    GPS.sendCommand(PMTK_SET_NMEA_UPDATE_1HZ); // 1 Hz update rate
    GPS.sendCommand(PGCMD_ANTENNA);
    delay(1000);
    GPS.println(PMTK_Q_RELEASE);
    
    // / /  Á n g u l o s     i n i c i a l e s     d e     S e r v o s / / / /
    //  Definir los pines en que se encuentran los servos y sus posiciones iniciales
    s1.attach(2);    //Colonist release servo in pin 2
    s1.write(1);    //posS1
    s2.attach(4);
    s2.write(30);   
    s3.attach(6);   //Payload release servo in pin 4
    s3.write(180);   //posS3
    s4.attach(7);
    s4.write(170);
    sCam.attach(8);
    sCam.write(angCam);
    

    if ( !IMU.begin())  // Se verifica que el acelerometro este funcionando
     {
         while (1) 
         {
           delay(1000);
         }
      }

      if ( !BARO.begin()) //Se verifica que el barometro esté funcionando
     {
         while (1) 
         {
           delay(1000);
         }
      }

      // / / / T i e m p o   d e  a r r a n q u e / / / / / /
      tiempo_prev = millis();
      delay(100);
      
      // / / / / / P r e s i ó n   i n i c i a l / / / / / / /
      int contador=0;
      float g1, g2, g3, a1, a2, a3;
      while(contador<10){
        P0=BARO.readPressure(MILLIBAR);
        IMU.readGyroscope(g1, g2, g3);  
        IMU.readAcceleration(a1, a2, a3); 
        dt = millis()-tiempo_prev;
        tiempo_prev=millis();
        float accelX = atan(a2/sqrt(pow(a1,2) + pow(a3,2)))*(180.0/3.14);
        float accelY = atan(-a1/sqrt(pow(a2,2) + pow(a3,2)))*(180.0/3.14);
        roll_prev = 0.85*((g1)*dt/1000.0 + roll_prev) + 0.15*accelX ;   //alabeo
        pitch_prev = 0.85*((g2)*dt/1000.0 + pitch_prev) + 0.15*accelY ;  //cabeceo
        contador++;
        delay(100);
      }
      
}

void loop() {
  datas = "";

  serialEvent(); //Evaluar liberaciones

  ///////////////////////   C O O R D E N A D A S     G P S        /////////////////////////
      char c=GPS.read();

      if (GPS.newNMEAreceived()) {
    
        if (!GPS.parse(GPS.lastNMEA())) // this also sets the newNMEAreceived() flag to false
            
          return; // we can fail to parse a sentence in which case we should just wait for another
      }
      
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
      //angZ= (gZ)*dt/1000.0 + yaw_prev;                                //guiñada 
      //Filtro solo puede utilizarse para ángulos en X y Y
      
      roll = String( angX, 3);
      pitch = String( angY,3);
      //yaw = String( angZ, 3);

      roll_prev = angX;
      pitch_prev = angY;
      //yaw_prev = angZ;
      
    }
     else
    {
      roll = "-123";
      pitch = "-123";
      //yaw = "-123";
    }
    
    ////////  B A R Ó M E T R O ////////
    alt= get_Altitude(P0);  
    
    ////// D A T A S
    datas = alt + coma;
    datas = datas + s + coma;
    //datas = datas + "00.000" + coma;
    datas = datas + lt + coma;
    //datas = datas + "-00.0000000" + coma;
    datas = datas + ln + coma;
    //datas = datas + "-00.0000000" + coma;
    datas = datas + roll + coma;
    datas = datas + pitch + coma;
    datas = datas + "0.00" + coma;
    datas = datas + distanceft + coma;
    //datas = datas + "00.000" + coma;
    datas = datas + SColonist + coma;
    datas = datas + SPayload + coma + SPayload;

    Serial1.println(datas);
    delay(200);
}


////////////  F U N C I Ó N     P A R A     S E R V O S /////////////
//  Los valores se actualizarán eventualmente *Mecanismo abierto de liberación*
void serialEvent(){
  
  switch (Serial1.read()) 
      { 
        case 'a': //Liberacion de planeadores
          if (posS1 == 1) //Ángulo de MECANISMO CERRADO (Inicial)
            {
              posS1 = 160;  //Ángulos de MECANISMO ABIERTO
              posS2 = 120;
            } 
          else 
            {
              posS1 = 1; //MECANISMO CERRADO
              posS2 = 30;
            }         
          s1.write(posS1);
          s2.write(posS2);
          SColonist = "1";
          delay(100); 
          break;
        case 'b': //Liberacion de carga
          if (posS3 == 180)   //Ángulo de MECANISMO CERRADO
            {
              posS3 = 20;     //Ángulos de MECANISMO ABIERTO
              posS4 = 20;
            } 
          else  
            {
              posS3 = 180;    //MECANISMO CERRADO
              posS4 = 170;
            }
          s3.write(posS3);
          s4.write(posS4);
          SPayload = "1";
          delay(100);
          break;
        case 'u': //Movimiento arriba
          if (angCam <= 170 && angCam >= 0){
            angCam = angCam + 10;
            sCam.write(angCam);  
          }
          delay(100);
          break;
        case 'd': //Movimiento abajo
          if (angCam >= 10 && angCam <= 180){
            angCam = angCam - 10;
            sCam.write(angCam);  
          }
          delay(100);
          break;
      }
}

String get_Altitude(double P_Inicial)  /// NUEVA FUNCIÓN PARA OBTENER LA ALTURA DEL BARÓMETRO
  {
    String altitud;
    double P = BARO.readPressure(MILLIBAR);
    altitud= String (( 44330.0 * (1.0 - pow (P/P_Inicial , 1.0 / 5.255 ) ) )* 3.28084, 3 );
    return altitud; //regresa en ft
  }

double Distancia(double latitudAvion, double longitudAvion, double latObj, double lonObj){
      double dlat, dlong, distancia;
      
      dlat=(latObj-latitudAvion);
      dlong=(lonObj-longitudAvion);
      distancia=(acos(sin(latitudAvion)*sin(latObj)+cos(latitudAvion)*cos(latObj)*cos(lonObj-longitudAvion)))*111180;
      return distancia;
}
