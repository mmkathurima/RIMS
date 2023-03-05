

/*
An applause-meter system intended for a game show. A sound sensor
measures sound on a scale of 0 to 7 (0 means quiet, 7 means loud), outputting a three-
bit binary number, connected to RIM's A2-A0. A button connected to A3 can be pressed
by the game show host to save (when A3 rises) the current sound level, which will then
be displayed on B. The system's behavior can be captured as an SM with a variable, as
shown.
*/

#include "rims.h"

/*Define user variables for this state machine here. No functions; make them global.*/
static unsigned char levCurr = 0;

unsigned char SM1_Clk;
void TimerISR() {
   SM1_Clk = 1;
}

enum SM1_States { SM1_Init, SM1_WaitA3, SM1_SaveCurr, SM1_s4 } SM1_State;

TickFct_State_Machine_1() {
   switch(SM1_State) { // Transitions
      case -1:
         SM1_State = SM1_Init;
         break;
         case SM1_Init:
			SM1_State = SM1_WaitA3;
         
         break;
      case SM1_WaitA3: //wait on host to press save button
         if (A3) {
            SM1_State = SM1_SaveCurr;
         }
         else if (!A3) { //host pressed the button, go to save state
            SM1_State = SM1_WaitA3;
         }
         break;
      case SM1_SaveCurr: //value saved, transition into waiting for release
         SM1_State = SM1_s4;
         
         break;
      case SM1_s4: //wait for button release to avoid multiple accidental saves
         if (!A3) {
            SM1_State = SM1_WaitA3;
         }
         else if (A3) {
            SM1_State = SM1_s4;
         }
         break;
      default:
         SM1_State = SM1_Init;
   } // Transitions

   switch(SM1_State) { // State actions
      case SM1_Init:
         levCurr = 0;

         B = levCurr;
         break;
      case SM1_WaitA3:
         break;
      case SM1_SaveCurr:
         levCurr = A & 0x07; // 0000 01111
							//save the value in levCurr in the state action

         B = levCurr;
         break;
      case SM1_s4:
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int periodState_Machine_1 = 1000; // 1000 ms default
   TimerSet(periodState_Machine_1);
   TimerOn();
   
   SM1_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TickFct_State_Machine_1();
      while(!SM1_Clk);
      SM1_Clk = 0;
   } // while (1)
} // Main