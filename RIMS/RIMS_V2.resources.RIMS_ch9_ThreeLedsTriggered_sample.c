/*
	Uses task struct and gcd of task periods to execute two tasks with different periods using the same timer.
	The tasks are the 3 led and blinking led tasks from before.
*/

#include "RIMS.h"

volatile unsigned char TimerFlag = 0; 

void TimerISR(void) {
   TimerFlag = 1;
}

typedef struct {
   int state;
   unsigned long period;
   unsigned long elapsedTime;
   int (*TickFct)(int);
} task_t;

enum BL_States { BL_LEDOFF, BL_LEDON };

int BL_Tick(int state) {
   switch(state) { // Transitions
      case -1:
         state = BL_LEDOFF;
         break;
      case BL_LEDOFF:
         state = BL_LEDON;
         break;
      case BL_LEDON:
         state = BL_LEDOFF;
         break;
      default:
         state = -1;
         break;
   }

   switch(state) { // State actions
      case BL_LEDOFF:
         B0 = 0;
         break;
      case BL_LEDON:
         B0 = 1;
         break;
      default:
         break;
   }
   return state;
}

enum TL_States { TL_ONE, TL_TWO, TL_THREE };

int TL_Tick(int state) {
   switch(state) { //Transitions
      case -1:
         state = TL_ONE;
         break;
      case TL_ONE:
         state = TL_TWO;
         break;
      case TL_TWO:
         state = TL_THREE;
         break;
      case TL_THREE:
         state = TL_ONE;
         break;
      default:
         state = -1;
         break;
      }

   switch(state) { //State actions
      case TL_ONE:
         B5 = 1;
         B6 = 0;
         B7 = 0;
         break;
      case TL_TWO:
         B5 = 0;
         B6 = 1;
         B7 = 0;
         break;
      case TL_THREE:
         B5 = 0;
         B6 = 0;
         B7 = 1;
         break;
      default:
         break;
   }
   return state;
}

int main()
{
   const unsigned long BL_period = 1500;
   const unsigned long TL_period = 500;

   const unsigned long GCD = 500;

   unsigned char i; // Index for scheduler's for loop

   static task_t        task1,  task2;
   task_t *tasks[] = { &task1, &task2 };
   const unsigned short numTasks = sizeof(tasks) / sizeof(task_t*);

   task1.state       = -1;
   task1.period      = BL_period;
   task1.elapsedTime = BL_period;
   task1.TickFct     = &BL_Tick;
   
   task2.state       = -1;
   task2.period      = TL_period;
   task2.elapsedTime = TL_period;
   task2.TickFct     = &TL_Tick;

   TimerSet(GCD);
   TimerOn();
   
   while(1) {
      for ( i = 0; i < numTasks; ++i ) {
         if ( tasks[i]->elapsedTime == tasks[i]->period ) { 
            // Task is ready to tick, so call its tick function
            tasks[i]->state = tasks[i]->TickFct(tasks[i]->state);
            tasks[i]->elapsedTime = 0; // Reset the elapsed time
         }
         tasks[i]->elapsedTime += GCD; // Account for below wait
      }
      while(!TimerFlag); // Wait for next timer tick
      TimerFlag = 0;
   }
}