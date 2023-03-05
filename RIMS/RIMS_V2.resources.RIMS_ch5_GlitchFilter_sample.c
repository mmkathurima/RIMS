

/*
A synchSM whose only purpose is to condition the signal on input A0
coming from a motion sensor (A0=1 should mean motion is sensed), into a clean signal on
output B0 such that B0=1 indicates motion.
*/


#include "rims.h"

/*Define user variables for this state machine here. No functions; make them global.*/

unsigned char GF_Clk;
void TimerISR() {
   GF_Clk = 1;
}

enum GF_States { GF_Wait, GF_FilterGlitch, GF_Motion } GF_State;

TickFct_GlitchFilter() {
   switch(GF_State) { // Transitions
      case -1:
         GF_State = GF_Wait;
         break;
         case GF_Wait: //wait on A0
         if (A0) {
            GF_State = GF_FilterGlitch;
         }
         else if (!A0) {
            GF_State = GF_Wait;
         }
         break;
      case GF_FilterGlitch:
         if (A0) { //there is still movement, go to motion state
            GF_State = GF_Motion;
         }
         else if (!A0) {
            GF_State = GF_Wait;
         }
         break;
      case GF_Motion: //keep outputting motion signal util A0 goes to 0
         if (A0) {
            GF_State = GF_Motion;
         }
         else if (!A0) {
            GF_State = GF_Wait;
         }
         break;
      default:
         GF_State = GF_Wait;
   } // Transitions

   switch(GF_State) { // State actions
      case GF_Wait:
         B0=0;
         break;
      case GF_FilterGlitch:
         break;
      case GF_Motion:
         B0=1;
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int periodGlitchFilter = 100;
   TimerSet(periodGlitchFilter);
   TimerOn();
   
   GF_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TickFct_GlitchFilter();
      while(!GF_Clk);
      GF_Clk = 0;
   } // while (1)
} // Main