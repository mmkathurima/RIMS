/* This is a sample program.  You can compile/run it,
   modify it first, or just load a different program. */

/*This program takes all UART input received and transmits 
it right back out.  Entering input to the UART input buffer
will result in that same input appearing in the UART output
history.*/

#include "rims.h"

int Flag;
void RxISR() { 
   Flag = 1; //This is executed when the UART receives new data
}

int main()
{
   UARTOn();//Turn on UART
   while(1){
      while(!Flag);//wait until we receive a full character
      Flag = 0;
      while (!TxReady);//and make sure we're clear to transmit
      T = R;
   }
}
