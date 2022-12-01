#include <BLEUUID.h>
#include <BLEDevice.h>
#include <BLEUtils.h>
#include <BLEServer.h>
#include <BLE2902.h>
#include <Stepper.h>

// See the following for generating UUIDs:
// https://www.uuidgenerator.net/

#define SERVICE_UUID "4fafc201-1fb5-459e-8fcc-c5c9c331914b"

BLECharacteristic SpeedCharacteristic("11111111-36e1-4688-b7f5-ea07361b26a8", BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLECharacteristic DirectionCharacteristic("22222222-36e1-4688-b7f5-ea07361b26a8", BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLECharacteristic DistanceCharacteristic("33333333-36e1-4688-b7f5-ea07361b26a8", BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLECharacteristic RunCharacteristic("aaaaaaaa-36e1-4688-b7f5-ea07361b26a8", BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLECharacteristic ReturnCharacteristic("bbbbbbbb-36e1-4688-b7f5-ea07361b26a8", BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);

// Define pin connections & motor's steps per revolution
const int dirPin = 26;
const int stepPin = 27;
const int stepsPerRevolution = 3200;
int userSpeed = 0;
int userDirection = 0;
int userSteps = 0;

Stepper myStepper(stepsPerRevolution, 26, 27);

void motorRun(){
  Serial.print("userSpeed is: "); 
  Serial.println(String(userSpeed));
  Serial.print("userDirection is: "), 
  Serial.println(String(userDirection));
  Serial.print("userSteps is: ");
  Serial.println(String(userSteps));
  myStepper.setSpeed(userSpeed);
  myStepper.step((userDirection)*(userSteps));
  RunCharacteristic.setValue("0");
  delay(4000);
  std::string returnValue = ReturnCharacteristic.getValue();
  while(returnValue != "Return") {
    delay(1000);
    returnValue = ReturnCharacteristic.getValue();
  }
  ReturnCharacteristic.setValue("0");
  myStepper.setSpeed(107);
  myStepper.step((-userDirection)*(userSteps));
  delay(1000);
}

void setup() {
  Serial.begin(115200);
  Serial.println("Starting BLE work!");

//  pinMode(stepPin,OUTPUT);
//  digitalWrite(stepPin, HIGH);

  BLEDevice::init("SlipNTrip ESP32");
  BLEServer *pServer = BLEDevice::createServer();
  BLEService *pService = pServer->createService(SERVICE_UUID);
  pService->addCharacteristic(&SpeedCharacteristic);
  SpeedCharacteristic.setValue("0");
  pService->addCharacteristic(&DirectionCharacteristic);
  DirectionCharacteristic.setValue("0");
  pService->addCharacteristic(&DistanceCharacteristic);
  DistanceCharacteristic.setValue("0");
  pService->addCharacteristic(&RunCharacteristic);
  RunCharacteristic.setValue("0"); 
  pService->addCharacteristic(&ReturnCharacteristic);
  ReturnCharacteristic.setValue("0"); 
  
  pService->start();
  // BLEAdvertising *pAdvertising = pServer->getAdvertising();  // this still is working for backward compatibility
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(true);
  pAdvertising->setMinPreferred(0x06);  // functions that help with iPhone connections issue
  pAdvertising->setMinPreferred(0x12);
  BLEDevice::startAdvertising();
  Serial.println("Characteristic defined! Now you can read it in your phone!");
}

void loop() {
  // put your main code here, to run repeatedly
  delay(5000);
  
  std::string speedValue = SpeedCharacteristic.getValue();
  std::string directionValue = DirectionCharacteristic.getValue();
  std::string distanceValue = DistanceCharacteristic.getValue();
  // std::string runValue = RunCharacteristic.getValue();
  
  if(speedValue == "15") {
    userSpeed = 107;
  }
  else if(speedValue == "16") {
    userSpeed = 111;
  }
  else if(speedValue == "17") {
    userSpeed = 114;
  }
  else if(speedValue == "18") {
    userSpeed = 118;
  }
  else if(speedValue == "19") {
    userSpeed = 121;
  }
  else if(speedValue == "20") {
    userSpeed = 125;
  }
  else if(speedValue == "21") {
    userSpeed = 128;
  }
  else if(speedValue == "22") {
    userSpeed = 132;
  }
  else if(speedValue == "23") {
    userSpeed = 135;
  }
  else if(speedValue == "24") {
    userSpeed = 139;
  }
  else if(speedValue == "25") {
    userSpeed = 142;
  }
  else if(speedValue == "26") {
    userSpeed = 146;
  }
  else if(speedValue == "27") {
    userSpeed = 149;
  }
  else if(speedValue == "28") {
    userSpeed = 153;
  }
  else if(speedValue == "29") {
    userSpeed = 156;
  }
  else if(speedValue == "30") {
    userSpeed = 160;
  }
  else if(speedValue == "31") {
    userSpeed = 163;
  }
  else if(speedValue == "32") {
    userSpeed = 167;
  }
  else if(speedValue == "33") {
    userSpeed = 170;
  }
  else if(speedValue == "34") {
    userSpeed = 174;
  }
  else if(speedValue == "35") {
    userSpeed = 174;
  }
  else {
    userSpeed = 0;
  }
  
  // Serial.println(String(userSpeed));
  
  // myStepper.setSpeed(userSpeed);
  

  
  if(directionValue == "F") {
    userDirection = -1;
  }
  else if(directionValue == "B"){
    userDirection = 1;
  }
  else {
    userDirection = 0;
  }
  
  if(distanceValue == "0") {
    userSteps = 0;
  }
  else if(distanceValue == "1") {
    userSteps = 400;
  }
  else if(distanceValue == "2") {
    userSteps = 800;
  }
  else if(distanceValue == "3") {
    userSteps = 1200;
  }
  else if(distanceValue == "4") {
    userSteps = 1600;
  }
  else if(distanceValue == "5") {
    userSteps = 2000;
  }
  else if(distanceValue == "6") {
    userSteps = 2400;
  }
  else if(distanceValue == "7") {
    userSteps = 2800;
  }
  else if(distanceValue == "8") {
    userSteps = 3200;
  }
  else if(distanceValue == "9") {
    userSteps = 3600;
  }
  else if(distanceValue == "10") {
    userSteps = 4000;
  }
  else if(distanceValue == "11") {
    userSteps = 4400;
  }
  else if(distanceValue == "12") {
    userSteps = 4800;
  }
  else if(distanceValue == "13") {
    userSteps = 5200;
  }
  else if(distanceValue == "14") {
    userSteps = 5600;
  }
  else if(distanceValue == "15") {
    userSteps = 6000;
  }
  else {
    userSteps = 0;
  }
  Serial.print("userSpeed is: "); 
  Serial.println(String(userSpeed));
  Serial.print("userDirection is: "), 
  Serial.println(String(userDirection));
  Serial.print("userSteps is: ");
  Serial.println(String(userSteps));
  
  std::string runValue = RunCharacteristic.getValue();

  if(runValue == "Run") {
    motorRun();
    Serial.println("Loop worked");
//      myStepper.setSpeed(userSpeed);
//      myStepper.step((userDirection)*(userSteps));
//      RunCharacteristic.setValue("0");
//      delay(2000);
//      Serial.println("Loop worked");
//      myStepper.setSpeed(107);
//      myStepper.step((-userDirection)*(userSteps));
  }
}
