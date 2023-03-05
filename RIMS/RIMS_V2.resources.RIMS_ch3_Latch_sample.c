

/*
SM named Latch (abbreviated as LA), which saves (or "latches") the value of A1 onto
B0 whenever A0 is 1
*/

#include "rims.h"

/*Define User Variables and Functions For this State Machine Here.*/

unsigned char SM1_Clk;
void TimerISR() {
   SM1_Clk = 1;
}

enum SM1_States { SM1_s1, SM1_s2 } SM1_State;

TickFct_State_Machine_1() {
   switch(SM1_State) { // Transitions
      case -1:
         SM1_State = SM1_s1;
         break;
         case SM1_s1: //wait on A0
         if (!A0) {
            SM1_State = SM1_s1;
         }
         else if (A0) {
            SM1_State = SM1_s2;
         }
         break;
      case SM1_s2: //wait on release 
         if (A0) {
            SM1_State = SM1_s2;
         }
         else if (!A0) {
            SM1_State = SM1_s1;
         }
         break;
      default:
         SM1_State = SM1_s1;
   } // Transitions

   switch(SM1_State) { // State actions
      case SM1_s1:
         break;
      case SM1_s2: //A0 pressed, save A1
         B0 = A1;
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