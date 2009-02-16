#include <avr\io.h>
#include <avr\interrupt.h>
#include "buffers.h"
#include "blocks.h"
#include "comproc.h"
#include "hal.h"

#define PARAM_MODIFIER_VAR				0
#define PARAM_MODIFIER_CONST			1
#define PARAM_MODIFIER_BUFFER			2

#define HEX_TO_HALF_BYTE(n, success) {success = 1; if ((n >= '0') && (n <= '9')) n = n - '0'; else if ((n >= 'A') && (n <= 'F')) n = n - 'A' + 10; else success = 0;}
#define HALF_BYTE_TO_HEX(n) {if (n <= 9) n = n + '0'; else if ((n >= 0x0A) && (n <= 0x0F)) n = n + 'A' - 10; else n = 0;}

unsigned char Block0 (unsigned short paramsAddr)
{	
	unsigned char paramsCount;
	unsigned char paramSize;
	unsigned char from;
	unsigned char to;
	unsigned char i;
	unsigned char j;
	unsigned short pos;

	paramsCount = HALGetExtROMByte (paramsAddr);
	pos = paramsAddr + 1;

	for (i = 0; i < paramsCount; i++)
	{
		from = HALGetExtROMByte (pos);
		to = HALGetExtROMByte (pos + 1);
		paramSize = HALGetExtROMByte (pos + 2);

		for (j = 0; j < paramSize; j++)
			ComProcData.Memory [to + j] = Buffers[BUFFER_IN_COMMAND_ID].Data[from + j];

		pos += 3;
	}

	return BLOCK_RES_SUCCESS;
}

unsigned char Block1 (unsigned short paramsAddr)
{
	unsigned char paramsCount;
	unsigned char paramSize;
	unsigned char from;
	unsigned char paramModifier;
	unsigned char i;
	unsigned char j;
	unsigned short pos;
	unsigned char buffId;
	unsigned char halfByteLo;
	unsigned char halfByteHi;
	unsigned char bufSizeInBytes;

	paramsCount = HALGetExtROMByte (paramsAddr);
	pos = paramsAddr + 1;

	for (i = 0; i < paramsCount; i++)
	{
		from = HALGetExtROMByte (pos);
		paramModifier = HALGetExtROMByte (pos + 1);
		paramSize = HALGetExtROMByte (pos + 2);

		if (paramModifier == PARAM_MODIFIER_VAR)
		{
			// отправляем параметр фиксированного размера
			
			for (j = 0; j < paramSize; j++)
				HALWriteOutCommandByte (ComProcData.Memory [from + j]);
		}
		else if (paramModifier == PARAM_MODIFIER_BUFFER)
		{
			// отправляем содержимое буфера
			buffId = ComProcData.Memory [from]; // индекс буфера
			
			if (Buffers[buffId].ElementSize == 1)
				bufSizeInBytes = Buffers[buffId].Size;
			else if (Buffers[buffId].ElementSize == 2)
				bufSizeInBytes = Buffers[buffId].Size + Buffers[buffId].Size;
			else
				bufSizeInBytes = 0;	
			
			if (buffId >= BUFFERS_COUNT)
			{
				ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
				HALWriteOutCommandByte (EOC);
				return BLOCK_RES_ERROR;
			}
			
			if (Buffers[buffId].NeedConvertToASCII) // преобразуем содержимое буфера в ASCII коды
			{
				for (j = 0; j < bufSizeInBytes; j++)
				{
					halfByteLo = Buffers[buffId].Data[j];
					halfByteHi = halfByteLo;
					halfByteLo &= 0x0F;
					halfByteHi >>= 4;
					HALF_BYTE_TO_HEX (halfByteLo);
					HALF_BYTE_TO_HEX (halfByteHi);
					HALWriteOutCommandByte (halfByteHi);	
					HALWriteOutCommandByte (halfByteLo);
				}
			}
			else // отправляем содержимое буфера как есть
			{
				for (j = 0; j < bufSizeInBytes; j++)
				{
					HALWriteOutCommandByte (Buffers[buffId].Data[j]);	
				}
			}			
		}
		else if (paramModifier == PARAM_MODIFIER_CONST)
		{
			// отправляем константу
			HALWriteOutCommandByte (from);
		}

		pos+= 3;
	}
   HALWriteOutCommandByte (EOC);
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockPlus (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 2)] =
		ComProcData.Memory [HALGetExtROMByte (paramsAddr)] + ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)];
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockMinus (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 2)] =
		ComProcData.Memory [HALGetExtROMByte (paramsAddr)] - ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)];
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockHexStrToByte (unsigned short paramsAddr)
{	
	unsigned char n1;
	unsigned char n2;
	unsigned char success;
	unsigned char index = 0;
	
	index = HALGetExtROMByte (paramsAddr);
	
	n1 = ComProcData.Memory [index + 1];
	HEX_TO_HALF_BYTE(n1, success);
	if (!success)
		goto Err;
		
	n2 = ComProcData.Memory [index];
	HEX_TO_HALF_BYTE(n2, success);
	if (!success)
		goto Err;
		
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)] = n1 | (n2 << 4);
	return BLOCK_RES_SUCCESS;
	
