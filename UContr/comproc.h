#ifndef __COM_PROC__H__
#define __COM_PROC__H__

#define LOCAL_MEMORY_SIZE						64
#define MASK_ANY_SYMBOL							'_'
#define EOC											'\r'

#define INTERNAL_COMMAND_PREFIX				'@'
#define INTERNAL_ERROR_COMMAND				"@ERROR"
#define INTERNAL_ERROR_COMMAND_LENGTH		6

#define IN_COMMAND_STATE_PROCESSED			0
#define IN_COMMAND_STATE_READY_TO_PROCESS	1

#define LAST_ERROR_INVALID_PARAM				1
#define LAST_ERROR_OUT_OF_RANGE_PARAM		2
#define LAST_ERROR_INVALID_CRC				3
#define LAST_ERROR_ADC_NOT_READY				4

typedef struct
{
	unsigned char Memory [LOCAL_MEMORY_SIZE];
	unsigned char InCommandState;
	unsigned char LastError;
} ComProcDataStruct;

extern ComProcDataStruct ComProcData;

void ComProcInit (void);
unsigned char ComProcProcessInCommand (void);

#endif



























