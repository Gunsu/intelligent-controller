#include "hal.h"
#include "comproc.h"
#include "buffers.h"
#include <avr\io.h>
#include <avr\interrupt.h>

//TODO: это временная структура, заменяет EEPROM

#define EEPROM_SIZE	2 + 7 * 4 + 18
unsigned char EEPROMData[EEPROM_SIZE];

// ******************************************************
// ********     Чтение внешнего FLASH ПЗУ       *********
// ******************************************************

inline void HALInitExtROM (void)
{
	DDRB = _BV(DDB7) | _BV(DDB5);
	SPCR = _BV(SPE) | _BV(MSTR);
}

unsigned char HALGetExtROMByte (unsigned short addr)
{
	unsigned char dataByte = 0;
	
	SPDR = (addr >> 8);				// hi
	loop_until_bit_is_set(SPSR, SPIF);
	SPDR = (unsigned char)addr;	// lo
	loop_until_bit_is_set(SPSR, SPIF);
	SPDR = 0xFF;
	loop_until_bit_is_set(SPSR, SPIF);
	dataByte = SPDR;
	
	return dataByte;
}

// ******************************************************
// ********     Чтение\запись команд в UART     *********
// ******************************************************

inline void HALInitUART (void)
{
	UBRR = (F_CPU / (16UL * UART_BAUD)) - 1;
	UCR = _BV(TXEN) | _BV(RXEN) | _BV(TXB8) | _BV(RXCIE);
}

ISR(UART_RX_vect)
{	
	unsigned char c;
	unsigned short bufSize;
	
	c = UDR;
	
	if (ComProcData.InCommandState == IN_COMMAND_STATE_PROCESSED)	
	{
		bufSize = Buffers[BUFFER_IN_COMMAND_ID].Size;
		if ((bufSize >= Buffers[BUFFER_IN_COMMAND_ID].MaxSize) || (c == EOC))
		{
			// внутренние команды игнорируются
  			if ((bufSize > 0) && (Buffers[BUFFER_IN_COMMAND_ID].Data[0] == INTERNAL_COMMAND_PREFIX))
  			{
  				Buffers[BUFFER_IN_COMMAND_ID].Size = 0;
  				return;	
  			}
  							
			ComProcData.InCommandState = IN_COMMAND_STATE_READY_TO_PROCESS;
			return;		
		}

		Buffers[BUFFER_IN_COMMAND_ID].Data[bufSize] = c;
		bufSize++;
		Buffers[BUFFER_IN_COMMAND_ID].Size = bufSize;
	}
}

void HALWriteOutCommandByte (unsigned char byte)
{
	loop_until_bit_is_set(USR, UDRE);
	UDR = byte;
}

// ******************************************************
// ******** Чтение\запись конфигурации в EEPROM *********
// ******************************************************

inline void HALInitEEPROM (void)
{
	//TODO: сделать инициализацию реальной EEPROM (если это вообще нужно)
	unsigned short i;
	
	// сейчас инициализация временной структуры нулями
	for (i = 0; i < EEPROM_SIZE; i++)
		EEPROMData[i] = 0;
		
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 0, 0); // code 0
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 2, 5000); // code 128
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 4, 10000); // code 256
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 6, 15000); // code 384
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 8, 20000); // code 512
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 10, 30000); // code 640
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 12, 45000); // code 768
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 14, 50000); // code 896
	HALSetEEPROMWord(EEPROM_APPROX_TABLES + 16, 60000); // code 1024
}

unsigned char HALGetEEPROMByte(unsigned short addr)
{
	//TODO: через EEPROM
	return EEPROMData[addr];
}

void HALSetEEPROMByte(unsigned short addr, unsigned char byte)
{
	//TODO: через EEPROM
	EEPROMData[addr] = byte;
}

unsigned short HALGetEEPROMWord(unsigned short addr)
{
	//TODO: через EEPROM
	//unsigned short word;
	
	//CREATE_WORD(EEPROMData[addr], EEPROMData[addr + 1], word);
	return *(unsigned short *)(EEPROMData + addr);
}

void HALSetEEPROMWord(unsigned short addr, unsigned short word)
{
	//TODO: через EEPROM
	*(unsigned short *)(EEPROMData + addr) = word;
}

