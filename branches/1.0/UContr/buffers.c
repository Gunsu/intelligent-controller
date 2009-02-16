#include "buffers.h"

unsigned short Buffer0Data [BUFFER_ADC_DATA_MAX_SIZE];
unsigned short Buffer1Data [BUFFER_ADC_DATA_MAX_SIZE];
unsigned short Buffer2Data [BUFFER_ADC_DATA_MAX_SIZE];
unsigned short Buffer3Data [BUFFER_ADC_DATA_MAX_SIZE];
/*
unsigned short Buffer4Data [BUFFER_ADC_DATA_MAX_SIZE];
unsigned short Buffer5Data [BUFFER_ADC_DATA_MAX_SIZE];
unsigned short Buffer6Data [BUFFER_ADC_DATA_MAX_SIZE];
unsigned short Buffer7Data [BUFFER_ADC_DATA_MAX_SIZE];
*/
unsigned char BufferInCommandData [BUFFER_IN_COMMAND_SIZE];

BufferStruct Buffers [BUFFERS_COUNT] = {
/*00*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, (unsigned char*)Buffer0Data},
/*01*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, (unsigned char*)Buffer1Data},
/*02*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, (unsigned char*)Buffer2Data},
/*03*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, (unsigned char*)Buffer3Data},
/*04*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, 0/*(unsigned char*)Buffer4Data*/},
/*05*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, 0/*(unsigned char*)Buffer5Data*/},
/*06*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, 0/*(unsigned char*)Buffer6Data*/},
/*07*/	{1, BUFFER_ADC_DATA_MAX_SIZE, 0, 0, 2, 0/*(unsigned char*)Buffer7Data*/},
/*08*/	{0, BUFFER_IN_COMMAND_SIZE, 0, 0, 1, BufferInCommandData}
};









