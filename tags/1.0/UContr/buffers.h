#ifndef __BUFFERS__H__
#define __BUFFERS__H__

#define BUFFERS_COUNT					9
// 16 ��������� �� 2 �����
#define BUFFER_ADC_DATA_MAX_SIZE		16
// 16 ��������� �� 1 �����
#define BUFFER_IN_COMMAND_SIZE		16
#define BUFFER_IN_COMMAND_ID			8
// ������ ������ ������� ������ ���
#define BUFFER_ADC_FIRST_BUF_INDEX	0	

typedef struct
{
	unsigned char	NeedConvertToASCII;
	unsigned char	MaxSize; 		// ������������ ������ ������ � ���������
	unsigned char	Size;				// ������� ������ ������ � ���������
	unsigned char 	Pos;				// ������� � ������ (����� ��������)
	unsigned char	ElementSize;	// ������ �������� � ������
	unsigned char* Data;
} BufferStruct;

extern BufferStruct Buffers[];

#endif






