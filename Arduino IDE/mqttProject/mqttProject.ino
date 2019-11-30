
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
void callback(char* topic, byte* payload, unsigned int length);

//const char* ssid = "AndroidAP0FBA";
//const char* password = "popn3103";

//char* ssid = "PPGEEC";
//const char* password = "ppgeec#fci";

char* ssid = "aco";
const char* password = "1234abcd";

//char* ssid = "RNL";
//const char* password = "R4n@p3lV";

//const char* mqtt_server = "test.mosquitto.org";

//WiFiClient espClient;
//PubSubClient client(espClient);

const int PinA0 = A0;
const int PinBuzzer = D6;

int gas_limit = 300;


// Timers auxiliar variables
long now = millis();
long lastMeasure = 0;

const char* mqtt_server="test.mosquitto.org";
WiFiClient espclient;
PubSubClient client(mqtt_server,1883,callback,espclient);


void setup() {
  
   
   pinMode(PinA0,INPUT);
  
  pinMode(PinBuzzer, OUTPUT);
  
  Serial.begin(115200);
  Serial.print("connecting");
  WiFi.begin(ssid,password);         //SSID,PASSWORD 
  while(WiFi.status()!=WL_CONNECTED){
    delay(500);
    Serial.print(".");
  }
  Serial.println();
  
  reconnect();

}


void callback(char* topic,byte* payload,unsigned int length1){    
Serial.print("message arrived[");
Serial.print(topic);
Serial.println("]");

for(int i=0;i<length1;i++){
  Serial.print(payload[i]); 
  
}
if(payload[0]==49) digitalWrite(2,HIGH);    //ASCII VALUE OF '1' IS 49
else if(payload[0]==50)digitalWrite(2,LOW);//ASCII VALUE OF '2' IS 50
Serial.println();
}


void reconnect(){
  while(WiFi.status()!=WL_CONNECTED){
    delay(500);
    Serial.print(".");
  }
  while(!client.connected()){
  if(client.connect("ESP8266Client123456789")){
    Serial.println("connected");
  }
    else{
      Serial.print("failed,rc=");
      Serial.println(client.state());
      delay(500);
    }
  } 
}

// For this project, you don't need to change anything in the loop function. Basically it ensures that you ESP is connected to your broker
void loop() {

  
if(!client.connected()){
      reconnect();
    }
    
    client.loop();

    int valor_analogico = 0;

  valor_analogico = analogRead(PinA0);

    static char gasTemp[7];
    dtostrf(valor_analogico, 6, 2, gasTemp);

  now = millis();
  
   int sub = now - lastMeasure;

    //Envia Email a cada 10 segundos se o gas for maior que 300

  if (valor_analogico > 300) {
  digitalWrite(PinBuzzer, HIGH);
  Serial.println("Tempo decorrido: ");
  Serial.print(sub);
    if(sub > 60000){
      lastMeasure = now;
      // Sensor readings may also be up to 2 seconds 'old' (its a very slow sensor)

        delay(1500);
        Serial.println("Sending email......");
        client.publish("room/notification", gasTemp);
        delay(1500);
        Serial.println("EMAIL SENT SUCCESSFULY !!!!!");
        
        Serial.println("Email enviado: ");
       
      Serial.print(valor_analogico);
      delay(100);
      }
    
    }else{
      digitalWrite(PinBuzzer, LOW); 
      
     }
   
    dtostrf(valor_analogico, 6, 2, gasTemp);

    Serial.println("ANALOG VALUE: ");

    Serial.println(valor_analogico);
    Serial.println("");
    Serial.println("SENT VALUE: ");
    Serial.println(gasTemp);
    
    client.publish("room/sensor", gasTemp);

    
    
    Serial.print("Gas: ");
    Serial.print(valor_analogico);
    
    client.publish("room/level", gasTemp);
  

    
} 