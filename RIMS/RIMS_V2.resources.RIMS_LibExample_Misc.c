/* This is a sample program.  You can save/compile/run it, 
   modify it first, or just load a different program.      */
   
/* This program demonstrates the use of miscellaneous
   library functions included in RIMS.h  */
   
#include "rims.h"

//Note the use of TimerXXX() functions, but the lack of TimerISR().
//When this is the case, TimerISR() is assumed to do nothing

int main()
{
   int i = 0;
   int PseudoRandom;
   
   ASSERT(A0 == 0);
   
   TimerSet(250);
   TimerOn();//Set an arbitrary timer period --
             //Short enough that it's likely this period
             //will have been exhausted many times by the time
             //the timer is read below

   puts("Please toggle A0 to ON...\n");
   
   while(!A0) {}//wait for the user to toggle the A0 switch
   PseudoRandom = TimerRead();//use the current timer tick as
                               //a source of entropy

   TimerOff();//not needed any more
   
   srand(PseudoRandom);//seed the random number generator with
                        //this pseudo-random value
   
   for(i = 0; i < 9; ++i){
	  int Random = rand();//rand() returns an unsigned integer
                           //in [0, 32767]
      Random -= 16384;//~50% chance of being negative IF rand()
                      //were statistically random (it's NOT)
      puti(Random);
	  puts(" is: ");
      if(abs(Random) != Random)
         puts("Negative\n");
      else
         puts("Positive\n");
   }   
}
