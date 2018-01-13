#ifndef TLC5940_DRIVER
#define TLC5940_DRIVER

#include <Arduino.h>
#include "globals.h"
#include <Tlc5940.h>

extern byte numTlcs;
bool tlc59Commands(char cmd_array[][10]);


#endif
