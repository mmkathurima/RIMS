/* This is a sample program.  You can save/compile/run it, 
   modify it first, or just load a different program.      */
   
/* This program demonstrates the use of the debug output
   facilities (puts, putc, puti, endl) included in RIMS.h  */

#include "rims.h"

int Flag = 0;
char Message[] = "Hello! ";

void TimerISR()
{
   Flag = 1;
}

int main()
{
   int i = 0;
   
   TimerSet(1000);//Set timer period to 1s
   TimerOn();
   
   while (1) {
      while (!Flag) {}
      Flag = 0;//Wait for timer period

      puts(Message);//Dump 'message' to debug output
      
      for(i = 0; i < strlen(Message); ++i)
         putc(Message[i]);//Same effect as puts() above
	  
	  puti(123);//Dump an integer value to debug output
	  
	  //putc('\n'); //Instead, use:
	  endl();
   }
}
