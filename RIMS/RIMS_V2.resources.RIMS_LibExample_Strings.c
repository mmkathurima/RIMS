/* This is a sample program.  You can save/compile/run it, 
   modify it first, or just load a different program.      */
   
/* This program demonstrates the use of the string facilities
   (strlen, strncmp, memcpy) included in RIMS.h            */
 
#include "rims.h"

#define BUFFER_SIZE 256

int Flag = 0;
char Message[] = "TestString\n";
char Buffer[BUFFER_SIZE];

int main()
{
   puts(Message);//Display the string once
   
   memset(Buffer, 0, BUFFER_SIZE);//'Zero out' receiving buffer
   memcpy(Buffer, Message, strlen(Message) + 1);
   //Copy strlen(message) + 1 bytes from 'message' to 'buffer'
   
   puts(Buffer);
   
   if(strncmp(Message, Buffer, strlen(Buffer)) != 0)
      puts("The strings don't match!");
   else
      puts("The strings match!");
}
