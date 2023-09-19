#include "Car.h"

Servo steering_servo;
Car car(5, 6, (int[]){3, 4, 8, 9, 10, 11}, (int[]){14, 15}, steering_servo);
unsigned long delaydur = 1200;
bool avoided = false;

void setup() {
  Serial.begin(9600);
  steering_servo.attach(7);
  car.calibrate_qtrs(1000, 2500);
}

void loop() {
  car.set_speed(12);
  if(car.is_obstacle() && !avoided){
      for (unsigned long start = millis(); millis() - start < delaydur;) {
      car.steer(17);
     }
     avoided = true;
  }
  else car.steer(map(car.get_line_position(), 0,5000, -27, 27));

}