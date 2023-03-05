/*
This code was automatically generated using the Riverside-Irvine State machine Builder tool
Version 2.1 --- 6/28/2010 13:23:10 PST
*/

#include "rims.h"

#define SIZE 29

char* message = "UCR Microcontroller Simulator";
int i = 0;


unsigned char MES_Clk;
void TimerISR() {
   MES_Clk = 1;
}

unsigned char MES_Rx_Flag = 0;
void RxISR() {
   MES_Rx_Flag = 1;
}

enum MES_States { MES_Wait, MES_Transmit, MES_Increment } MES_State;

MES_Tick() {
   switch(MES_State) { // Transitions
      case -1:
         MES_State = MES_Wait;
         break;
         case MES_Wait:
         if (TxReady) {
            MES_State = MES_Transmit;
         }
         else if (!TxReady) {
            MES_State = MES_Wait;
         }
         break;
      case MES_Transmit:
         if (1) {
            MES_State = MES_Increment;
         }
         break;
      case MES_Increment:
         if (1) {
            MES_State = MES_Wait;
         }
         break;
      default:
         MES_State = MES_Wait;
   } // Transitions

   switch(MES_State) { // State actions
      case MES_Wait:
         break;
      case MES_Transmit:
         T = message[i];
         B = message[i];
         break;
      case MES_Increment:
         if ( i < SIZE - 1 ) ++i;
         else i = 0;
         break;
      default: // ADD default behaviour below
      break;
   } // State actions

}

int main() {

   const unsigned int MES_Period = 200;
   TimerSet(MES_Period);
   TimerOn();
   UARTOn();


   MES_State = -1; // Initial state
   B = 0; // Init outputs

   while(1) {
      MES_Tick();
      while(!MES_Clk);
      MES_Clk = 0;
   } // while (1)
} // Main