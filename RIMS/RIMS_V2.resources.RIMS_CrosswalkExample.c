/*
This code was automatically generated using the Riverside-Irvine State machine Builder tool
Version 2.3 --- 6/28/2010 13:3:34 PST
*/

#include "rims.h"

/* This is a sample crosswalk design. A non jaywalking pedestrian would push the button attached to A0 to inform the system that they desire to cross the street.  The state machine then gives the walk signal by holding the output attached to B0 high for 3 seconds.  Following the walk signal, the caution signal is given by flashing the output every 500 milliseconds for 5 seconds.  The wait signal is finally given by driving the output low until the button is pressed again.
*/

#define BUTTON A0
#define SIGNAL B0

unsigned char counter = 0;
unsigned char flash_count = 0;

unsigned char CW_Clk;
void TimerISR() {
   CW_Clk = 1;
}

enum CW_States { CW_NOWALK, CW_WALK, CW_CAUTION1, CW_CAUTION2 } CW_State;

CW_Tick() {
   switch(CW_State) { // Transitions
      case -1:
         CW_State = CW_NOWALK;
         break;
         case CW_NOWALK:
         if (BUTTON) {
            CW_State = CW_WALK;
            counter = 0;
         }
         else if (!BUTTON) {
            CW_State = CW_NOWALK;
         }
         break;
      case CW_WALK:
         if (counter == 60) {
            CW_State = CW_CAUTION1;
            counter = 0;
         }
         else if (counter < 60) {
            CW_State = CW_WALK;
         }
         break;
      case CW_CAUTION1:
         if (counter == 10) {
            CW_State = CW_CAUTION2;
            counter = 0;
         }
         else if (counter < 10) {
            CW_State = CW_CAUTION1;
         }
         break;
      case CW_CAUTION2:
         if (counter < 10) {
            CW_State = CW_CAUTION2;
         }
         else if (counter == 10 && flash_count < 4) {
            CW_State = CW_CAUTION1;
            counter = 0;
            flash_count += 1;
         }
         else {
            CW_State = CW_NOWALK;
            flash_count = 0;
         }
         break;
      default:
         CW_State = CW_NOWALK;
   } // Transitions

   switch(CW_State) { // State actions
      case CW_NOWALK:
         SIGNAL = 0;
         break;
      case CW_WALK:
         counter += 1;
         SIGNAL = 1;
         break;
      case CW_CAUTION1:
         counter += 1;
         SIGNAL = 0;
         break;
      case CW_CAUTION2:
         counter += 1;
         SIGNAL = 1;
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int CW_Period = 100;
   TimerSet(CW_Period);
   TimerOn();
   
   CW_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      CW_Tick();
      while(!CW_Clk);
      CW_Clk = 0;
   } // while (1)
} // Main