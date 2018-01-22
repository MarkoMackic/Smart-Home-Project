# :smile:  Smart Home Automation :smile:
#### This is project under active development, so don't expect it to do something that works out of the box !
---
## :book: About 

The goal of this project is to create ecosystem of devices that can be controlled with  `Arduino` microcontroller platform, and create drivers for them to be used in *client software* ( which is written in `Visual Basic` ). And finally connecting software to *server* (which is currently written in `Python`) so user could control those devices directly from browser and read device states at real time.
 
---
## :book: More details for interested 

### :large_blue_circle: Hardware and firmware

Source for this is located in `/firmware/` relative to project root. So here is files structure thats relevant for us now:

```
firmware (dir)
    | MasterDevice (dir)
        | MasterMega (dir)

When I talk about files in text below, they exist in this directory.
```

First thing you need is a single `Arduino Mega` board as master to all other devices, later I plan on supporting other boards but for now I only want this to work on one. If you want to test the program you're going to need a couple of external devices, for now you can use `Digital output devices`, `PWM output devices`, `Tlc 5940`, `Servo` (not currently supported in *client software* code) , so I'll first walk you through the process of how the firmware works, and then I'm going to use one specific part of code which you can reference if you ever want to contribute and write a driver for a device :smile: . The basic idea of this project is that you can control everything relevant to hardware control through serial port. When you first connect to device you should send it `authmessage` which is defined in `MasterMega.ino` because we want to make sure only our client software can use this device.  And then after that you are in command mode and here is a code sample so I can walk you through :
```cpp

    //interpret the command from pc
    void getCmd() {
        //...Code before this is currently irelevant to you

        char *pch;
        cmd_indexer  = 0;
        char cmd_array[4][10] = {{0}};
        pch = strtok(input, ":");
        strcpy(cmd_array[cmd_indexer], pch);
        while (cmd_indexer < 3) {
          cmd_indexer++;
          pch = strtok(0, ":");
          if (pch != 0) {
            strcpy(cmd_array[cmd_indexer], pch);
          } else {
            strcpy(cmd_array[cmd_indexer], "");
          }
        }

        bool isHandled = false;
    #ifdef GENERIC_DRIVER
        if (!isHandled) {
          isHandled = genericCommands(cmd_array);
        }

    #endif
    #ifdef SERVO_DRIVER
        if (!isHandled) {
          isHandled = servoCommands(cmd_array);
        }
    #endif
    #ifdef TLC5940_DRIVER
        if (!isHandled) {
          isHandled = tlc59Commands(cmd_array);
        }
    #endif

        //parser end
        if (!isHandled) Serial.println("CMDERROR");


      }

```
Great, what this code does is it splits the input `char[]` from serial port with `:` character, and then passes the array to the handlers. Now you see, every driver has to has it's own handler, and all drivers other than generic can be excluded, so if the user runs out of RAM, he could use only drivers he needs. I'll go over `Servo` driver for example : 

`Servo.h`
```cpp
    
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
```
Ok, so every driver should have some specific [header guard](https://en.wikipedia.org/wiki/Include_guard), it should include `Arduino.h` for use of `Arduino` std library functions, and include `globals.h` if you need it. I use to include `globals.h` anyway. You should initialize the resources you need, and define handling function.

`Servo.cpp`
```cpp

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
          return true;
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


```
Here we have my implementation of `Servo driver`, so this driver supports three commands `as` (attachServo), `ds` (detachServo), `sp` (servoPosition). I think the code is self explainable, if not please add issue and I'll update readme. Main command is in `cmd_array[0]` and parameters are at indexes `1 to 3`, you may add more, but be aware that you are tight on RAM on `Arduino`. Function that handles serial port data should return true if the main command is found, an please make sure to always use `Serial.println` because the *client software* uses `\n` to signal success at receiving message.

That's it my friends about hardware, drivers, and handlers, you hopefully can now easily write driver for device that you need, but please sure it's not is some way supported in firmware, because if it is, it's easier to write driver in *client software* where you have more hardware resources to play with.

### :large_blue_circle: Software

### :large_blue_circle: Server

