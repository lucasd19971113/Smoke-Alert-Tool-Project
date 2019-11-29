
#include <ESP8266WiFi.h>
#include <PubSubClient.h>


//const char* ssid = "AndroidAP0FBA";
//const char* password = "popn3103";

//char* ssid = "PPGEEC";
//const char* password = "ppgeec#fci";

//char* ssid = "aco";
//const char* password = "1234abcd";

char* ssid = "RNL";
const char* password = "R4n@p3lV";

const char* mqtt_server = "test.mosquitto.org";

WiFiClient espClient;
PubSubClient client(espClient);

const int PinA0 = A0;

int gas_limit = 300;


// Timers auxiliar variables
long now = millis();
long lastMeasure = 0;

void setup_wifi() {
  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("WiFi connected - ESP IP address: ");
  Serial.println(WiFi.localIP());
}


void callback(String topic, byte* message, unsigned int length) {
  Serial.print("Message arrived on topic: ");
  Serial.print(topic);
  Serial.print(". Message: ");
  String messageTemp;
  
  for (int i = 0; i < length; i++) {
    Serial.print((char)message[i]);
    messageTemp += (char)message[i];
  }
  Serial.println();
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    
    if (client.connect("ESP8266Client")) {
      Serial.println("connected"); 
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      
      delay(5000);
    }
  }
}

// The setup function sets your ESP GPIOs to Outputs, starts the serial communication at a baud rate of 115200
// Sets your mqtt broker and sets the callback function
// The callback function is what receives messages and actually controls the LEDs
void setup() {
 
  Serial.begin(115200);
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);

}

// For this project, you don't need to change anything in the loop function. Basically it ensures that you ESP is connected to your broker
void loop() {

  delay(10);

  int valor_analogico = 0;

  valor_analogico = analogRead(PinA0);

    static char gasTemp[7];
    dtostrf(valor_analogico, 6, 2, gasTemp);

  now = millis();
  // Publishes new temperature and humidity every 30 seconds
  
    // Sensor readings may also be up to 2 seconds 'old' (its a very slow sensor)
   
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

    //Envia Email a cada 60 segundos se o gas for maior que 300
    if (now - lastMeasure > 60000 && valor_analogico > 300) {
    lastMeasure = now;
    // Sensor readings may also be up to 2 seconds 'old' (its a very slow sensor)
      client.publish("room/notification", gasTemp);

      Serial.println("Email enviado: ");
    Serial.print(valor_analogico);
    
    }

    

  if (!client.connected()) {
    reconnect();
  }
  if(!client.loop())
    client.connect("ESP8266Client");


    
    
} 