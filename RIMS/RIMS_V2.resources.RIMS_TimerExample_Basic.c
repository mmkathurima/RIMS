/* This is a sample program.  You can compile/run it, 
     modify it first, or just load a different program. */

/*This program will continuously wait 5s, assign A to B,
then repeat.                                            */

#include "rims.h"

int Flag = 0;

void TimerISR() {
   Flag = 1;
}

int main()
{
   const int Period = 5000; //5s
   TimerSet(Period);//Set timer period
   TimerOn();//Turn timer on

   B = 0;//Initialize output

   while (1) {
      while(!Flag) {}
      Flag = 0;//wait for a timer period to elapse
	  B = A;//assign input to output
   }
}