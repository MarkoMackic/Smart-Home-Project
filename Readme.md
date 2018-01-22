# :smile:  Smart Home Automation :smile:
#### This is project under active development (with many features unfinished, or buggy), so don't expect it to do something that works out of the box ! 
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

First thing you need is a single `Arduino Mega` board as master to all other devices, later I plan on supporting other boards but for now I only want this to work on one. If you want to test the program you're going to need a couple of external devices, for now you can use `Digital output devices`, `PWM output devices`, `Tlc 5940`, `Servo` (not currently supported in *client software* code) , so I'll first walk you through the process of how the firmware works, and then I'm going to use one specific part of code which you can reference if you ever want to contribute and write a driver for a device :smile:. The basic idea of this project is that you can control everything relevant to hardware control through serial port. When you first connect to device you should send it `authmessage` which is defined in `MasterMega.ino` because we want to make sure only our client software can use this device.  And then after that you are in command mode and here is a code sample so I can walk you through :
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
Ok, so every driver should have some specific [header guard](https://en.wikipedia.org/wiki/Include_guard), it should include `Arduino.h` for use of `Arduino` std library functions, and include `globals.h` if you need it. I use to include `globals.h` anyway. You should initialize the resources you need, and define handling function. Don't use dynamic memory allocation because of heap fragmentation.

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

---
### :large_blue_circle: Client software
Source for this is located in `/software/client/windows/SmartHomeClient/` relative to project root. So here is files structure thats relevant for us now:

```
software (dir)
  | client (dir)
    | windows (dir)
      | SmartHomeClient (dir)
              | Resource_DLLS (dir) - containes references to EMGU, XZing, JSON.Net
                | SmartHomeClient (dir_main)
                | SmartHomeClient.sln



```
*IMPORTANT : When I talk about files in this block below, they exist in directory and 
subdirectories of SmartHomeClinet (dir_main). So from now on, every path I write 
is relative to that.*

###### Some information for potential contributors :
* Requirements are : <b style="color:#aa44fF">EmguCV 3.0.0</b>, <b style="color:#aa44fF">Newtonsoft JSON.Net</b>, <b style="color:#aa44fF" >XZing.Net</b>

 * I don't know but I think resource DLLs are x64. If someone would test this with x86 arhitecture and file an issue,
so I could address it. 

 * I use Visual Basic 2010 Express IDE for and project is compiled with .NET 4, so if you don't have or want to use this editor and you use newer one please submit only code files you've changed or added, I will somehow link them to existing project.

 * If you contribute, there is just one thing to know for now, I don't have any preferences for coding standards, but the code you write should be easy to understand.
 
###### And now it's time to shortly describe you the lifetime of program, with my opinions about some of them :

* `/MainForm.vb` ( the program entry point) :

  * It loads showing `SerialPortDialog` and `ClientConnectionDialog`, so we take data from those dialogs and we initialize our modules `HardwareComm` , `MessageHandler`, `DBDriver`, `MasterController`, `ClientMiddleware` (if connection data is entered in `ClientConnectionDialog`).
  * Instances of the modules are held in `Globals` for easier accessing.
  * It attaches a `LoggedIn` event handler to our `MasterController` so we could continue initialization, initializing `DeviceManager`
  * It contains log container, and control panel, for accessing other GUI modules.
* `/Hardware/HardwareComm.vb`:
  * Provides us with serial port communication
  * Provides us the callback when Arduino responds
  * It has queue so more requests can be processed 
  * It has thread for pulling data from `msg` string buffer ( this is inefficient, it should be implemented whithout thread and `String` shouldn't be the buffer type, but it serves it's purpose )
  * API : 
     * ```vb
        Public Sub New(ByVal comName As String, ByVal baudRate As Integer)
       ```
       
     * ```vb
        Public Function sendData(ByVal cmd As String,
                                 Optional ByVal waitForData As Boolean = False,
                                 Optional ByVal caller As Object = Nothing,
                                 Optional ByVal callback As String = "SerialDataRecieved")
       ```
     * ```vb
         Public Function stopCommunication()
       ```
     *  ```vb
         Public Function stopCommunication()
       ```
   * It's API is mostly used by another layer of abstraction which is `MasterController`, and `MainForm` when constructing it.
* `/Hardware/MasterController.vb`:
  * Manages Arduino master device with higher level of abstraction.
  * Isolates direct SP communication with something more meaningful.
  * 1 Thread for getting data about states on analog and digital pins ( implemented with timeout feature ) and will broadcast them for input devices.
  * Measures hardware communication speed on thread since it's most active by the means of communication to device ( I don't know if these calculations are correct, if someone is willing to review it, open issue describing opinion on how to get it more accurate ).
  * API : 
    * ```vb
      Public Sub New(Optional ByVal State As Integer = States.Login)
      ```
    * ```vb
       Public Function sendData(ByVal cmd As String,
                                Optional ByVal waitForData As Boolean = False,
                                Optional ByVal caller As Object = Nothing,
                                Optional ByVal callback As String = "SerialDataRecieved")
         ```
    * ```vb
       Public Function Destroy()
       ```
  * This module is mostly used to interact with master device hardware. 
  
* `/Hardware/DeviceManager.vb`:
  * Manages Devices objects.
  * Makes sure there is no pin conflicts between devices.
  * 1 Thread for sending device data to server, so users could access their house in web browser.
  * Measures network communication speed on thread since it's most active by the means of communication to server ( I don't know if these calculations are correct, if someone is willing to review it, open issue describing opinion on how to get it more accurate ).
  * API : 
    * ```vb
      Public Sub New()
      ```
    * ```vb
        Public Function addDevice(ByVal devName As String,
                                  ByVal devPins() As Integer,
                                  ByVal devType As Integer,
                                  ByVal devId As Integer,
                                  Optional ByVal devMasterId As Integer = -1,
                                  Optional ByVal devAddress As String = Nothing)
      ```
    * ```vb
      Public Sub attachUI(ByVal UI As DeviceManagerUI) -> UI communication not yet implemented
       ```
    * ```vb
      Public Sub Destroy()
      ```
  * Will handle loading devices from database.


### :large_blue_circle: Server


# New Document
