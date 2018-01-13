#ifndef GENERIC_DRIVER
#define GENERIC_DRIVER

#include <Arduino.h>
#include "globals.h"


extern byte pwmValues[];
bool genericCommands(char cmd_array[][10]);


#endif
