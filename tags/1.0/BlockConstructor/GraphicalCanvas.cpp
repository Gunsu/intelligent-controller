#include "StdAfx.h"
#include "GraphicalCanvas.h"

GraphicalCanvas* GraphicalCanvas::graphicalCanvas = NULL;

/*******************************
*        OpenGLCanvas
********************************/

OpenGLCanvas::OpenGLCanvas (HDC hdc) : mulCoef (100.0)
{
	this->hdc = hdc;
	GLInitialize (hdc);
	SetMaxSize (GraphicalPoint (3000, 3000));
	SetVScroll (0.5);
	SetHScroll (0.5);
}

OpenGLCanvas::~OpenGLCanvas ()
{
	GLUnInitialize ();
}

void OpenGLCanvas::GLSetPixelFormatDescriptor (HDC hdc)
{
	int pixelFormat;

	PIXELFORMATDESCRIPTOR pfd = {
		sizeof (PIXELFORMATDESCRIPTOR),  // size of this pfd
		1,								// version number
		PFD_DRAW_TO_WINDOW |
		PFD_SUPPORT_OPENGL |
		PFD_GENERIC_ACCELERATED |
		PFD_DOUBLEBUFFER,
		PFD_TYPE_RGBA,					// RGBA type
		24,								// 24-bit color depth
		0, 0, 0, 0, 0, 0,				// color bits ignored
		0,								// no alpha buffer
		0,								// shift bit ignored
		0,								// no accumulation buffer
		0, 0, 0, 0,						// accum bits ignored
		0,								// 0-bit z-buffer
		0,								// no stencil buffer
		0,								// no auxiliary buffer
		PFD_MAIN_PLANE,					// main layer
		0,								// reserved
		0, 0, 0							// layer masks ignored
	};
	pixelFormat = ChoosePixelFormat (hdc, &pfd);
	SetPixelFormat (hdc, pixelFormat, &pfd);
}

void OpenGLCanvas::GLInitialize (HDC hdc)
{
	GLSetPixelFormatDescriptor (hdc);
	glrc = wglCreateContext (hdc); 
	wglMakeCurrent (hdc, glrc);
	GLLoadFont ();
}

void OpenGLCanvas::GLUnInitialize ()
{
	GLUnloadFont ();
	wglMakeCurrent (0, 0);
	wglDeleteContext (glrc);
}