Err:
	ComProcData.LastError = LAST_ERROR_INVALID_PARAM;
	return BLOCK_RES_ERROR;	
}

unsigned char BlockByteToHexStr (unsigned short paramsAddr)
{
	unsigned char n1;
	unsigned char n2;
	unsigned char index;
	
	index = HALGetExtROMByte (paramsAddr + 1);
	
	n1 = ComProcData.Memory [HALGetExtROMByte (paramsAddr)];
	n2 = n1 >> 4;
	n1 &= 0x0F;
	
	HALF_BYTE_TO_HEX (n1);
	HALF_BYTE_TO_HEX (n2);
	
	ComProcData.Memory [index] = n2;
	ComProcData.Memory [index + 1] = n1;
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockConst0 (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr)] = 0;
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockConst1 (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr)] = 1;
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockConst2 (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr)] = 2;
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockModuleAddress (unsigned short paramsAddr)
{
	unsigned char addr;
	
	if (ComProcData.Memory [HALGetExtROMByte (paramsAddr)] == 0)
	{
		// read
		addr = HALGetModuleAddress();
	}
	else
	{
		// write
		addr = ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)];
		HALSetModuleAddress(addr);
	}
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 3)] = addr;
	
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockCmpByte (unsigned short paramsAddr)
{
	unsigned char res;
	
	if (ComProcData.Memory [HALGetExtROMByte (paramsAddr)] == ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)])
		res = 1;
	else
		res = 0;
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 2)] = res;	
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockBreak (unsigned short paramsAddr)
{
	if (ComProcData.Memory [HALGetExtROMByte (paramsAddr)])
		return BLOCK_RES_SKIP_PROCESSING;
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockNot (unsigned short paramsAddr)
{		
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)] = !(ComProcData.Memory [HALGetExtROMByte (paramsAddr)]);
	return BLOCK_RES_SUCCESS;		
}

unsigned char BlockLastError (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr)] = ComProcData.LastError;
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockSortBuf (unsigned short paramsAddr)
{
	unsigned char bufId;
	short i;
	short j;
	unsigned short* data;
	unsigned char dataSize;
	unsigned short buf;
	
	//TODO: сейчас всегда подразумевается, что буфер с 2х байтными элементами
	
	bufId = ComProcData.Memory [HALGetExtROMByte (paramsAddr)];
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 3)] = bufId;
	
	// если блок disabled
	if (!ComProcData.Memory[HALGetExtROMByte(paramsAddr + 1)])
		return BLOCK_RES_SUCCESS;
			
	if (bufId >= BUFFERS_COUNT)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
	
	data = (unsigned short*)Buffers[bufId].Data;
	dataSize = Buffers[bufId].Size;
	
  	for(i = 1; i < dataSize; i++) // пробегаем весь массив, кроме начального эл-та
  	{
		buf = data[i]; 				// запомнили текущий эл-т, его мы затрем, надвигая массив
		for(j = i-1; (j >= 0) && (data[j] > buf); j--) // сдвигаем массив, прямо на текущий эл-т
			data[j+1] = data[j]; 	// этот сдвиг происходит пока сдвигаемые эл-ты больше, чем тот, на который надвигаем(i-тый)
		data[j+1] = buf; 				// теперь наш i-тый эл-т, который оказался меньше пачки сдвинутых эл-тов, помещаем перед ними
	}
			
	return BLOCK_RES_SUCCESS;	
}

