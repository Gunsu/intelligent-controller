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
		throw CompilationException ("������� ������ ����� ��� �� ������ " + IntToString (index) + ". ����� ������ �������� " + IntToString (size) + " ����.");
	return data[index];
}

unsigned char & RomData::operator [] (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("������� ������ ����� ��� �� ������ " + IntToString (index) + ". ����� ������ �������� " + IntToString (size) + " ����.");
	if (usedSize <= index)
		usedSize = index + 1;
	return data[index];
}

void RomData::SaveBin (std::string filePath)
{
	FILE* f;

	f = fopen (filePath.c_str (), "wb+");
	if (f == 0)
		throw CompilationException ("������ ��� ������ ������ ��� � ����. ���������� ������� ���� " + filePath);
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
		throw CompilationException ("������� �������� ��������� ����� ���� ������ �� ������ " + IntToString (index) + ". ������ ���� ������ " + IntToString (size) + " ����.");
	return (memory[index] == 0);
}

void MemoryPool::AllocateByte (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("������� ��������� ����� ���� ������ �� ������ " + IntToString (index) + ". ������ ���� ������ " + IntToString (size) + " ����.");
	
	if (maxUsedSize <= index)
		maxUsedSize = index + 1;

	memory[index] = 1;
}

void MemoryPool::FreeByte (int index)
{
	if ((index < 0) || (index >= size))
		throw CompilationException ("������� ������������ ����� ���� ������ �� ������ " + IntToString (index) + ". ������ ���� ������ " + IntToString (size) + " ����.");
	
	memory[index] = 0;
}