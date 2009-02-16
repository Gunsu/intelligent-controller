#include "StdAfx.h"
#include "Common.h"

double Min (double arg1, double arg2)
{
	if (arg1 < arg2)
		return arg1;
	return arg2;
}

double Max (double arg1, double arg2)
{
	if (arg1 > arg2)
		return arg1;
	return arg2;
}

double Abs (double arg)
{
	if (arg < 0)
		return -arg;
	return arg;
}

int Abs (int arg)
{
	if (arg < 0)
		return -arg;
	return arg;
}

int Round (double x)
{
	return ((x - int (x) >= 0.5) ? int (x) + 1 : int (x));
}

std::string IntToString(int num)
{
	char str [32];
	itoa (num, str, 10);
	return std::string (str);
}

int StringToInt (std::string s)
{
	return atoi (s.c_str ());
}

std::string DoubleToString (double num)
{
	char str [32];
	sprintf(str, "%f", num);
	return std::string (str);
}

double StringToDouble (std::string s)
{
	return atof (s.c_str ());
}

void GetModuleFolder (LPCH folderPath, int size)
{
	CHAR* slashP;

	::GetModuleFileName(NULL, folderPath, size);
	slashP = strrchr(folderPath, '\\');

	if (slashP > 0)
		folderPath[(slashP - folderPath) / sizeof (CHAR) + 1] = '\0';
}

CString GetModuleFolder ()
{
	CHAR s[1024];
	GetModuleFolder (s, 1024);
	return CString (s);
}

CString GetFileTitle (CString fileName)
{
	int pos;

	pos = fileName.ReverseFind('.');
	if (pos >= 0)
		return fileName.Left (pos);

	return fileName;
}
