#include "generic.h"

byte pwmValues[]  =  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

bool genericCommands(char cmd_array[][10]) {
  if (strcmp(cmd_array[0], "dw") == 0) {
    byte pin = atoi(cmd_array[1]);
    bool state = atoi(cmd_array[2]);
    if (state == 1 || state == 0) {
      digitalWrite(pin, state);
      //update pwm state of the pin
      for (byte i = 0; i < sizeof(pwmPins); i++) {
        if (pwmPins[i] == pin) {
          pwmValues[i] = state * 255;
        }
      }
      Serial.println(ok_msg);
    }

  } else if (strcmp(cmd_array[0], "pm") == 0) {

    byte pin = atoi(cmd_array[1]);
    bool state = atoi(cmd_array[2]);
    pinMode(pin, state);
    Serial.println(ok_msg);

  }else if (strcmp(cmd_array[0], "gpws") == 0) {
    
    byte pin = atoi(cmd_array[1]);
    for (byte i = 0; i < sizeof(pwmPins); i++) {
      if (pin == pwmPins[i]) {
        Serial.println(pwmValues[i]);
        break;
      }
    }
    
  }else if (strcmp(cmd_array[0], "gds") == 0) {

    byte pin = atoi(cmd_array[1]);
    Serial.println(digitalReadOutputPin(pin));
    
  }else if (strcmp(cmd_array[0], "aw") == 0) {
    byte pin = atoi(cmd_array[1]);
    byte state = atoi(cmd_array[2]);
    bool statechanged = 0;
    for (byte i = 0; i < sizeof(pwmPins); i++) {
      if (pin == pwmPins[i]) {
        analogWrite(pin, state);
        pwmValues[i] = state;
        Serial.println(ok_msg);
        statechanged = 1;
      }
    }
    if (!statechanged) {
      Serial.println(err_msg);
    }
  } else {
    return false;
  }
  return true;
}
