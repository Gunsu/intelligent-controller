#ifndef __BUFFERS__H__
#define __BUFFERS__H__

#define BUFFERS_COUNT					9
// 16 элементов по 2 байта
#define BUFFER_ADC_DATA_MAX_SIZE		16
// 16 элементов по 1 байту
#define BUFFER_IN_COMMAND_SIZE		16
#define BUFFER_IN_COMMAND_ID			8
// индекс буфера первого канала АЦП
#define BUFFER_ADC_FIRST_BUF_INDEX	0	

typedef struct
{
	unsigned char	NeedConvertToASCII;
	unsigned char	MaxSize; 		// максимальный размер буфера в элементах
	unsigned char	Size;				// текущий размер буфера в элементах
	unsigned char 	Pos;				// позиция в буфере (номер элемента)
	unsigned char	ElementSize;	// размер элемента в байтах
	unsigned char* Data;
} BufferStruct;

extern BufferStruct Buffers[];

#endif






