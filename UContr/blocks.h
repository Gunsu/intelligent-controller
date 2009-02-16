#ifndef __BLOCKS__H__
#define __BLOCKS__H__

#define BLOCK_RES_ERROR					0
#define BLOCK_RES_SUCCESS				1
#define BLOCK_RES_SKIP_PROCESSING	2

#define BLOCKS_COUNT	33

typedef unsigned char ((BlockHandler)(unsigned short paramsAddr));
extern BlockHandler* BlockHandlers[];

#endif














