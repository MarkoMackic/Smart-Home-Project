
#include "servo.h"

Servo servos[max_servos];
byte servoPins[max_servos];

bool servoCommands(char cmd_array[][10]) {
  if (strcmp(cmd_array[0], "as") == 0) {
    byte pin = atoi(cmd_array[1]);
    //check if the pin is used
    for (byte i = 0; i < max_servos; i++) {
      if (servoPins[i] == pin) {
        Serial.println(err_msg);
      }
    }
    //check for available servo
    boolean servoFound = false;
    for (int i = 0; i < max_servos; i++) {
      if (servos[i].attached() == false) {
        servos[i].attach(pin);
        servoPins[i] = pin;
        servoFound = true;
        break;
      }
    }
    if (servoFound) {
      Serial.println(ok_msg);
    } else {
      Serial.println(err_msg);
    }


  } else if (strcmp(cmd_array[0], "ds") == 0) {
    //check if there's a servo that's attached to pin, and detach it
    byte pin = atoi(cmd_array[1]);
    boolean servoFound = false;

    for (byte i = 0; i < max_servos; i++) {
      if (servos[i].attached() == true && servoPins[i] == pin) {
        servos[i].detach();
        servoPins[i] = 0;
        servoFound = true;
        break;
      }
    }
    if (servoFound) {
      Serial.println(ok_msg);

    } else {
      Serial.println(err_msg);
    }
  } else if (strcmp(cmd_array[0], "sp") == 0) {
    //check if there's a servo that's attached to pin, and write position to it
    byte pin = atoi(cmd_array[1]);
    byte pos = atoi(cmd_array[2]);
    boolean servoFound = false;
    for (byte i = 0; i < max_servos; i++) {
      if (servos[i].attached() == true && servoPins[i] == pin) {
        servos[i].write(pos);
        servoFound = true;
        break;
      }
    }
    if (servoFound) {
      Serial.println(ok_msg);

    } else {
      Serial.println(err_msg);
    }
  } else {
    return false;
  }
  return true;

}
