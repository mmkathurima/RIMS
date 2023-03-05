
/*
Consider an electronic doorbell, with a button connected to
A0, and B0 connecting to a bell. Consider the following timing features.
The minimum button press length considered is 400 ms. The minimum
separation between presses is 500 ms. The maximum latency between
a press and the start of the bell ringing should be 100 ms. When a valid
button press is detected, the bell should ring for 1 second and then stop
ringing until the next distinct button press
*/
#include "rims.h"

/*Define user variables for this state machine here. No functions; make them global.*/
unsigned char cnt;

unsigned char DB_Clk;
void TimerISR() {
   DB_Clk = 1;
}

enum DB_States { DB_WaitPress, DB_Ring } DB_State;

TickFct_Dorbell() {
   switch(DB_State) { // Transitions
      case -1:
         DB_State = DB_WaitPress;
         break;
         case DB_WaitPress: //wait on A0
         if (A0) {
            DB_State = DB_Ring;
            cnt = 0;
         }
         else if (!A0) {
            DB_State = DB_WaitPress;
         }
         break;
      case DB_Ring:
         if (!(cnt < 10)) {
            DB_State = DB_WaitPress;
         }
         else if (cnt<10) {
            DB_State = DB_Ring;
         }
         break;
      default:
         DB_State = DB_WaitPress;
   } // Transitions

   switch(DB_State) { // State actions
      case DB_WaitPress:
         B0=0;
         break;
      case DB_Ring: //ring for a second
         B0=1;

         cnt++;
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int periodDorbell = 100;
   TimerSet(periodDorbell);
   TimerOn();
   
   DB_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TickFct_Dorbell();
      while(!DB_Clk);
      DB_Clk = 0;
   } // while (1)
} // Main