#ifndef COMMON_H
#define COMMON_H

#include <string>

double Min (double arg1, double arg2);
double Max (double arg1, double arg2);
double Abs (double arg);
int Abs (int arg);
int Round (double x);
std::string IntToString(int num);
int StringToInt (std::string s);
std::string DoubleToString (double num);
double StringToDouble (std::string s);

void GetModuleFolder (LPCH folderPath, int size);
CString GetModuleFolder ();
CString GetFileTitle (CString fileName);

#endif