#include <avr\io.h>
#include <avr\interrupt.h>
#include "blocks.h"
#include "hal.h"
#include "comproc.h"
#include "buffers.h"

ComProcDataStruct ComProcData;

//инициализация ComProc чего?
void ComProcInit (void)
{
	ComProcData.LastError = 0;
	ComProcData.InCommandState = IN_COMMAND_STATE_PROCESSED;
	Buffers[BUFFER_IN_COMMAND_ID].Size = 0;
}

//получение адреса схемы
static unsigned short ComProcGetSchemaAddress (void)
{
	//похоже, что маска - это маски команд. т.е. определяет допустимые команды
	
	unsigned char masksCount; //число масок. зачем?
	unsigned char i; //для цикла
	unsigned char j; //для цикла
	unsigned char maskLength; //длина маски. зачем?
	unsigned short pos; //позиция. чего в чём?
	unsigned char curMaskByte; //текущий байт маски
	unsigned short result; //адрес схемы
	
	pos = 0;
	masksCount = HALGetExtROMByte (pos); //получаем число масок. это что и зачем?
	pos++;

   //для каждой маски находим хз чего. причём результат всегда возвращается уже после первой маски
	for (i = 0; i < masksCount; i++)
	{
		maskLength = HALGetExtROMByte (pos); //читаем длину маски. интересно где она сохраняется?

		if (maskLength != Buffers[BUFFER_IN_COMMAND_ID].Size) //сравниваем длину маски с длиной идентификатора команды в буфере.
		{                                                     //непонятно считается pos
			pos += maskLength + 3;
			continue;
		}

		//что мы нашли к этому моменту? позицию чего?

		pos++;

		//обрабатываем текущую маску
		for (j = 0; j < maskLength; j++)
		{
			curMaskByte = HALGetExtROMByte (pos + j); //получаем байт маски

			if (curMaskByte == MASK_ANY_SYMBOL)			//ничё не делаем, если он равен любому символу
				continue;

			if (curMaskByte != Buffers[BUFFER_IN_COMMAND_ID].Data[j]) //если байт маски не равен данным из буфера, то выходим
				break;
		}

		if (j < maskLength)	//совсем непонятные вычисления
		{
			pos += maskLength + 2;
			continue;
		}

		//TODO: проверить BIG endian или LITTLE endian. зачем?
		//чё за х?
		result = HALGetExtROMByte (pos + maskLength);
		result <<= 8;
		result += HALGetExtROMByte (pos + maskLength + 1);
		//почему возвращаем результат?
		return result;
	}
	
	return 0;
}

//обработка схемы по полученному адресу
static unsigned char ComProcProcessSchema (unsigned short schemaAddress)
{
	unsigned char blocksCount; //число блоков
	unsigned char blockIndex; //непонятно
	unsigned char i; //для цикла
	unsigned short pos;
	unsigned short paramsAddr;
	unsigned char res;

	pos = schemaAddress;
	blocksCount = HALGetExtROMByte (pos); //считываем число блоков
	pos++;
	
	//для каждого блока
	for (i = 0; i < blocksCount; i++)
	{
		blockIndex = HALGetExtROMByte (pos); //получаем номер блока. он соответствует bloack
		//TODO: проверить BIG endian или LITTLE endian
		paramsAddr = HALGetExtROMByte (pos + 1);
		paramsAddr <<= 8;
		paramsAddr |= HALGetExtROMByte (pos + 2);
		res = (BlockHandlers [blockIndex])(paramsAddr); //тут похоже идёт обработка данных данным блоком. интересно где данные? по параметру paramsAddr?
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