void OpenGLCanvas::GLSetViewport (const int W, const int H)
{	
	GraphicalPoint size (canvasSize / 2.0);

	glMatrixMode (GL_PROJECTION);
	glLoadIdentity ();
	glViewport (0, 0, W, H);
	glOrtho (-size.x, size.x, -size.y, size.y, -1.0, 0.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity ();
}

void OpenGLCanvas::Flush ()
{
	SwapBuffers (hdc);
	glClearColor (0.95, 0.95, 0.95, 0);
	glClear (GL_COLOR_BUFFER_BIT);
}

void OpenGLCanvas::SetVisibleSize (GraphicalPoint &size)
{
	this->realSize = size;
	this->canvasSize = realSize / mulCoef;
	GLSetViewport ((int) size.x, (int) size.y);
	SetVScroll (this->scroll.y);
	SetHScroll (this->scroll.x);
}

void OpenGLCanvas::SetMaxSize (GraphicalPoint &size)
{
	this->maxRealSize = size;
	this->maxCanvasSize = maxRealSize / mulCoef;
}

void OpenGLCanvas::SetVScroll (double vScroll)
{
	if (vScroll > 1)
		vScroll = 1;
	else if (vScroll < 0)
		vScroll = 0;
	
	double delta = maxCanvasSize.y - canvasSize.y;
	if (delta < 0)
		delta = 0;
	this->scroll.y = vScroll;
	translate.y = (scroll.y - 0.5) * delta;
}

void OpenGLCanvas::SetHScroll (double hScroll)
{
	if (hScroll > 1)
		hScroll = 1;
	else if (hScroll < 0)
		hScroll = 0;
	
	double delta = maxCanvasSize.x - canvasSize.x;
	if (delta < 0)
		delta = 0;
	this->scroll.x = hScroll;
	translate.x = -(scroll.x - 0.5) * delta;
}

inline GraphicalPoint OpenGLCanvas::GetCanvasPoint (GraphicalPoint &realPoint)
{
	GraphicalPoint canvasPoint = (realPoint / mulCoef) - (canvasSize / 2.0) + (scroll - 0.5) * (maxCanvasSize - canvasSize);
	canvasPoint.y *= -1;
	return canvasPoint;
}

inline GraphicalPoint OpenGLCanvas::GetRealPoint (GraphicalPoint &canvasPoint)
{
	return canvasPoint;
}

GraphicalPoint OpenGLCanvas::GetMaxCanvasSize ()
{
	return this->maxCanvasSize;
}

GraphicalPoint OpenGLCanvas::GetScroll ()
{
	return scroll;
}

void OpenGLCanvas::GLLoadFont ()
{
	static LOGFONT logFont = {-10, 0, 0, 0, FW_NORMAL, 0, 0, 0, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS, NONANTIALIASED_QUALITY, FF_MODERN, "Arial Cyr"};
	HFONT hFontNew;
	HFONT hFontOld;
		
	hFontNew = CreateFontIndirect (&logFont);
	hFontOld = (HFONT) SelectObject (hdc, hFontNew);
	DeleteObject (hFontOld);

	wglUseFontBitmaps (hdc, 0, 255, GLF_START_LIST);
}

void OpenGLCanvas::GLUnloadFont ()
{
	glDeleteLists (GLF_START_LIST, 255);
}

void OpenGLCanvas::OutText (char* text, int length, Allign allign, GraphicalPoint &pos, GraphicalColor &color)
{
	static const int bufLength = 1024;
	static char buf[bufLength];
	int l = Min (Min (strlen (text), length), bufLength);
	int i;
	char* p = text;

	if (l < length)
	{
		if (allign == Allign::alCenter)
		{
			for (i = 0; i < (length - l) / 2; i++)
				buf[i] = ' ';
			strcpy (buf + i, text);
			length = strlen (buf);
		}
		else if (allign == Allign::alRight)
		{
			for (i = 0; i < (length - l); i++)
				buf[i] = ' ';
			strcpy (buf + i, text);
		}
		else if (allign == Allign::alLeft)
		{
			strcpy (buf, text);
			for (i = l; i < length; i++)
				buf[i] = ' ';
		}
		p = buf;
	}
	
	glColor3f (color.r, color.g, color.b);
	glRasterPos2f (pos.x + translate.x, pos.y + translate.y);
	glListBase (GLF_START_LIST);
	glCallLists (length, GL_UNSIGNED_BYTE, p);
}

void OpenGLCanvas::Circle (GraphicalPoint& center, double R, const GraphicalColor &color)
{
	glPushMatrix ();
	glTranslatef (translate.x, translate.y, 0);
	//glEnable (GL_POINT_SMOOTH);
	glPointSize (R * this->mulCoef);
	glBegin (GL_POINTS);
		glColor3f (color.r, color.g, color.b);
		glVertex2f (center.x, center.y);
	glEnd ();
	//glDisable (GL_POINT_SMOOTH);
	glPopMatrix ();
}

void OpenGLCanvas::Rectangle (GraphicalPoint& upperLeft, GraphicalPoint& size, GraphicalColor &color)
{
	glPushMatrix ();
	glTranslatef (translate.x, translate.y, 0);
	glBegin (GL_TRIANGLE_STRIP);
		glColor3f (color.r, color.g, color.b);
		glVertex2f (upperLeft.x, upperLeft.y);
		glVertex2f (upperLeft.x + size.x, upperLeft.y);
		glVertex2f (upperLeft.x, upperLeft.y + size.y);
		glVertex2f (upperLeft.x + size.x, upperLeft.y + size.y);
	glEnd ();
	glPopMatrix ();
}

void OpenGLCanvas::Box (GraphicalPoint &upperLeft, GraphicalPoint &size, double fat, GraphicalColor &color)
{
	glPushMatrix ();
	glTranslatef (translate.x, translate.y, 0);
	glLineWidth (fat * this->mulCoef);
	glBegin (GL_LINE_STRIP);
		glColor3f (color.r, color.g, color.b);
		glVertex2f (upperLeft.x, upperLeft.y);
		glVertex2f (upperLeft.x + size.x, upperLeft.y);
		glVertex2f (upperLeft.x + size.x, upperLeft.y + size.y);
		glVertex2f (upperLeft.x, upperLeft.y + size.y);
		glVertex2f (upperLeft.x, upperLeft.y);
	glEnd ();
	glPopMatrix ();
}

void OpenGLCanvas::Line (GraphicalPoint &from, GraphicalPoint &to, double fat, GraphicalColor &color)
{
	glPushMatrix ();
	glTranslatef (translate.x, translate.y, 0);
	glLineWidth (fat * this->mulCoef);
	glBegin(GL_LINE_STRIP);
		glColor3f(color.r, color.g, color.b);
		glVertex2f(from.x, from.y);
		glVertex2f(to.x, to.y);
	glEnd();
	glPopMatrix ();
}

/*******************************
*        WinAPICanvas
********************************/
/*
WinAPICanvas::WinAPICanvas (HDC bufDC, HDC DC, GraphicalPoint &size)
{
	this->bufDC = bufDC;
	this->DC = DC;
	this->size = size;
}

void WinAPICanvas::Flush ()
{
	::BitBlt (DC, 0, 0, (int) size.x, (int) size.y, bufDC, 0, 0, SRCCOPY);
	// clear buffer
	::Rectangle(bufDC, 0, 0, (int) size.x, (int) size.y);
}

void WinAPICanvas::SetSize (GraphicalPoint &size)
{
	this->size = size;
}

void WinAPICanvas::Circle (GraphicalPoint& center, double R)
{
	GraphicalPoint rPoint(R, R);
	GraphicalPoint point1 = GetRealPoint (center - rPoint);
	GraphicalPoint point2 = GetRealPoint (center + rPoint);
	::Ellipse(bufDC, Round(point1.x), Round(point1.y), Round(point2.x), Round(point2.y));
}

void WinAPICanvas::Rectangle (GraphicalPoint& upperLeft, GraphicalPoint& size)
{
	GraphicalPoint point1 = GetRealPoint (upperLeft);
	GraphicalPoint point2 = GetRealPoint (upperLeft + size);
	::Rectangle(bufDC, Round(point1.x), Round(point1.y), Round(point2.x), Round(point2.y));
}

void WinAPICanvas::Line (GraphicalPoint &from, GraphicalPoint &to)
{
	GraphicalPoint point1 = GetRealPoint (from);
	GraphicalPoint point2 = GetRealPoint (to);
	::MoveToEx (bufDC, Round(point1.x), Round(point1.y), 0);
	::LineTo (bufDC, Round(point2.x), Round(point2.y));
}
*/