unsigned char BlockAnd (unsigned short paramsAddr)
{	
	ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2)] = (ComProcData.Memory[HALGetExtROMByte (paramsAddr)]) && (ComProcData.Memory[HALGetExtROMByte (paramsAddr + 1)]);	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockCRC (unsigned short paramsAddr)
{
	unsigned char buffId;
	unsigned char sz;
	unsigned char crc = 0xFF;
   unsigned char i;
   unsigned char* pcBlock;
	
	// если блок disabled
	if (!ComProcData.Memory[HALGetExtROMByte(paramsAddr + 2)])
		return BLOCK_RES_SUCCESS;
	
	buffId = ComProcData.Memory[HALGetExtROMByte (paramsAddr)];
			
	if (buffId >= BUFFERS_COUNT)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
	
	sz = ComProcData.Memory[HALGetExtROMByte(paramsAddr + 1)];
	
	if (sz > Buffers[buffId].Size)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
	
	sz *= Buffers[buffId].ElementSize;
	
	pcBlock = Buffers[buffId].Data;
	
   while (sz--)
   {
        crc ^= *pcBlock++;

        for (i = 0; i < 8; i++)
            crc = crc & 0x80 ? (crc << 1) ^ 0x31 : crc << 1;
   }
	
	ComProcData.Memory[HALGetExtROMByte(paramsAddr + 4)] = crc; // сейчас CRC всегда = 7
	
	return BLOCK_RES_SUCCESS;	
}

unsigned char BlockCRCError (unsigned short paramsAddr)
{
	if (ComProcData.Memory[HALGetExtROMByte(paramsAddr)])
	{
		ComProcData.LastError = LAST_ERROR_INVALID_CRC;
		return BLOCK_RES_ERROR;
	}
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockInCommandBuffId (unsigned short paramsAddr)
{
	ComProcData.Memory[HALGetExtROMByte(paramsAddr)] = BUFFER_IN_COMMAND_ID;
	return BLOCK_RES_SUCCESS;	
}

unsigned char BlockADC (unsigned short paramsAddr)
{
	unsigned char mask;
	
	if (ComProcData.Memory[HALGetExtROMByte(paramsAddr)]) // если write
	{
		if (!HALGetADCReady())
		{
			ComProcData.LastError = LAST_ERROR_ADC_NOT_READY;
			return BLOCK_RES_ERROR;
		}
		mask = HALGetADCMask();
		HALRunADC(mask);
	}
	else // если read
	{
		if (!HALGetADCReady())
		{
			ComProcData.LastError = LAST_ERROR_ADC_NOT_READY;
			return BLOCK_RES_ERROR;
		}
		
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 3)] =  0;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 4)] =  1;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 5)] =  2;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 6)] =  3;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 7)] =  4;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 8)] =  5;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 9)] =  6;
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 10)] = 7;
	}
	return BLOCK_RES_SUCCESS;	
}

unsigned char BlockADCMask (unsigned short paramsAddr)
{
	unsigned char ADCMask;
	
	if (ComProcData.Memory[HALGetExtROMByte(paramsAddr)]) // если write
	{
		ADCMask = ComProcData.Memory[HALGetExtROMByte(paramsAddr + 1)];
		HALSetADCMask(ADCMask);
	}
	else // если read
	{
		ADCMask = HALGetADCMask();
	}
	
	ComProcData.Memory[HALGetExtROMByte(paramsAddr + 3)] = ADCMask;
	
	return BLOCK_RES_SUCCESS;	
}

