#include "Car.h"

Car::Car(unsigned int rpwm_pin, unsigned int lpwm_pin, unsigned int qtr_pins[6], unsigned int ir_pins[2], Servo &obj) {
  _rpwm_pin = rpwm_pin;
  _lpwm_pin = lpwm_pin;
  _steering_servo = obj;
  _ir_pins[0] = ir_pins[0];
  _ir_pins[1] = ir_pins[1];

  _qtrs.setTypeRC();
  _qtrs.setSensorPins((const uint8_t[])qtr_pins, 6);
  _qtrs.setEmitterPin(2);

  initialize_mpu();
}

float Car::get_ir_distance(unsigned int ir_pin)
{
  float ADCValue = (float)analogRead(ir_pin);

  return  2583.711122992086 - 20.197897855471 * ADCValue + 0.071746539329 * ADCValue * ADCValue - 0.000115854182 * ADCValue * ADCValue * ADCValue + 0.000000068590 * ADCValue * ADCValue * ADCValue * ADCValue;
}

bool Car::is_obstacle(){
  int x=0;
    int result2 = get_ir_distance(_ir_pins[0])/10;
    int result1 = get_ir_distance(_ir_pins[1])/10;

    for(int i=0; i < 15; i++){
      if(result2-result1>= 10  && result1>20 && result1<50)
        x++;
      else
        x=0;
    }
    if(x==15) 
      return true;

    return false;
}


void Car::set_speed(int speed) 
{
  if(speed > 0 && speed <= 100){
    analogWrite(_lpwm_pin, 0);
    analogWrite(_rpwm_pin, map(speed, 0, 100, 0, 255));
  }else if(speed < 0 && speed >= -100){
    analogWrite(_lpwm_pin, map(speed, 0, 100, 0, 255));
    analogWrite(_rpwm_pin, 0);
  }else{
    analogWrite(_lpwm_pin, 0);
    analogWrite(_rpwm_pin, 0);
  }
}

void Car::calibrate_qtrs(unsigned int min, unsigned int max) {
  _qtrs.calibrate();
  for (uint8_t i = 0; i < 6; i++)
  {
    _qtrs.calibrationOn.minimum[i]=min;
    _qtrs.calibrationOn.maximum[i]=max;
  }
  delay(100);
}

void Car::steer(int servo_angle){
  _steering_servo.write(90 + servo_angle);
}

int Car::get_line_position(){
  return _qtrs.readLineWhite(_qtr_values);
}

bool Car::detects_finish() {
  for (uint8_t i = 0; i < 6; i++)
  {
    if(_qtr_values[i] == 1000) return false;
  }
  return true;
}

void Car::initialize_mpu(){
  Wire.begin();
  Wire.beginTransmission(0x68);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);
}

double Car::get_tilt(){
  int16_t AcX,AcY,AcZ;
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr,14,true);
  AcX=Wire.read()<<8|Wire.read();
  AcY=Wire.read()<<8|Wire.read();
  AcZ=Wire.read()<<8|Wire.read();
  int xAng = map(AcX,265,402,-90,90);
  int yAng = map(AcY,265,402,-90,90);
  int zAng = map(AcZ,265,402,-90,90);
  
  return RAD_TO_DEG * (atan2(-yAng, -zAng)+PI);
}