/*
void HALGetEEPROMData(unsigned short addr, void* data, unsigned short size)
{
	//TODO: через EEPROM
	unsigned short i;
	
	for (i = 0; i < size; i++, addr++)
		((unsigned char*)data)[i] = HALGetEEPROMByte(addr);	
}

void HALSetEEPROMData(unsigned short addr, void* data, unsigned short size)
{
	//TODO: через EEPROM
	unsigned short i;
	
	for (i = 0; i < size; i++, addr++)
		HALSetEEPROMByte(addr, ((unsigned char*)data)[i]);
}
*/

// ******************************************************
// ********             Работа с АЦП            *********
// ******************************************************

typedef struct
{
	unsigned char ChannelsMask;		   	// маска задействованых каналов
	unsigned char CurChannelIdx; 				// индекс текущего канала в массиве задействованых каналов
	unsigned char NotReadyChannelsCount;	// количество не до конца заполненых каналов
} HALADCDataStruct;

HALADCDataStruct HALADCData;

inline void HALInitADC (void)
{
	HALADCData.ChannelsMask = 0;
	HALADCData.CurChannelIdx = 0;
	HALADCData.NotReadyChannelsCount = 0; // считаем, что АЦП готов
	
	ADCSR = _BV(ADEN) | _BV(ADIE) | _BV(ADPS0) | _BV(ADPS1) | _BV(ADPS2);	
}

void HALSetNextADCChannel(void)
{
	// переходим к следующему задействованному каналу
	// внимание, алгоритм зациклится, если маска каналов нулевая	
	do
	{
		if (HALADCData.CurChannelIdx == 7)
			HALADCData.CurChannelIdx = 0;
		else
			HALADCData.CurChannelIdx++;	
	}
	while (((HALADCData.ChannelsMask >> HALADCData.CurChannelIdx) & 0x01) == 0);
}

void HALRunADC (unsigned char ChannelsMask)
{
	unsigned char i;
	
	HALADCData.ChannelsMask = ChannelsMask;
	HALADCData.CurChannelIdx = 7;
	HALADCData.NotReadyChannelsCount = 0;
	
	for (i = 0; i < 8; i++)
	{
		if ((ChannelsMask >> i) & 0x01)
			HALADCData.NotReadyChannelsCount++;
			
		Buffers[BUFFER_ADC_FIRST_BUF_INDEX + i].Pos = 0;
	}
	
	// запускаем АЦП, если маска не нулевая
	if (HALADCData.NotReadyChannelsCount > 0)
	{
		HALSetNextADCChannel(); 				// переходим к первому из задействованых каналов
		ADMUX = HALADCData.CurChannelIdx; 	// для этого канала и будем запускать АЦП
		ADCSR = ADCSR | _BV(ADSC);	
	}
}

unsigned char HALGetADCReady(void)
{
	if (HALADCData.NotReadyChannelsCount == 0)
		return 1;
	return 0;
}

ISR(ADC_vect)
{
	unsigned char byteLo;
	unsigned char byteHi;
	unsigned short word;
	unsigned char bufIdx;
	
	// кладем значение с АЦП в буфер текущего канала	
	byteLo = ADCL;
	byteHi = ADCH;	
	CREATE_WORD(byteLo, byteHi, word);
	
	// TODO: допусковый контроль
	
	bufIdx = BUFFER_ADC_FIRST_BUF_INDEX + HALADCData.CurChannelIdx;
	((unsigned short*)Buffers[bufIdx].Data)[Buffers[bufIdx].Pos] = word;
   Buffers[bufIdx].Pos++;

	// если текущий буфер заполнен, то декрементируем количество незаполненых буферов
   if (Buffers[bufIdx].Pos >= Buffers[bufIdx].Size)
   	HALADCData.NotReadyChannelsCount--;
   	
  	// если был заполнен последний незаполненый буфер, то к следующему каналу не переходим
   if (HALADCData.NotReadyChannelsCount == 0)
   	return;
   		
	HALSetNextADCChannel(); 				// переходим к следующему каналу
	ADMUX = HALADCData.CurChannelIdx; 	// для этого канала и будем запускать АЦП
	ADCSR = ADCSR | _BV(ADSC);          // и запускаем
}

// ******************************************************
// ********          Инициализация HAL          *********
// ******************************************************

void HALInit (void)
{	
	HALInitEEPROM();
	HALInitExtROM();
	HALInitADC();	
	HALInitUART();
	
	sei();
}






