unsigned char BlockADCChannelConfig (unsigned short paramsAddr)
{
	unsigned char channel;
	unsigned char mask;
	unsigned char addr;
	unsigned short word = 0;
	
	channel = ComProcData.Memory[HALGetExtROMByte(paramsAddr + 1)];
	mask = ComProcData.Memory[HALGetExtROMByte(paramsAddr + 2)];
	
	if (ComProcData.Memory[HALGetExtROMByte(paramsAddr)]) // если write
	{
		if (mask & 1)
		{
			addr = HALGetExtROMByte(paramsAddr + 3);	
			CREATE_WORD(ComProcData.Memory[addr], ComProcData.Memory[addr + 1], word);
			HALSetADCChannelMaxLevel(channel, word);
		}
		if (mask & 2)
		{
		   addr = HALGetExtROMByte(paramsAddr + 4);	
			CREATE_WORD(ComProcData.Memory[addr], ComProcData.Memory[addr + 1], word);
			HALSetADCChannelMinLevel(channel, word);
		}
		if (mask & 4)
		{
			
			HALSetADCChannelDataSize(channel, ComProcData.Memory[HALGetExtROMByte(paramsAddr + 5)]);
			Buffers[BUFFER_ADC_FIRST_BUF_INDEX + channel].Size =  HALGetADCChannelDataSize(channel);
		}
		if (mask & 8)
			HALSetADCChannelOpCode(channel, ComProcData.Memory[HALGetExtROMByte(paramsAddr + 6)]);
		if (mask & 16)
			HALSetADCChannelApproxId(channel, ComProcData.Memory[HALGetExtROMByte(paramsAddr + 7)]);	
	}
	else // если read
	{
		addr = HALGetExtROMByte(paramsAddr + 9);	
		word = HALGetADCChannelMaxLevel(channel);
		SPLIT_WORD(word, ComProcData.Memory[addr], ComProcData.Memory[addr + 1]);
	
		addr = HALGetExtROMByte(paramsAddr + 10);	
		word = HALGetADCChannelMinLevel(channel);
		SPLIT_WORD(word, ComProcData.Memory[addr], ComProcData.Memory[addr + 1]);

		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 11)] = HALGetADCChannelDataSize(channel);
	
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 12)] = HALGetADCChannelOpCode(channel);

		ComProcData.Memory[HALGetExtROMByte(paramsAddr + 13)] = HALGetADCChannelApproxId(channel);		
	}
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockHexStrToWord (unsigned short paramsAddr)
{	
	unsigned char n1;
	unsigned char n2;
	unsigned char success;
	unsigned char index;
	unsigned char index1;
	
	index = HALGetExtROMByte (paramsAddr);
	index1 = HALGetExtROMByte (paramsAddr + 1);
	
	n1 = ComProcData.Memory [index + 3];
	HEX_TO_HALF_BYTE(n1, success);
	if (!success)
		goto Err;
	
	n2 = ComProcData.Memory [index + 2];
	HEX_TO_HALF_BYTE(n2, success);
	if (!success)
		goto Err;
		
	ComProcData.Memory [index1] = n1 | (n2 << 4);
	
	n1 = ComProcData.Memory [index + 1];
	HEX_TO_HALF_BYTE(n1, success);
	if (!success)
		goto Err;
	
	n2 = ComProcData.Memory [index];
	HEX_TO_HALF_BYTE(n2, success);
	if (!success)
		goto Err;
	
	ComProcData.Memory [index1 + 1] = n1 | (n2 << 4);
	return BLOCK_RES_SUCCESS;
	
Err:
	ComProcData.LastError = LAST_ERROR_INVALID_PARAM;
	return BLOCK_RES_ERROR;
}

