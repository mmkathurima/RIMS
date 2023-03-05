/*
This code was automatically generated using the Riverside-Irvine State machine Builder tool
Version 2.1 --- 1/28/2010 14:44:53 PST
*/

#include "rims.h"

/*Global C code is defined at global scope in generated code*/

int SM_Clk;
void TimerISR() {
   SM_Clk = 1;
}

void SM_ClkTick() {
   while(!SM_Clk);
   SM_Clk = 0;
}

enum SM_States { SM_s0, SM_s1, SM_s2 } SM_State;

int main() {

   /*Local C code is located inside main() in generated code*/

   const int SM_Period = 1000; // 1000 ms default
   TimerSet(SM_Period);
   TimerOn();
   
   SM_State = SM_s0; // Initial state
   B = 0; // Init outputs

   while(1) {
      switch(SM_State) { // State actions
         case SM_s0:
            break;
         case SM_s1:
            break;
         case SM_s2:
            B++;
            break;
         default: // ADD default behaviour below
         break;
      } // State actions

      SM_ClkTick();

      switch(SM_State) { // Transitions
         case SM_s0:
            if (A0 && !A1) {
               SM_State = SM_s1;
            }
            else if (!A0 && !A1) {
               SM_State = SM_s0;
            }
            break;
         case SM_s1:
            if (A0 && A1) {
               SM_State = SM_s2;
            }
            else if (A0 && !A1) {
               SM_State = SM_s1;
            }
            else {
               SM_State = SM_s0;
            }
            break;
         case SM_s2:
            if (1) {
               SM_State = SM_s0;
            }
            break;
         default:
            SM_State = SM_s0;
      } // Transitions
   } // while (1)
} // Main