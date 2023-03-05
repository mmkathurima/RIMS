

#include "rims.h"

/*Define User Variables and Functions For this State Machine Here.*/
const unsigned char Thresh = 85;

unsigned char SM1_Clk;
void TimerISR() {
   SM1_Clk = 1;
}

enum SM1_States { SM1_Normal, SM1_AboveThresh } SM1_State;

TickFct_SpeakerProject() {
   switch(SM1_State) { // Transitions
      case -1:
         SM1_State = SM1_Normal;
         break;
         case SM1_Normal:
         if (A > Thresh) {
            SM1_State = SM1_AboveThresh;
         }
         else if (!(A > Thresh)) {
            SM1_State = SM1_Normal;
         }
         break;
      case SM1_AboveThresh:
         if (!(A > Thresh)) {
            SM1_State = SM1_Normal;
         }
         else if (A > Thresh) {
            SM1_State = SM1_AboveThresh;
         }
         break;
      default:
         SM1_State = SM1_Normal;
   } // Transitions

   switch(SM1_State) { // State actions
      case SM1_Normal:
         B0 = 1;
         break;
      case SM1_AboveThresh:
         B0 = 0;
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int periodSpeakerProject = 1000; // 1000 ms default
   TimerSet(periodSpeakerProject);
   TimerOn();
   
   SM1_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TickFct_SpeakerProject();
      while(!SM1_Clk);
      SM1_Clk = 0;
   } // while (1)
} // Main