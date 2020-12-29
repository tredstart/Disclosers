#include "crossbase.h"
#include "ESP8266WiFi.h"
 

 // nie jest potrzebne na razief
//String ssid = ""; //Enter SSID
//const char* password = "yarilpb1805"; 

CrossBase crossbase("crossuniverse");

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
 // WiFi.begin(ssid, password);
   //   while (WiFi.status() != WL_CONNECTED)      to tez nie potrzebne na razie
     // {
       //   delay(500);
         // Serial.print("*");
      //}
}

void loop() {
  // put your main code here, to run repeatedly:

   

  String ssid = "";   // ----------------> tu powinno pojawic sie ssid od ktorego bedziemy mierzyli moc 
  int rssi = avg_rssi(ssid);
  
  Serial.print("Punkt A ---->");
  Serial.print(rssi);
    int n = WiFi.scanNetworks();
    
    for(int j = 0; j < n; j++){
      Serial.println(WiFi.SSID(j));
      if(WiFi.SSID(j) == ssid){
         abs(WiFi.RSSI(j));
      } 
    }
    
    WiFi.scanDelete();
  Serial.println("");
  delay(5000);// czas na przejscie miedzy punktami
  // Wait a bit before scanning again
    
}// loop

int avg_rssi(String ssid){
  int wifi_signal = 0;
  int times = 4;                    //ilosc pomiarow dla wysredniania, dla wiekszej dokladnosci zwiekszyc 
  for(int i = 0; i < times; i++){
    int n = WiFi.scanNetworks();
    
    for(int j = 0; j < n; j++){
      
      if(WiFi.SSID(j) == ssid){
        wifi_signal += abs(WiFi.RSSI(j));
      } 
    }
    
    WiFi.scanDelete();
  }
  return wifi_signal/times;
  
  

}
