
/*
A pulse width modulator (PWM) is a programmable component that generates pulses
to achieve a specified period and duty cycle. Assume a PWM allows a period that is a multiple
of 1 second (e.g., 2 seconds) and a duty cycle that is a multiple of 10% (e.g., 40%). To
implement this PWM, a synchSM can be defined with a period of 100 ms. Variables H and
L store the number of synchSM ticks to hold the signal high and low, respectively; for 2
seconds and 40%, H would be 2000ms*0.40 = 800 ms, meaning 8 ticks, and L would be
12
*/
#include "rims.h"

/*Define user variables for this state machine here. No functions; make them global.*/
unsigned char H;
unsigned char L;
unsigned char X;

unsigned char SM1_Clk;
void TimerISR() {
   SM1_Clk = 1;
}

enum SM1_States {SM1_Init, SM1_PwmH, SM1_PwmL } SM1_State;

TickFct_Pwm() {
   switch(SM1_State) { // Transitions
      case -1:
         SM1_State = SM1_Init;
         break;
         case SM1_Init:
            SM1_State = SM1_PwmH;
            X = 0;
         
         break;
      case SM1_PwmH://hold high H cycles
         if (X >= H) {
            SM1_State = SM1_PwmL;
            X=0;
         }
         else if (X<H) {
            SM1_State = SM1_PwmH;
         }
         break;
      case SM1_PwmL: //hold low L cycles
         if (X < L) {
            SM1_State = SM1_PwmL;
         }
         else if (!(X < L)) {
            SM1_State = SM1_PwmH;
            X = 0;
         }
         break;
      default:
         SM1_State = SM1_Init;
   } // Transitions

   switch(SM1_State) { // State actions
      case SM1_Init:
         B0 = 0;

         H = 8;

         L = 12;
         break;
      case SM1_PwmH:
         B0 = 1;

         X = X + 1;
         break;
      case SM1_PwmL:
         B0 = 0;

         X = X + 1;
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int periodPwm = 100;
   TimerSet(periodPwm);
   TimerOn();
   
   SM1_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      TickFct_Pwm();
      while(!SM1_Clk);
      SM1_Clk = 0;
   } // while (1)
} // Main