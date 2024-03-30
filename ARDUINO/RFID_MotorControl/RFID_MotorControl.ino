#include <SPI.h>
#include <MFRC522.h>
 
#define SS_PIN 10
#define RST_PIN 9
#define SERVO_PINA 7
#define SERVO_PINB 6
MFRC522 mfrc522(SS_PIN, RST_PIN);   
void setup() 
{
  Serial.begin(9600);   
  SPI.begin();      
  mfrc522.PCD_Init();   
}

void loop() 
{
  if(checkCard(mfrc522)){
    Serial.print("UID:");
    Serial.print(getCardUID(mfrc522));
    Serial.println("");
    delay(500);
    if(Serial.readString() ='G'){
      driveServo(true);
      delay(3000);
      stopServo();
      delay(5000);
      driveServo(false);
      delay(3000);
      stopServo();
    }
  }  
} 

void driveServo(bool direction){
  if(direction){
    digitalWrite(SERVO_PINA,HIGH);
    digitalWrite(SERVO_PINB,LOW);
  }
  else{
    digitalWrite(SERVO_PINA,LOW);
    digitalWrite(SERVO_PINB,HIGH);
  }
}

void stopServo(){
    digitalWrite(SERVO_PINA,LOW);
    digitalWrite(SERVO_PINB,LOW);
}

bool checkCard(MFRC522 card){return (card.PICC_IsNewCardPresent()&&card.PICC_ReadCardSerial()) ;}

String getCardUID(MFRC522 card){
  String content;
  for (byte i = 0; i < card.uid.size; i++) 
  {
     content.concat((card.uid.uidByte[i], HEX));
     content.concat(card.uid.uidByte[i] < 0x10 ? "0" : "");
  }
  return content; 
}