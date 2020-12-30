#include "crossbase.h"
#include "band.h"
#include "ESP8266WiFi.h"
 

 // nie jest potrzebne na razief
String ssid = "KaiRu’s iPhone"; //Enter SSID
const char* password = "yarilpb1805"; 

typedef struct AccessPoints{

  String X, Y, Z, V;
  
}AccessPoints;


CrossBase crossbase("crossuniverse");
Band* band = new Band();

AccessPoints AP; 


void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED){
    delay(500);
    Serial.print("*");
  }
  AP = {crossbase.getData("AP-X"), crossbase.getData("AP-Y"), crossbase.getData("AP-Z"), crossbase.getData("AP-V")};

}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.println("start");
  String id = band->getID();
  Serial.println(id);
  band->vibrate = crossbase.getData("id-"+id+"-vibrate") == "true";
  Serial.println(band->vibrate);
  if(band->vibrate){
    Serial.println("Vibrating");
    delay(2000);
  }
  
  getCoordinates(AP.X, AP.Y, AP.Z, AP.V, band);

  crossbase.setDataString("id-"+id+"-X", String(band->X));
  crossbase.setDataString("id-"+id+"-Y", String(band->Y));
  crossbase.setDataString("id-"+id+"-Z", String(band->Z));
  crossbase.setDataString("id-"+id+"-V", String(band->V));
  
  Serial.println("");
    
}// loop

int findCoordinate(String ssid, int n){
  int wifi_signal = 0;
  for(int j = 0; j < n; j++){
    
    if(WiFi.SSID(j) == ssid){
      wifi_signal = abs(WiFi.RSSI(j));
      Serial.println(wifi_signal);
    } 
  }
  return wifi_signal;
}

void getCoordinates(String X, String Y, String Z, String V, Band *band){
  
  int X_signal = 0;
  int Y_signal = 0;
  int Z_signal = 0;
  int V_signal = 0;

  int times = 4;                    
  for(int i = 0; i < times; i++){
    int n = WiFi.scanNetworks();
    if(X != "None"){
      X_signal += findCoordinate(X, n);
    }
    if(Y != "None"){
      Y_signal += findCoordinate(Y, n);
    }
    if(Z != "None"){
      Z_signal += findCoordinate(Z, n);
    }
    if(V != "None"){
      V_signal += findCoordinate(V, n);
    }
    
    
    WiFi.scanDelete();
  }
  band->X = X_signal/times;
  band->Y = Y_signal/times;
  band->Z = Z_signal/times;
  band->V = V_signal/times;

  Serial.print(band->X);
    Serial.print(", ");
    Serial.println(band->Y);
}
