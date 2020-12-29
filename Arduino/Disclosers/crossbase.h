#include <ESP8266HTTPClient.h>

class CrossBase{
  private:
    String db_name;
    String base_link = "http://crossbase.herokuapp.com/";
  public:
    CrossBase(String database_name){
      db_name = database_name;
      Serial.print("CrossBase ");
      Serial.print(db_name);
      Serial.println(" was created successfully");
    }
    String getData(String keys=""){
      HTTPClient http;
      String link;
      if(keys == ""){
        link = base_link+"get/"+db_name+"/";
      }else{
        link = base_link+"get-by-keyword/"+db_name+"/"+keys+"/";
      }
      http.begin(link);
      int httpCode = http.GET();
      String response = http.getString();
      http.end();
      return response;
    }
    void setDataString(String key, String value){
      HTTPClient http;
      String link = base_link+"set/"+db_name+"/"+key+"/" + value + "/";
      http.begin(link);
      int httpCode = http.GET();
      http.end();
    }
};
