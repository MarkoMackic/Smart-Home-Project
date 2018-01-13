#ifndef SERVO_DRIVER
#define SERVO_DRIVER

#include <Arduino.h>
#include "globals.h"
#include <Servo.h>


const byte max_servos = sizeof(pwmPins);
extern Servo servos[max_servos];
extern byte servoPins[max_servos];

bool servoCommands(char cmd_array[][10]);

#endif
