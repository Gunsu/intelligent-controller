#ifndef __HAL__H__
#define __HAL__H__

// Структура EEPROM

// 1b // moduleAddress
// 1b // DACMask

// Channel0 // 2b minLevel
//	Channel0 // 2b maxLevel
//	Channel0 // 1b dataSize
//	Channel0 // 1b operationCode
//	Channel0 // 1b approxTableId

// Channel1 // 2b minLevel
//	Channel1 // 2b maxLevel
//	Channel1 // 1b dataSize
//	Channel1 // 1b operationCode
//	Channel1 // 1b approxTableId

//..............................

// Channel7 // 2b minLevel
//	Channel7 // 2b maxLevel
//	Channel7 // 1b dataSize
//	Channel7 // 1b operationCode
//	Channel7 // 1b approxTableId

// Далее таблицы аппроксимации


#define EEPROM_APPROX_TABLE_SIZE			9 * 2						// в байтах

// Адреса и смещения в EEPROM

#define EEPROM_MODULE_ADDRESS				0                    // адрес адреса модуля
#define EEPROM_ADC_MASK						1                    // адрес маски каналов АЦП
#define EEPROM_ADC_CHANNELS				2							// адрес, с которорго начинаются данные каналов
#define EEPROM_ADC_CHANNEL_MIN_LEVEL	0                    // смещения полей в структуре канала
#define EEPROM_ADC_CHANNEL_MAX_LEVEL	2
#define EEPROM_ADC_CHANNEL_DATA_SIZE	4
#define EEPROM_ADC_CHANNEL_OP_COPDE		5
#define EEPROM_ADC_CHANNEL_APPROX_ID	6
#define EEPROM_ADC_CHANNEL_SIZE			7       					// размер данных на канал
#define EEPROM_APPROX_TABLES				EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * 4	// адрес, с которого начинаются таблицы аппроксимации

// Чтение\запись конкретных значений в EEPROM

#define HALGetModuleAddress()							HALGetEEPROMByte(EEPROM_MODULE_ADDRESS);
#define HALSetModuleAddress(addr)					HALSetEEPROMByte(EEPROM_MODULE_ADDRESS, addr);
#define HALGetADCMask()									HALGetEEPROMByte(EEPROM_ADC_MASK);
#define HALSetADCMask(mask)							HALSetEEPROMByte(EEPROM_ADC_MASK, mask);
#define HALGetADCChannelMinLevel(channel)	 		HALGetEEPROMWord(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_MIN_LEVEL);		
#define HALSetADCChannelMinLevel(channel, val)	HALSetEEPROMWord(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_MIN_LEVEL, val);
#define HALGetADCChannelMaxLevel(channel)			HALGetEEPROMWord(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_MAX_LEVEL);
#define HALSetADCChannelMaxLevel(channel, val)	HALSetEEPROMWord(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_MAX_LEVEL, val);
#define HALGetADCChannelDataSize(channel)			HALGetEEPROMByte(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_DATA_SIZE);
#define HALSetADCChannelDataSize(channel, val)	HALSetEEPROMByte(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_DATA_SIZE, val);
#define HALGetADCChannelOpCode(channel)         HALGetEEPROMByte(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_OP_COPDE);
#define HALSetADCChannelOpCode(channel, val)    HALSetEEPROMByte(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_OP_COPDE, val);
#define HALGetADCChannelApproxId(channel)       HALGetEEPROMByte(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_APPROX_ID);
#define HALSetADCChannelApproxId(channel, val)  HALSetEEPROMByte(EEPROM_ADC_CHANNELS + EEPROM_ADC_CHANNEL_SIZE * channel + EEPROM_ADC_CHANNEL_APPROX_ID, val);
#define HALGetApproxWord(approxId, index)			HALGetEEPROMWord(EEPROM_APPROX_TABLES + EEPROM_APPROX_TABLE_SIZE * approxId + index * 2);


// Константы АЦП

#define ADC_CHANNELS_COUNT	8

// Константы UART

#define F_CPU					16000000UL
#define UART_BAUD				76800

// сборка\разборка слова из байтов

#define CREATE_WORD(byteLo, byteHi, word) {word = byteHi << 8; word |= byteLo;}
#define SPLIT_WORD(word, byteLo, byteHi) {byteLo = word; byteHi = word >> 8;}

void HALInit (void);
unsigned char HALGetExtROMByte(unsigned short addr);
void HALProcessChannels(void);
void HALWriteOutCommandByte(unsigned char byte);

unsigned char HALGetEEPROMByte(unsigned short addr);
void HALSetEEPROMByte(unsigned short addr, unsigned char byte);
unsigned short HALGetEEPROMWord(unsigned short addr);
void HALSetEEPROMWord(unsigned short addr, unsigned short word);

void HALRunADC (unsigned char ChannelsMask);
unsigned char HALGetADCReady(void);

#endif
































