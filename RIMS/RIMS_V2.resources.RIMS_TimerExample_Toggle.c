/*
	RIMS_TimerExample_Toggle.c
	A simple program that toggles an led on off after a full button press( 0 -> 1 -> 0)
*/

#include "rims.h"

unsigned char TG_Clk;
void TimerISR() {
   TG_Clk = 1;
}

enum TG_States { TG_s0, TG_s1, TG_s2, TG_s3 } TG_State;

TG_Tick() {
   switch(TG_State) { // Transitions
      case -1:
         TG_State = TG_s0;
         break;
         case TG_s0: //wait for press
         if (A0) {
            TG_State = TG_s1;
         }
         else if (!A0) {
            TG_State = TG_s0;
         }
         break;
      case TG_s1: //wait for release
         if (!A0) {
            TG_State = TG_s2;
         }
         else if (A0) {
            TG_State = TG_s1;
         }
         break;
      case TG_s2: //wait for press
         if (A0) {
            TG_State = TG_s3;
         }
         else if (!A0) {
            TG_State = TG_s2;
         }
         break;
      case TG_s3: //wait for release
         if (!A0) {
            TG_State = TG_s0;
         }
         else if (A0) {
            TG_State = TG_s3;
         }
         break;
      default:
         TG_State = TG_s0;
   } // Transitions

   switch(TG_State) { // State actions
      case TG_s0: //init output/off state
         B0 = 0;
         break;
      case TG_s1: //waiting for release
         break;
      case TG_s2: //they released the button, turn on the light
         B0 = 1;
         break;
      case TG_s3: //waiting for release, once they do we will go to s0 and turn off the led
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int TG_Period = 200; // .2s
   TimerSet(TG_Period); //set the period
   TimerOn(); //turn on timer, will fire every .2s
   
   TG_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TG_Tick(); //call tick
      while(!TG_Clk); //wait on isr
      TG_Clk = 0;
   } // while (1)
} // Main