# Smart Home Autmation


The goal of this project is to do device control with Arduino. The project is divided into couple pieces : 
* Hardware control over serial port
* Devices and drivers abstraction layer in software
* Server for web interfacing

I've done the hardware control over serial port ( but protocol changes will be done, to increase speed of parsing and execution ). For now there is basic <b>I/O</b> support and <b>Servo</b> is supported. When software connects to master device, it must send 'lo' (log out) and authmessage that is defined when code is uploaded. All the serial messages are parsed when there is '\n' in the buffer, so software needs to take care of that. When master device boots up, all pins are in INPUT mode,.

The commands currently supported by hardware are :
* `pps -> `It will print PWM values ( last written by aw command ) to software in format `<pwm>pin:value;pin:value; ... </pwm>`
* `pds -> `It will print DIGITAL states ( last written by dw command ) to software in format `<ds>pin:state;pin:state; ... </ds>`
* `pas -> `It will print ANALOG values read from analog inputs to software in format `<an>pin:value;pin:value; ... </an>`
* `pm:pin:mode ->` pinMode(pin, mode) . Mode is either 0 if the pin is input or 1 if the pin is output.
* `dw:pin:state ->` digtialWrite(pin, state) . State is either 0 or 1.
* `aw:pin:value ->` analogWrite(pin, value). Value is value 0 - 255.
* `as:pin ->` attachServo(pin). Attaches servo to pin and stores the servo instance to array.
* `ds:pin ->` detachServo() for servo that is attached to pin.
* `sp:pin:pos ->` Servo.write(pos) for servo that is attached to pin.


I hope this command set will be expanded for many more devices. I'll certainly work on that. Currently I'm interested in extending this protocol for SLAVE devices which would be able to have all devices supported on master, because we don't have so many pins on single Arduino. And that's it for hardware part. 


Software is here to get us device abstraction we need. It must query master device to get state of pins or devices, and must be able to control their state based on user input. It provides us with TCP client for our server so we could connect our house to the world. It's not yet done but here is the things I finished:
* `HardwareComm` is the main serial port abstraction. It's responsible for handling writes to serial port, and invoking callbacks on classes that sent the data. There is a single thread in this module which serves for getting data from `msg` buffer and invoking callbacks.
* `FaceRecognition` uses EmguCv. It's the UI for managing `faces` table in database and for recognizing faces. It provides us with automated procedures which are defined by `state` variable inside the class.

* `MasterDevice	` abstraction over `HardwareComm` ( I don't know  is this needed, but for now I'll keep it)

Server will be there to provide web interface. It's not yet started.



<b>If you feel I left something from this README please feel free to contribute, I'd appreciate it very much :).




