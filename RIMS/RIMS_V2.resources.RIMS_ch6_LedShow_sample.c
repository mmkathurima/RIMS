

/*
A system named LedShow may blink
an LED connected to B0 on for 500 ms and off for 500 ms, repeatedly. That same system
may also light three LEDs connected to B7B6B5 in the sequence 001, 010, and 100, 500
ms each, repeatedly.
*/
#include "rims.h"


/*This code will be shared between state machines.*/
unsigned char TimerFlag = 0;
void TimerISR() {
   TimerFlag = 1;
}


enum BL_States { BL_LedOff, BL_LedOn } BL_State;

TickFct_BlinkLed() {
   /*VARIABLES MUST BE DECLARED STATIC*/
/*e.g., static int x = 0;*/
/*Define user variables for this state machine here. No functions; make them global.*/
switch(BL_State) { // Transitions
      case -1:
         BL_State = BL_LedOff;
         break;
         case BL_LedOff:
            BL_State = BL_LedOn;
         
         break;
      case BL_LedOn:
            BL_State = BL_LedOff;
         
         break;
      default:
         BL_State = BL_LedOff;
      } // Transitions

   switch(BL_State) { // State actions
         case BL_LedOff:
         B0=0;
         break;
      case BL_LedOn:
         B0=1;
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
}

enum TL_States { TL_T0, TL_T1, TL_T2 } TL_State;

TickFct_ThreeLeds() {
   /*VARIABLES MUST BE DECLARED STATIC*/
/*e.g., static int x = 0;*/
/*Define user variables for this state machine here. No functions; make them global.*/
switch(TL_State) { // Transitions
      case -1:
         TL_State = TL_T0;
         break;
         case TL_T0: 
            TL_State = TL_T1;
         
         break;
      case TL_T1:
            TL_State = TL_T2;
        
         break;
      case TL_T2:
            TL_State = TL_T0;
         
         break;
      default:
         TL_State = TL_T0;
      } // Transitions

   switch(TL_State) { // State actions
         case TL_T0: //100
         B5=1;
		B6=0;
		B7=0;
         break;
      case TL_T1: //010
         B5=0;
		B6=1;
		B7=0;
         break;
      case TL_T2: //001
         B5=0;
		B6=0;
		B7=1;
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
}
int main() {
   B = 0; //Init outputs
   TimerSet(500);
   TimerOn();
   BL_State = -1;
   TL_State = -1;
   while(1) {
      TickFct_BlinkLed();
      TickFct_ThreeLeds();
      while (!TimerFlag);
      TimerFlag = 0;
   }
}