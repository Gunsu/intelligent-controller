#ifndef COMPILE_HELPER_H
#define COMPILE_HELPER_H

#include <string>
#include "Exceptions.h"
#include "Common.h"

/*******************************
*            RomData
********************************/

class RomData
{
	private:
		int size;
		int usedSize;
		unsigned char* data;

	public:
		RomData (int size);
		~RomData ();
		int GetSize () { return size; }
		int GetUsedSize () { return usedSize; }
		unsigned char operator [] (int index) const;
		unsigned char & operator [] (int index);
		void SaveBin (std::string filePath);
};

/*******************************
*          MemoryPool
********************************/

class MemoryPool
{
	private:
		int size;
		int maxUsedSize;
		unsigned char *memory;

	public:
		MemoryPool (int memoryPoolSize);
		~MemoryPool ();
		int GetSize () { return size; }
		int GetMaxUsedSize () { return maxUsedSize; }
		void Free ();
		bool IsFreeByte (int index);
		void AllocateByte (int index);
		void FreeByte (int index);
};

#endif