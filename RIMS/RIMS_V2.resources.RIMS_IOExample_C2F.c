/* This is a sample program.  You can compile/run it, 
       modify it first, or just load a different program. */

/* This program interprets the input pins as a temperature in
Celsius and outputs the corresponding Fahrenheit temperature.*/

#include "rims.h"

#define C_i A
#define F_o B

int main()
{
   while (1) {
      F_o = ((9 * C_i) / 5) + 32;
      //Note the order of operations.  If it were written:
      //   F_o = ((9 / 5) * C_i) + 32
      //as one would likely try first, integer math would ensure
      //the output is not what would be expected
	  
	  
      //Also note the 'wrap around' above values of 124C
   }
}