/*
This code was automatically generated using the Riverside-Irvine State machine Builder tool
Version 2.1 --- 7/6/2011 11:31:44 PST
*/

#include "rims.h"


//This sample is a simple producer/consumer example, containing two state machines. The producer will create items at 4x the speed of the consumer, thus eventually filling the entire queue.

#define SIZE 20

unsigned char queue[SIZE] = {0};
int front = 0, back = 0;

void push(unsigned char c) {
   if (front%SIZE == (back+1)%SIZE) {
      puts("Queue full\n");
      return;
   }
   queue[back] = c;
   back = (back+1)%SIZE;
}

unsigned char pop() {
   if (front == back) {
      puts("queue empty\n");
      return;
   }
   front = (front+1)%SIZE;
   return queue[(front-1)%SIZE];
}

unsigned char flag = 0;
void TimerISR() {
   flag = 1;
}

unsigned char rx_flag = 0;
void RxISR() {
   rx_flag = 1;
}
const unsigned long int numTasks = 2;

const unsigned long int P_period = 250;
const unsigned long int C_period = 1000;

const unsigned long int GCD = 250;

typedef struct task {
   int state;
   unsigned long int period;
   unsigned long int elapsedTime;
   int (*TickFct)(int);
} task;


enum P_States { P_s0 } P_State;
int P_Tick(int state) {
   static int i = 0;
switch(state) { // Transitions
      case -1:
         state = P_State = P_s0;
         break;
      case P_s0:
         if (1) {
            state = P_State = P_s0;
         }
         break;
      default:
         state = -1;
      } // Transitions

   switch(state) { // State actions
      case P_s0:
         push(i++);
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
   return state;
}


enum C_States { C_s0 } C_State;
int C_Tick(int state) {
   
switch(state) { // Transitions
      case -1:
         state = C_State = C_s0;
         break;
      case C_s0:
         if (1) {
            state = C_State = C_s0;
         }
         break;
      default:
         state = -1;
      } // Transitions

   switch(state) { // State actions
      case C_s0:
         puti(pop());
putc('\n');
         break;
      default: // ADD default behaviour below
         break;
   } // State actions
   return state;
}

int main() {
   unsigned long int i; //scheduler for-loop iterator

   task tasks[2];

   tasks[0].state = -1;
   tasks[0].period = 250;
   tasks[0].elapsedTime = 250;
   tasks[0].TickFct = &P_Tick;

   tasks[1].state = -1;
   tasks[1].period = 1000;
   tasks[1].elapsedTime = 1000;
   tasks[1].TickFct = &C_Tick;

   TimerSet(GCD);
   TimerOn();
   UARTOn();
while(1) {
      for ( i = 0; i < numTasks; ++i ) {
         if ( tasks[i].elapsedTime == tasks[i].period ) {
            tasks[i].state = tasks[i].TickFct(tasks[i].state);
            tasks[i].elapsedTime = 0;
         }
         tasks[i].elapsedTime += GCD;
      }
      while(!flag);
      flag = 0;
   }
   return 0;
}