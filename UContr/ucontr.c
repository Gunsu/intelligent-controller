#include <avr\io.h>
#include <avr\interrupt.h>
#include "comproc.h"
#include "hal.h"
#include "buffers.h"

int main(void)
{		
	ComProcInit();
	HALInit();	
 	
 	while (1)
 	{
		if (ComProcData.InCommandState == IN_COMMAND_STATE_READY_TO_PROCESS)
		{
			ComProcProcessInCommand();
			ComProcData.InCommandState = IN_COMMAND_STATE_PROCESSED;
			Buffers[BUFFER_IN_COMMAND_ID].Size = 0;
		}
   }	

	return 0;
}






























