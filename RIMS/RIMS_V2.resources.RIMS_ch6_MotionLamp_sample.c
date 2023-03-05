

/*
Consider a motion-triggered lamp system
with a motion sensor connected to A0. The system defines motion as
A0=1 for two consecutive 200 ms samples. When motion is detected,
the system should illuminate a lamp (by setting B1 to 1), keeping the
lamp on for 10 seconds past the last detected motion. The system
should also blink a small LED (connected to B0) for 200 ms on and
off while motion is detected.

The DetectMotion task detects motion and informs the other tasks of the detected motion
by setting a shared variable mtn. The other two tasks read mtn and respond accordingly:
BlinkLed blinks the LED, and IlluminateLamp turns on the lamp for the required time.

*/

#include "rims.h"


/*This code will be shared between state machines.*/
volatile unsigned char mtn = 0;
unsigned char TimerFlag = 0;
void TimerISR() {
   TimerFlag = 1;
}


enum DM_States { DM_DM0, DM_DM1, DM_DM2 } DM_State;

TickFct_DetectMotion() {
   /*VARIABLES MUST BE DECLARED STATIC*/
/*e.g., static int x = 0;*/
/*Define user variables for this state machine here. No functions; make them global.*/
switch(DM_State) { // Transitions
      case -1:
         DM_State = DM_DM0;
         break;
         case DM_DM0:
         if (A0) {//detected motion
            DM_State = DM_DM1;
         }
         else if (!A0) {
            DM_State = DM_DM0;
         }
         break;
      case DM_DM1:
         if (A0) { //motion still happening, it must be real
            DM_State = DM_DM2;
         }
         else if (!A0) {
            DM_State = DM_DM0;
         }
         break;
      case DM_DM2:
         if (A0) {//wait for motion to stop
            DM_State = DM_DM2;
         }
         else if (!A0) {
            DM_State = DM_DM0;
         }
         break;
      default:
         DM_State = DM_DM0;
      } // Transitions

   switch(DM_State) { // State actions
         case DM_DM0:
         mtn = 0;
         break;
      case DM_DM1:
         break;
      case DM_DM2:
         mtn = 1;
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
}

enum IL_States { IL_IL0, IL_IL1, IL_IL2 } IL_State;

TickFct_IlluminateLamp() {
   /*VARIABLES MUST BE DECLARED STATIC*/
/*e.g., static int x = 0;*/
/*Define user variables for this state machine here. No functions; make them global.*/
unsigned char cnt; // bit
switch(IL_State) { // Transitions
      case -1:
         IL_State = IL_IL0;
         break;
         case IL_IL0:
         if (mtn) {
            IL_State = IL_IL1;
         }
         else if (!mtn) {
            IL_State = IL_IL0;
         }
         break;
      case IL_IL1:
         if (!mtn) { //keep lamp on as long as motion is detected
            IL_State = IL_IL2;
            cnt = 0;
         }
         else if (mtn) {
            IL_State = IL_IL1;
         }
         break;
      case IL_IL2://keep on for an extra 10 seconds
         if (mtn) {
            IL_State = IL_IL1;
         }
         else if (!mtn && cnt < 50) {
            IL_State = IL_IL2;
         }
         else {
            IL_State = IL_IL0;
         }
         break;
      default:
         IL_State = IL_IL0;
      } // Transitions

   switch(IL_State) { // State actions
         case IL_IL0:
         B1=0;
         break;
      case IL_IL1:
         B1=1;
         break;
      case IL_IL2:
         cnt++;
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
}

enum BL_States { BL_BL0, BL_BL1 } BL_State;

TickFct_BlinkLed() {
   /*VARIABLES MUST BE DECLARED STATIC*/
/*e.g., static int x = 0;*/
/*Define user variables for this state machine here. No functions; make them global.*/
switch(BL_State) { // Transitions
      case -1:
         BL_State = BL_BL0;
         break;
         case BL_BL0: //blink off and on as long as motion is detected
         if (mtn) {
            BL_State = BL_BL1;
         }
         else if (!mtn) {
            BL_State = BL_BL0;
         }
         break;
      case BL_BL1:
            BL_State = BL_BL0;
         
         break;
      default:
         BL_State = BL_BL0;
      } // Transitions

   switch(BL_State) { // State actions
         case BL_BL0:
         B0=0;
         break;
      case BL_BL1:
         B0=1;
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
}
int main() {
   B = 0; //Init outputs
   TimerSet(200);
   TimerOn();
   DM_State = -1;
   IL_State = -1;
   BL_State = -1;
   while(1) {
      TickFct_DetectMotion();
      TickFct_IlluminateLamp();
      TickFct_BlinkLed();
      while (!TimerFlag);
      TimerFlag = 0;
   }
}