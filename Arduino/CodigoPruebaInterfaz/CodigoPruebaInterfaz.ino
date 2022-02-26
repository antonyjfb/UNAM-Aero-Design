int x = 0;
float y = 0.000001;
float lat = 19.424184;
float longi = -99.134937;

// the setup routine runs once when you press reset:
void setup() {
  // initialize  serial communication at 9600 bits per second:
     Serial.begin(9600);
}

// the loop routine runs over and over again forever:
void loop() {
  // read the input on analog pin 0:
  if(Serial.available()>0)
  {
    if(Serial.readString() == "1#")
    {
      bool flujo = true;
      while (flujo)
      {
//        int dato = analogRead(A0);
//        Serial.print("\x02");
//        Serial.print(dato);
//        Serial.print("\x03");
        Serial.print("\x02");
        Serial.print(millis()); // [0]
        Serial.print(",");
        Serial.print(sin(x*(3.14/180))); // [1]
        Serial.print(",");
        Serial.print(lat,6); // [2]
        Serial.print(",");
        Serial.print(longi,6); // [3]
        Serial.print(",");
        Serial.print(4*sin(x*(3.14/180))); // [4]
        Serial.print(",");
        Serial.print(5*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(6*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(7*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(8*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(9*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(10*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(11*sin(x*(3.14/180)));
        Serial.print(",");
        Serial.print(12*sin(x*(3.14/180)));
        Serial.print("\x03");
        x++;
        lat = lat + y;
        longi = longi + y;
        if(Serial.available()>0)
          if(Serial.readString() == "0#")
            flujo = false;
        delay(10);
      }
    }
  }
  delay(100);// delay in between reads for stability
}
