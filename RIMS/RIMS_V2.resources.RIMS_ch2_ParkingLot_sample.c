/*
        A parking lot has eight parking spaces, each with a sensor connected to input A. The following program sets B to the number of occupied spaces, by counting the number of 1s using the GetBit function:
*/
#include "RIMS.h"

unsigned char GetBit(unsigned char x, unsigned char k) {
   return ((x & (0x01 << k)) != 0);
}

int main()
{
   unsigned char i;
   unsigned char cnt;
   while (1) {
      cnt=0;
      for (i=0; i<8; i++) {
         if (GetBit(A, i)) { //each bit of A is connected to a sensor, getting the value of each bit gives us the amount of occupied spaces.
            cnt++;
         }
      }
      B = cnt; 
   }
}