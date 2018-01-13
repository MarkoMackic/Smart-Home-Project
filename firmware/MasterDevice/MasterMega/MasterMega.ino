/*
 * Command set : 
 * 
 * *** General ***
 *  dw:pin:state -> digitalWrite
 *  aw:pin:state -> analogWrite to PWM
 *  pm:pin:mode -> pinMode
 *  pps -> printPwmStates <pwm>pin:value;pin:value;....</pwm>
 *  pds -> printDigitalStates <ds>pin:state;pin:state;...</ds>
 *  pas -> printAnalogStates <as> pin:value;pin:value;....</as>
 *  
 *  *** Servo ***
 *  as:pin -> attachServo
 *  ds:pin -> detachServo
 *  sp:pin:pos -> Servo.write(pos) -> pin
 *  
 *  *** TLC5940 ***
 *  tl59i:num_tlcs -> TLC.init()
 *  tl59c -> Tlc.clear()
 *  tl59s:pin:value -> Tlc.set(pin,value)
 *  tl59u -> Tlc.update()
 * 
 */


#include "globals.h"
#include "servo.h"
#include "tlc59.h"

#include "generic.h" //this can't be excluded

/*Serial*/
#define max_cmd_size 60

boolean vertified = 0;
char input[max_cmd_size];
byte index = 0;
const char authMsg[] = "authmessage";
char tempC; 
char ret[300];
char buf[4];
byte cmd_indexer = 0;



/*TLC5940*/


void setup(){
	for(byte i=0;i<54;i++){
		pinMode(i,INPUT);
    //digitalWrite(i,0);
	}
	Serial.begin(115200);
  Serial.flush();

  in_buffer_empty();
	getAuth();
}

void loop(){
	if(vertified){
		//Now we're ok 
    if(Serial.available()){
		getCmd();
    }
	}else{
		//Reg again
		getAuth();
	}
}
//bufferempty
void in_buffer_empty(){
  while(Serial.available())
    Serial.read();
}
//auth program with house
void getAuth(){
  in_buffer_empty();
	while(!Serial.available());
	for( int i = 0; i < sizeof(input);  i++ )
   		input[i] = (char)0;
	index = 0;
	tempC = (char)Serial.read();
  
	while(!(tempC == '\n' || tempC == '\r')){
     
        input[index] = tempC;
        while(!Serial.available());
        tempC = (char)Serial.read();
        index++;
     
 }
 
  in_buffer_empty();
	if(strcmp(input,authMsg) == 0){
		Serial.println(ok_msg);
		vertified = 1;
	}else if(strcmp(input,"lo")==0){
		vertified = 0;
	}else{
		Serial.println(err_msg);
		vertified=0;
	}

}


//interpret the command from pc
void getCmd(){

		for( int i = 0; i < sizeof(input);  i++ )
   			input[i] = (char)0;
		index = 0;
		tempC = (char)Serial.read();
		while(!(tempC == '\n' || tempC == '\r')){
			  input[index] = tempC;
        while(!Serial.available());
			  tempC = (char)Serial.read();
			  index++;
		}

		if(strcmp(input,"pds")==0){
			printDigitalStates();
		}else if(strcmp(input,"pps") == 0){
			printPwmStates();
		}else if(strcmp(input,"pas") == 0){
			printAnalogStates();
		}else if(strcmp(input,"lo") == 0){
			vertified = 0;
		}else{ 
			char *pch;
	 		cmd_indexer	 = 0;
			char cmd_array[4][10]={{0}};
			pch	= strtok(input,":");
			strcpy(cmd_array[cmd_indexer],pch);
			while(cmd_indexer<3){
				cmd_indexer++;
				pch = strtok(0,":");
				if(pch != 0){
					strcpy(cmd_array[cmd_indexer],pch);
				}else{
					strcpy(cmd_array[cmd_indexer],"");
				}
			}

      bool isHandled = false;
      #ifdef GENERIC_DRIVER
        if(!isHandled){
          isHandled = genericCommands(cmd_array);
        }

      #endif
      #ifdef SERVO_DRIVER
        if(!isHandled){
          isHandled = servoCommands(cmd_array);
        }
      #endif
      #ifdef TLC5940_DRIVER
        if(!isHandled){
          isHandled = tlc59Commands(cmd_array);
        }
      #endif
     
			//parser end
	    if(!isHandled) Serial.println("CMDERROR");
			

		}
	
}



bool digitalReadOutputPin(uint8_t pin)
{
 uint8_t bit = digitalPinToBitMask(pin);
 uint8_t port = digitalPinToPort(pin);
 if (port == NOT_A_PIN) 
   return LOW;

 return (*portOutputRegister(port) & bit) ? HIGH : LOW;
}
//states of digital pins
void printDigitalStates(){
  strcpy(ret,"<ds>");
  for(byte i=2;i<54;i++){
    if(i!=53){    
      snprintf(buf, sizeof(buf), "%i", i);
      strcat(ret,buf);
      strcat(ret,":");
      snprintf(buf, sizeof(buf), "%i", digitalReadOutputPin(i));
      strcat(ret,buf);
      strcat(ret,",");    
    }else{
      snprintf(buf, sizeof(buf), "%i", i);
      strcat(ret,buf);
      strcat(ret,":");
      snprintf(buf, sizeof(buf), "%i", digitalReadOutputPin(i));
      strcat(ret,buf);
      strcat(ret,"</ds>");
    }
  }
  Serial.println(ret);
}
//states on pwm pins
void printPwmStates(){
  strcpy(ret,"<pwm>");
  for(byte i=0;i<sizeof(pwmPins);i++){
    if(i!=sizeof(pwmPins)-1){
      snprintf(buf, sizeof(buf), "%d", pwmPins[i]);
        strcat(ret,buf);
        strcat(ret,":");
        snprintf(buf, sizeof(buf), "%d", pwmValues[i]);
        strcat(ret,buf);
        strcat(ret,",");
    }else{
        snprintf(buf, sizeof(buf), "%d", pwmPins[i]);
        strcat(ret,buf);
        strcat(ret,":");
        snprintf(buf, sizeof(buf), "%d", pwmValues[i]);
        strcat(ret,buf);
        strcat(ret,"</pwm>");
    }
  }
  Serial.println(ret);
}
//states on analog pins
void printAnalogStates(){
  strcpy(ret,"<an>");
  for(byte i=0;i<sizeof(analogPins);i++){
    if(i!=sizeof(analogPins)-1){
      snprintf(buf,sizeof(buf),"%d",analogPins[i]);
      strcat(ret,buf);
      strcat(ret,":");
      snprintf(buf,sizeof(buf),"%d",analogRead(analogPins[i]));
      strcat(ret,buf);
      strcat(ret,",");
    }else{
      snprintf(buf,sizeof(buf),"%d",analogPins[i]);
      strcat(ret,buf);
      strcat(ret,":");
      snprintf(buf,sizeof(buf),"%d",analogRead(analogPins[i]));
      strcat(ret,buf);
      strcat(ret,"</an>");
    }
    delayMicroseconds(500);
  }
  Serial.println(ret);
}



