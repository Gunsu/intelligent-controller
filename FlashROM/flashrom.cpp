// =============================================================================
// C++ code template for a VMLAB user defined component
// Target file must be Windows .DLL (no .EXE !!)
//
// Component name: flashrom
//
#include <windows.h>
#include <commctrl.h>
#include <stdio.h>
#pragma hdrstop
#include "C:\VMLAB\bin\blackbox.h"
int WINAPI DllEntryPoint(HINSTANCE, unsigned long, void*) {return 1;} // is DLL
//==============================================================================

#define DATA_SIZE		1024 * 64
#define STATE_READ_ADDR		1
#define STATE_PASS_TICK		2
#define STATE_WRITE_DATA	3
//#define _DEBUG_

//==============================================================================
// Declare pins here
//
DECLARE_PINS
   DIGITAL_IN(SCK, 1);
   DIGITAL_IN(MOSI, 2);
   DIGITAL_OUT(MISO, 3);
END_PINS

// =============================================================================
// Declare module global variables here.
// The reason for this is to keep a set of variables by instance
// To use a variable, do it through the the macro VAR(...)
//
DECLARE_VAR
	unsigned char Data [DATA_SIZE];
	int State;
	unsigned short Addr;
	unsigned char DataByte;
	int Iterations;
	double LastTime; 
END_VAR

// You can delare also globals variable outside DECLARE_VAR / END_VAR, but if
// multiple instances of this cell are placed, all these instances will
// share the same variable.
//

// ============================================================================
// Say here if your component has an associated window or not. Pass as parameter
// the dialog resource ID (from .RC file) or 0 if no window
//
USE_WINDOW(0);   // If window USE_WINDOW(WINDOW_USER_1) (for example)

// =============================================================================
// Callback functions. These functions are called by VMLAB at the proper time

const char *On_create()
//********************
// Perform component creation. It must return NULL if the creation process is
// OK, or a message describing the error cause. The message will show in the
// Messages window. Typical tasks: check passed parameters, open files,
// allocate memory,...
{
   return NULL;
}

void On_window_init(HWND pHandle)
//*******************************
// Window initialization. Fill only if the component has an associated window
// -USE_WINDOW(..) not zero-. The Parameter pHandle brings the main component
// window handle. Typical tasks: fill controls with data, intitialize gadgets,
// hook an own Windows structure (VCL, MFC,...)
{

}

void On_destroy()
//***************
// Destroy component. Free here memory allocated at On_create; close files
// etc.
{

}

void On_simulation_begin()
//************************
// VMLAB informs you that the simulation is starting. Initialize pin values
// here Open files; allocate memory, etc.
{
	const int dataPathSize = 1024;
	TCHAR dataPath [dataPathSize];

	GetModuleFileName (NULL, dataPath, dataPathSize);
	(*strrchr (dataPath, '\\')) = '\0';
	strcat (dataPath, "\\rom.dat");

	PRINT (dataPath);	

	FILE* f;
	f = fopen (dataPath, "rb");
	if (f == NULL)
	{
		BREAK ("FILE ROM.DAT NOT FOUND");
		return;
	}

	if (fread (VAR(Data), 1, DATA_SIZE, f) == 0)
	{
		BREAK ("CANT READ DATA FROM FILE ROM.DAT");
		fclose (f);
		return;	
	}

	fclose (f);

	VAR (DataByte) = 0;
	VAR (Addr) = 0;
	VAR (State) = STATE_READ_ADDR;
	VAR (Iterations) = 0;
	VAR (LastTime) = -1;

}

void On_simulation_end()
//**********************
// Undo here the operations done at On_simulation_begin: free memory, close
// files, etc.
{

}

void On_digital_in_edge(PIN pDigitalIn, EDGE pEdge, double pTime)
//**************************************************************
// Response to a digital input pin edge. The EDGE type parameter (pEdge) can
// be RISE or FALL. Use pin identifers as declared in DECLARE_PINS
{
	char s [1024];

	if (pTime == VAR (LastTime))
		return;

	VAR (LastTime) = pTime;
	
	if ((pDigitalIn == SCK) && (pEdge == RISE))
	{
		if (VAR (State) == STATE_PASS_TICK)
		{
			VAR (State) = STATE_READ_ADDR;
		}
		else if (VAR (State) == STATE_READ_ADDR)
		{
			VAR (Addr) <<= 1;
			VAR (Addr) |= GET_LOGIC (MOSI);

			#ifdef _DEBUG_
			if (GET_LOGIC (MOSI) == 0)
				sprintf (s, "Addr bit 0");
			else
				sprintf (s, "Addr bit 1");
			PRINT (s);
			#endif

			VAR (Iterations) = VAR (Iterations) + 1;
			if (VAR (Iterations) == 16)
			{	
				if (VAR (Addr) >= DATA_SIZE)
				{
					BREAK ("UNACESSABLE ADDRESS");
					return;
				}

				#ifdef _DEBUG_
				sprintf (s, "Addr %d", VAR (Addr));
				PRINT (s);
				#endif
				
				VAR (DataByte) = VAR (Data) [VAR (Addr)];
				VAR (State) = STATE_WRITE_DATA;	
				VAR (Iterations) = 0;
			}	
		}
		if (VAR (State) == STATE_WRITE_DATA)
		{
			if (VAR (DataByte) & 0x80)
			{
				SET_LOGIC (MISO, 1, 0); //100.0e-6
				#ifdef _DEBUG_
				sprintf (s, "Data bit 1");
				PRINT (s);
				#endif
			}
			else
			{
				SET_LOGIC (MISO, 0, 0);
				#ifdef _DEBUG_
				sprintf (s, "Data bit 0");
				PRINT (s);
				#endif
			}

			VAR (DataByte) <<= 1;
			VAR (Iterations) = VAR (Iterations) + 1;

			if (VAR (Iterations) == 8)
			{			
				#ifdef _DEBUG_
				sprintf (s, "Data %d", VAR (Data) [VAR (Addr)]);
				PRINT (s);
				#endif

				VAR (Iterations) = 0;
				VAR (DataByte) = 0;
				VAR (State) = STATE_PASS_TICK;	
				VAR (Addr) = 0;
			}
		}	
	}

}

double On_voltage_ask(PIN pAnalogOut, double pTime)
//**************************************************
// Return voltage as a function of Time for analog outputs that must behave
// as a continuous time wave.
// SET_VOLTAGE(), SET_LOGIC()etc. not allowed here. Return KEEP_VOLTAGE if
// no changes. Use pin identifers as declared in DECLARE_PINS
{
   return KEEP_VOLTAGE;
}

void On_time_step(double pTime)
//*****************************
// The analysis at the given time has finished. DO NOT place further actions
// on pins (unless they are delayed). Pins values are stable at this point.
{

}

void On_remind_me(double pTime, int pData)
//***************************************
// VMLAB notifies about a previouly sent REMIND_ME() function.
{

}

void On_gadget_notify(GADGET pGadgetId, int pCode)
//************************************************
// A window gadget (control) is sending a notification.
{

}