unsigned char BlockWordToHexStr (unsigned short paramsAddr)
{
	unsigned char n1;
	unsigned char n2;
	unsigned short index;
	unsigned short index1;
	
	index = HALGetExtROMByte (paramsAddr + 1);
	index1 = HALGetExtROMByte (paramsAddr);
	
	n1 = ComProcData.Memory [index1];
	n2 = n1 >> 4;
	n1 &= 0x0F;
	
	HALF_BYTE_TO_HEX (n1);
	HALF_BYTE_TO_HEX (n2);
	
	ComProcData.Memory [index + 2] = n2;
	ComProcData.Memory [index + 3] = n1;
	
	n1 = ComProcData.Memory [index1 + 1];
	n2 = n1 >> 4;
	n1 &= 0x0F;
	
	HALF_BYTE_TO_HEX (n1);
	HALF_BYTE_TO_HEX (n2);
	
	ComProcData.Memory [index] = n2;
	ComProcData.Memory [index + 1] = n1;
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockByteToBit (unsigned short paramsAddr)
{
	unsigned char byte;
	unsigned char i;	
	
	byte = ComProcData.Memory[HALGetExtROMByte(paramsAddr)];
	
	for (i = 1; i < 9; i++)
	{
		ComProcData.Memory[HALGetExtROMByte(paramsAddr + i)] = (byte & 0x01);
		byte >>= 1;
	}
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockBitToByte (unsigned short paramsAddr)
{
	unsigned char byte;
	unsigned char i;
	
	byte = 0;
	for (i = 0; i < 8; i++)
	{
		byte >>= 1;
		if (ComProcData.Memory[HALGetExtROMByte(paramsAddr + i)])
			byte |= 0x80;	
	}
	
	ComProcData.Memory[HALGetExtROMByte(paramsAddr + 8)] = byte;
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockBufLen (unsigned short paramsAddr)
{
	unsigned char buffId;
	
	buffId = ComProcData.Memory [HALGetExtROMByte (paramsAddr)];
			
	if (buffId >= BUFFERS_COUNT)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
		
	ComProcData.Memory [HALGetExtROMByte (paramsAddr + 1)] = Buffers[buffId].Size;
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockDiv (unsigned short paramsAddr)
{
	if (!ComProcData.Memory[HALGetExtROMByte (paramsAddr)]) // если disable	
	{
		ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3)] = 0;
		return BLOCK_RES_SUCCESS;
	}	

	ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3)] =  ComProcData.Memory[HALGetExtROMByte (paramsAddr + 1)] / ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2)];
	
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockAvgMean (unsigned short paramsAddr)
{
	unsigned char from;
	unsigned char to;
	unsigned char bufId;
	unsigned short* data;
	unsigned long S;
	unsigned char count;
	
	//TODO: сейчас всегда подразумевается буфер с 2х байтными элементами

	if (!ComProcData.Memory[HALGetExtROMByte (paramsAddr)]) // если disable	
	{
		ComProcData.Memory[HALGetExtROMByte (paramsAddr + 4)] = 0;
		ComProcData.Memory[HALGetExtROMByte (paramsAddr + 4) + 1] = 0;
		return BLOCK_RES_SUCCESS;
	}
	
	bufId = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 1)];
	from = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2)];
	to = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3)];
	
	if (bufId >= BUFFERS_COUNT)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
	if (from >= Buffers[bufId].Size)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
	if (to >= Buffers[bufId].Size)
	{
		ComProcData.LastError = LAST_ERROR_OUT_OF_RANGE_PARAM;
		return BLOCK_RES_ERROR;
	}
	if (to <= from)
	{
		ComProcData.LastError = LAST_ERROR_INVALID_PARAM;
		return BLOCK_RES_ERROR;
	}
	
	data = (unsigned short*)Buffers[bufId].Data;
	count = to - from + 1;
	S = 0;
	
	for (; from <= to; from++)
		S += data[from];
		
	S = S / count;
	
	SPLIT_WORD(S, ComProcData.Memory[HALGetExtROMByte (paramsAddr + 4)], ComProcData.Memory[HALGetExtROMByte (paramsAddr + 4) + 1]);	
	
	return BLOCK_RES_SUCCESS;
}

/*
   unsigned char approxId;
	unsigned short code;
	unsigned short val;
	unsigned short u1;
	unsigned char index;
	*/
	
