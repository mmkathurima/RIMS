/*
This code was automatically generated using the Riverside-Irvine State machine Builder tool
Version 2.6 --- 9/20/2013 17:40:8 PST
*/

#include "rims.h"

/*Define user variables and functions for this state machine here.*/
/*
   To use: enable wave input, set A0 to 1 to activate, the frequency in hertz will be displayed on B.
*/
unsigned short timeCnt; //amount of ticks signal is positive
unsigned short SAMPLE_RATE = 20;
unsigned short newFreq;
unsigned char SM1_Clk;
void TimerISR() {
   SM1_Clk = 1;
}

enum SM1_States { SM1_wait, SM1_WaitForNegAndCount, SM1_waitForPositive, SM1_waitRelease } SM1_State;

TickFct_FrequencyReader() {
   switch(SM1_State) { // Transitions
      case -1:
         SM1_State = SM1_wait;
         break;
         case SM1_wait: 
         if (A6 && A5) {
            SM1_State = SM1_waitForPositive;
         }
         else if (!(A6 && A5)) {
            SM1_State = SM1_wait;
         }
         break;
      case SM1_WaitForNegAndCount: 
         if (!A6 || A5) {
            SM1_State = SM1_waitRelease;
            newFreq = 1000/(timeCnt*SAMPLE_RATE);
            B = newFreq; 
             
         }
         else if (!(!A6 || A5 != 0)) {
            SM1_State = SM1_WaitForNegAndCount;
         }
         break;
      case SM1_waitForPositive: 
         if (A6 && !A5) {
            SM1_State = SM1_WaitForNegAndCount;
            timeCnt = 0;
         }
         else if (!(A6 && !A5)) {
            SM1_State = SM1_waitForPositive;
         }
         break;
      case SM1_waitRelease: 
         if (!A6) {
            SM1_State = SM1_wait;
         }
         else if (A6) {
            SM1_State = SM1_waitRelease;
         }
         break;
      default:
         SM1_State = SM1_wait;
   } // Transitions

   switch(SM1_State) { // State actions
      case SM1_wait:
         break;
      case SM1_WaitForNegAndCount:
         timeCnt++;
         break;
      case SM1_waitForPositive:
         break;
      case SM1_waitRelease:
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int periodFrequencyReader = 20;
   TimerSet(periodFrequencyReader);
   TimerOn();
   
   SM1_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TickFct_FrequencyReader();
      while(!SM1_Clk);
      SM1_Clk = 0;
   } // while (1)
} // Main