#include "stdafx.h"
#include "CompileHelper.h"

/*******************************
*            RomData
********************************/

RomData::RomData (int romDataSize)
{
	size = romDataSize;
	usedSize = 0;
	data = new unsigned char [size];
	memset (data, 0, size);
}

RomData::~RomData ()
{
	delete [] data;
}

unsigned char RomData::operator [] (const int index) const
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("Попытка чтения байта ПЗУ по адресу " + IntToString (index) + ". Всего памяти доступно " + IntToString (size) + " байт.");
	return data[index];
}

unsigned char & RomData::operator [] (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("Попытка записи байта ПЗУ по адресу " + IntToString (index) + ". Всего памяти доступно " + IntToString (size) + " байт.");
	if (usedSize <= index)
		usedSize = index + 1;
	return data[index];
}

void RomData::SaveBin (std::string filePath)
{
	FILE* f;

	f = fopen (filePath.c_str (), "wb+");
	if (f == 0)
		throw CompilationException ("Ошибка при записи данных ПЗУ в файл. Невозможно открыть файл " + filePath);
	fwrite (data, 1, size, f);
	fclose (f);
}

/*******************************
*          MemoryPool
********************************/

MemoryPool::MemoryPool (int memoryPoolSize)
{
	size = memoryPoolSize;
	maxUsedSize = 0;
	memory = new unsigned char [size];
	memset (memory, 0, size);
}

MemoryPool::~MemoryPool ()
{
	delete [] memory;
}

void MemoryPool::Free ()
{
	memset (memory, 0, size);
}

bool MemoryPool::IsFreeByte (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("Попытка проверки состояния байта пула памяти по адресу " + IntToString (index) + ". Размер пула памяти " + IntToString (size) + " байт.");
	return (memory[index] == 0);
}

void MemoryPool::AllocateByte (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("Попытка выделения байта пула памяти по адресу " + IntToString (index) + ". Размер пула памяти " + IntToString (size) + " байт.");
	
	if (maxUsedSize <= index)
		maxUsedSize = index + 1;

	memory[index] = 1;
}

void MemoryPool::FreeByte (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("Попытка освобождения байта пула памяти по адресу " + IntToString (index) + ". Размер пула памяти " + IntToString (size) + " байт.");
	
	memory[index] = 0;
}