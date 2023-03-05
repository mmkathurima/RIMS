/* This is a sample program.  You can compile/run it, 
       modify it first, or just load a different program. */

/* This program interprets the input pins as two sets of
4-bit unsigned integers: {A3, A2, A1, A0} and {A7, A6, A5, A4}.
Output pins are updated as quickly as possible to represent
the sum of these two unsigned integers.*/

#include "rims.h"

char Op1, Op2;

int main()
{
   while (1) {
      Op1 = A & 0x0F;
      Op2 = (A & 0xF0) >> 4;
      B = Op1 + Op2;
   }
}