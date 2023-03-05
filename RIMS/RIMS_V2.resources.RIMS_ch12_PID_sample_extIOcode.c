// C statements describing external system.
// Statements can READ RIMS outputs B and/or B0, B1, ..., B7.
// Statements can WRITE RIMS inputs A and/or A0, A1, ..., A7.
// Such writes override any manual configurations of RIMS inputs.
// Statements CANNOT READ RIMS inputs A and/or A0, A1, ..., A7.
// Statements CANNOT WRITE RIMS inputs B and/or B0, B1, ..., B7.

#include "rims.h"
#define Actuator B
short flame = 0;
long flameConstant = 10;
long offset = 4;
long actuatorMax = 200;

unsigned char SM1_Clk;
void TimerISR() {
   SM1_Clk = 1;
}

enum SM1_States { SM1_Init, SM1_Burn } SM1_State;

TickFct_Heater() {
   switch(SM1_State) { // Transitions
      case -1:
         SM1_State = SM1_Init;
         break;
         case SM1_Init:
         if (1) {
            SM1_State = SM1_Burn;
         }
         break;
      case SM1_Burn:
         if (1) {
            SM1_State = SM1_Burn;
         }
         break;
      default:
         SM1_State = SM1_Init;
   } // Transitions

   switch(SM1_State) { // State actions
      case SM1_Init:
         flame = 0;
         break;
      case SM1_Burn:
         flame += ((flameConstant*Actuator)/actuatorMax)-offset;
         if(flame < 0) flame = 0;
         if(flame > 100) flame = 100;
         A = (A & 0x0F)|((flame/10)<<4);
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}
int main()
{
   const unsigned int periodHeater_Ctrl = 1000;
   TimerSet(periodHeater_Ctrl);
   TimerOn();
   
   SM1_State = -1; // Initial state

   while(1) {
      TickFct_Heater();
      while(!SM1_Clk);
      SM1_Clk = 0;
   }
}
