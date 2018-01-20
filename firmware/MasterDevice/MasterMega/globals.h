#include "Arduino.h"

#ifndef G_PINS
#define G_PINS

const byte analogPins[] = {A0,A1,A2,A3,A4,A5,A6,A7,A8,A9,A10,A11,A12,A13,A14,A15};
const byte pwmPins[] =    {2,3,4,5,6,7,8,9,10,11,12,13,44,45,46};



#endif

#ifndef	S_MSG
#define S_MSG

extern bool digitalReadOutputPin(uint8_t pin);

const char ok_msg[] = "OK";
const char err_msg[] = "ERROR";

#endif
