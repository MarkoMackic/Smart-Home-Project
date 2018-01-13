#include "tlc59.h"


byte numTlcs = 0;
bool tlc59Commands(char cmd_array[][10]) {
  if (strcmp(cmd_array[0], "tl59i") == 0 ) {
    
    byte tNumTlcs = atoi(cmd_array[1]);

    if (tNumTlcs < MAX_TLCS && tNumTlcs > 0 && numTlcs == 0) {
      numTlcs = tNumTlcs;
      Tlc.init(numTlcs);
      Serial.println(ok_msg);
    } else {
      Serial.println(err_msg);
    }

  } else if (strcmp(cmd_array[0], "tl59c") == 0 ) {
    if (numTlcs > 0) {
      Tlc.clear();
      Serial.println(ok_msg);
    } else {
      Serial.println(err_msg);
    }
  } else if (strcmp(cmd_array[0], "tl59u") == 0 ) {
    if (numTlcs > 0) {
      Tlc.update();
      Serial.println(ok_msg);
    } else {
      Serial.println(err_msg);
    }
  } else if (strcmp(cmd_array[0], "tl59s") == 0 ) {
    if (numTlcs > 0) {
      int pin = atoi(cmd_array[1]);
      int value = atoi(cmd_array[2]);
      if (pin < numTlcs * 16 && value > -1 && value < 4096) {
        Tlc.set(pin, value);
        Serial.println(ok_msg);
      } else {
        Serial.println(err_msg);
      }
    } else {
      Serial.println(err_msg);
    }
  } else {
    return false;
  }

  return true;


}