unsigned char BlockApprox (unsigned short paramsAddr)
{
 	unsigned char approxId;
	unsigned short code;
	unsigned short val;
	unsigned short u1;
	unsigned char index;
	
	
	// если не enable
	if (!ComProcData.Memory[HALGetExtROMByte (paramsAddr)])
	{
		ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3)] = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2)];
		ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3) + 1] = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2) + 1];
		return BLOCK_RES_SUCCESS;	
	}
	
	approxId = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 1)];
	CREATE_WORD(ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2)], ComProcData.Memory[HALGetExtROMByte (paramsAddr + 2) + 1], code);
	
	index = code / 128;
	val = HALGetApproxWord(approxId, index);
	
	if (index <= 7)
	{
		u1 = HALGetApproxWord(approxId, (index + 1));	
		val += ((u1 - val) / 128) * (code - index * 128);			
	}	
	
	SPLIT_WORD(val, ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3)], ComProcData.Memory[HALGetExtROMByte (paramsAddr + 3) + 1]);

	return BLOCK_RES_SUCCESS;
}

unsigned char BlockConst00 (unsigned short paramsAddr)
{
	ComProcData.Memory[HALGetExtROMByte (paramsAddr)] = 0;
	ComProcData.Memory[HALGetExtROMByte (paramsAddr) + 1] = 0;
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockWord2DecStr (unsigned short paramsAddr)
{
	unsigned short index1;
	unsigned short index2;
	unsigned short word;
	unsigned char byteLo;
	unsigned char byteHi;
	unsigned char i;
	unsigned char pointPos;
	
	index1 = HALGetExtROMByte (paramsAddr);
	index2 = HALGetExtROMByte (paramsAddr + 2);
	
	byteLo = ComProcData.Memory[index1];
	byteHi = ComProcData.Memory[index1 + 1];
	pointPos = ComProcData.Memory[HALGetExtROMByte (paramsAddr + 1)];
	
	CREATE_WORD(byteLo, byteHi, word);
	
	for (i = 0; i < 6; i++)
	{
		if (i == pointPos && i > 0)
			ComProcData.Memory[index2 + 5 - i] = '.';
		else
		{
			ComProcData.Memory[index2 + 5 - i] = (word % 10) + '0';
			word /= 10;
		}
	}
		
	return BLOCK_RES_SUCCESS;
}

unsigned char BlockConst3 (unsigned short paramsAddr)
{
	ComProcData.Memory [HALGetExtROMByte (paramsAddr)] = 3;
	return BLOCK_RES_SUCCESS;		
}

BlockHandler* BlockHandlers [BLOCKS_COUNT] = {
/*00*/	&Block0,
/*01*/	&Block1,
/*02*/	&BlockPlus,
/*03*/	&BlockMinus,
/*04*/	&BlockHexStrToByte,
/*05*/	&BlockByteToHexStr,
/*06*/	&BlockConst0,
/*07*/	&BlockConst1,
/*08*/	&BlockConst2,
/*09*/	&BlockModuleAddress,
/*10*/	&BlockCmpByte,
/*11*/	&BlockBreak,
/*12*/	&BlockNot,
/*13*/   &BlockLastError,
/*14*/ 	&BlockSortBuf,
/*15*/ 	&BlockAnd,
/*16*/ 	&BlockCRC,
/*17*/ 	&BlockCRCError,
/*18*/ 	&BlockInCommandBuffId,
/*19*/   &BlockADC,
/*20*/ 	&BlockADCMask,
/*21*/	&BlockADCChannelConfig,
/*22*/	&BlockHexStrToWord,
/*23*/	&BlockWordToHexStr,
/*24*/	&BlockByteToBit,
/*25*/	&BlockBitToByte,
/*26*/	&BlockBufLen,
/*27*/	&BlockDiv,
/*28*/	&BlockAvgMean,
/*29*/	&BlockApprox,
/*30*/	&BlockConst00,
/*31*/	&BlockWord2DecStr,
/*32*/	&BlockConst3
};







































