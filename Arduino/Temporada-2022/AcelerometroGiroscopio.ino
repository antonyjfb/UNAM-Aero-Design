 // Programa del arduino Nano 33 BLE
 
#include <Arduino_LSM9DS1.h> //libreria para usar el IMU

//  / / / / V A R I A B L E S / / / / / /
float aX, aY, aZ;
float roll,pitch,yaw;

void setup() {
  Serial.begin(9600);
  while (!Serial);


//////// A C E L E R Ó M E T R O  Y G I R Ó S C O P I O//////////

// Se verifica que el acelerometro este funcionando
  if (!IMU.begin()) {
    Serial.println("Failed to initialize IMU!");
    while (1);
  }
  Serial.print("Accelerometer and Gyroscope sample rate = ");
  Serial.print(IMU.accelerationSampleRate());
  Serial.println(" Hz");
  Serial.println();
}

void loop() {
  if (IMU.accelerationAvailable() && IMU.gyroscopeAvailable()) {
    IMU.readAcceleration(aX, aY, aZ);     //leer las aceleraciones
    IMU.readGyroscope(roll,pitch,yaw); //leer las velocidades angulares

    Serial.print("\x02");
    // acelerómetro//
    Serial.print(aX);
    Serial.print(",");
    Serial.print(aY);
    Serial.print(",");
    Serial.println(aZ);
    Serial.print(",");
    //Giróscipio//
    Serial.print(roll);
    Serial.print(",");
    Serial.print(pitch);
    Serial.print(",");
    Serial.println(yaw);
    
    Serial.print("\x03");

    delay(100);// delay in between reads for stability
  }
}
