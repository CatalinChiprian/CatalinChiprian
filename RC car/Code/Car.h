#ifndef Car_h
#define Car_h
#include <QTRSensors.h>
#include "Arduino.h"
#include <Servo.h>
#include <Wire.h>

class Car{
private:
  // External objects
  QTRSensors _qtrs;
  Servo _steering_servo;
  // Pins
  unsigned int _rpwm_pin; 
  unsigned int _lpwm_pin;
  unsigned int _ir_pins[2];
  // Values
  uint16_t _qtr_values[6];
  // MPU6050
  void initialize_mpu();
public:
  Car(unsigned int rpwm_pin, unsigned int lpwm_pin, unsigned int qtr_pins[6], unsigned int ir_pins[2], Servo &obj); // Constructor initializes everything
  // Movement logic
  void set_speed(int speed); // speed between -100 and 100
  void steer(int steering_angle); // steering_angle between -27 and 27 (negative - left, positive - right)
  // Sensors logic
  void calibrate_qtrs(unsigned int min, unsigned int max);
  int get_line_position();
  float get_ir_distance(unsigned int ir_pin);
  bool is_obstacle();
  bool detects_finish();
  double get_tilt();
}
};
#endif