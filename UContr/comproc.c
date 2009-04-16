#include <avr\io.h>
#include <avr\interrupt.h>
#include "blocks.h"
#include "hal.h"
#include "comproc.h"
#include "buffers.h"

ComProcDataStruct ComProcData;

//������������� ComProc ����?
void ComProcInit (void)
{
	ComProcData.LastError = 0;
	ComProcData.InCommandState = IN_COMMAND_STATE_PROCESSED;
	Buffers[BUFFER_IN_COMMAND_ID].Size = 0;
}

//��������� ������ �����
static unsigned short ComProcGetSchemaAddress (void)
{
	//������, ��� ����� - ��� ����� ������. �.�. ���������� ���������� �������
	
	unsigned char masksCount; //����� �����. �����?
	unsigned char i; //��� �����
	unsigned char j; //��� �����
	unsigned char maskLength; //����� �����. �����?
	unsigned short pos; //�������. ���� � ���?
	unsigned char curMaskByte; //������� ���� �����
	unsigned short result; //����� �����
	
	pos = 0;
	masksCount = HALGetExtROMByte (pos); //�������� ����� �����. ��� ��� � �����?
	pos++;

   //��� ������ ����� ������� �� ����. ������ ��������� ������ ������������ ��� ����� ������ �����
	for (i = 0; i < masksCount; i++)
	{
		maskLength = HALGetExtROMByte (pos); //������ ����� �����. ��������� ��� ��� �����������?

		if (maskLength != Buffers[BUFFER_IN_COMMAND_ID].Size) //���������� ����� ����� � ������ �������������� ������� � ������.
		{                                                     //��������� ��������� pos
			pos += maskLength + 3;
			continue;
		}

		//��� �� ����� � ����� �������? ������� ����?

		pos++;

		//������������ ������� �����
		for (j = 0; j < maskLength; j++)
		{
			curMaskByte = HALGetExtROMByte (pos + j); //�������� ���� �����

			if (curMaskByte == MASK_ANY_SYMBOL)			//���� �� ������, ���� �� ����� ������ �������
				continue;

			if (curMaskByte != Buffers[BUFFER_IN_COMMAND_ID].Data[j]) //���� ���� ����� �� ����� ������ �� ������, �� �������
				break;
		}

		if (j < maskLength)	//������ ���������� ����������
		{
			pos += maskLength + 2;
			continue;
		}

		//TODO: ��������� BIG endian ��� LITTLE endian. �����?
		//�� �� �?
		result = HALGetExtROMByte (pos + maskLength);
		result <<= 8;
		result += HALGetExtROMByte (pos + maskLength + 1);
		//������ ���������� ���������?
		return result;
	}
	
	return 0;
}

//��������� ����� �� ����������� ������
static unsigned char ComProcProcessSchema (unsigned short schemaAddress)
{
	unsigned char blocksCount; //����� ������
	unsigned char blockIndex; //���������
	unsigned char i; //��� �����
	unsigned short pos;
	unsigned short paramsAddr;
	unsigned char res;

	pos = schemaAddress;
	blocksCount = HALGetExtROMByte (pos); //��������� ����� ������
	pos++;
	
	//��� ������� �����
	for (i = 0; i < blocksCount; i++)
	{
		blockIndex = HALGetExtROMByte (pos); //�������� ����� �����. �� ������������� bloack
		//TODO: ��������� BIG endian ��� LITTLE endian
		paramsAddr = HALGetExtROMByte (pos + 1);
		paramsAddr <<= 8;
		paramsAddr |= HALGetExtROMByte (pos + 2);
		res = (BlockHandlers [blockIndex])(paramsAddr); //��� ������ ��� ��������� ������ ������ ������. ��������� ��� ������? �� ��������� paramsAddr?
		if (res == BLOCK_RES_ERROR)
			return 0;
		if (res == BLOCK_RES_SKIP_PROCESSING)
			return 1;
		pos += 3;
	}

	return 1;
}

static void ComProcFillInCommand (char* data, unsigned char size)
{
	unsigned char i;
	
	for (i = 0; i < size; i++)
		Buffers[BUFFER_IN_COMMAND_ID].Data[i] = data[i];

	Buffers[BUFFER_IN_COMMAND_ID].Size = size;
}

unsigned char ComProcProcessInCommand (void)
{
	unsigned short schemaAddress;

	schemaAddress = ComProcGetSchemaAddress ();
	if (schemaAddress != 0)
	{
		if (ComProcProcessSchema (schemaAddress))
			return 1;
		
		ComProcFillInCommand (INTERNAL_ERROR_COMMAND, INTERNAL_ERROR_COMMAND_LENGTH);
		schemaAddress = ComProcGetSchemaAddress ();
		if (schemaAddress != 0)
			return ComProcProcessSchema (schemaAddress);

		return 0;
	}
		
	return 0;
}






