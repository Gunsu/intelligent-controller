#ifndef GRAPHICAL_CANVAS_H
#define GRAPHICAL_CANVAS_H

#include <windows.h>
#include <math.h>
#include <gl/gl.h>
#include "Common.h"

#define GLF_START_LIST 0

/*******************************
*        GraphicalPoint
********************************/

struct GraphicalPoint
{
	double x;
	double y;
	inline GraphicalPoint () : x (0), y (0)  {}
	inline GraphicalPoint (double X, double Y) : x (X), y (Y) {}
	inline GraphicalPoint operator + (GraphicalPoint& point) {return GraphicalPoint (x + point.x, y + point.y);}
	inline GraphicalPoint operator - (GraphicalPoint& point) {return GraphicalPoint (x - point.x, y - point.y);}
	inline GraphicalPoint operator - (double num) {return GraphicalPoint (x - num, y - num);}
	inline GraphicalPoint operator / (GraphicalPoint& point) {return GraphicalPoint (x / point.x, y / point.y);}
	inline GraphicalPoint operator / (double num) {return GraphicalPoint (x / num, y / num);}
	inline GraphicalPoint operator * (GraphicalPoint& point) {return GraphicalPoint (x * point.x, y * point.y);}
	inline GraphicalPoint operator * (double num) {return GraphicalPoint (x * num, y * num);}
	inline bool operator > (GraphicalPoint& point) {return (x > point.x) && (y > point.y);}
	inline bool operator < (GraphicalPoint& point) {return (x < point.x) && (y < point.y);}
	inline double Mod () {return sqrt (x * x + y * y);}
};

/*******************************
*        GraphicalColor
********************************/

struct GraphicalColor
{
	double r;
	double g;
	double b;
	GraphicalColor (double R, double G, double B) : r(R), g(G), b(B) {}
};

/*******************************
*        GraphicalCanvas
********************************/

enum Allign
{
	alCenter = 0,
	alRight,
	alLeft
};

class GraphicalCanvas
{
	protected:
		static GraphicalCanvas* graphicalCanvas;

	public:
		static void SetCanvas (GraphicalCanvas* canvas) {graphicalCanvas = canvas;}
		static GraphicalCanvas* GetCanvas () {return graphicalCanvas;}

	public: 
		virtual GraphicalPoint GetCanvasPoint (GraphicalPoint &realPoint) = 0;
		virtual GraphicalPoint GetRealPoint (GraphicalPoint &canvasPoint) = 0;
		virtual void SetVisibleSize (GraphicalPoint &size) = 0;
		virtual void SetMaxSize (GraphicalPoint &size) = 0;
		virtual GraphicalPoint GetMaxCanvasSize () = 0;
		virtual GraphicalPoint GetScroll () = 0;
		virtual void SetVScroll (double vScroll) = 0;
		virtual void SetHScroll (double hScroll) = 0;
		virtual void Flush () = 0;
		virtual void Circle (GraphicalPoint &center, double R, const GraphicalColor &color) = 0;
		virtual void Rectangle (GraphicalPoint &upperLeft, GraphicalPoint &size, GraphicalColor &color) = 0;
		virtual void Box (GraphicalPoint &upperLeft, GraphicalPoint &size, double fat, GraphicalColor &color) = 0;
		virtual void Line (GraphicalPoint &from, GraphicalPoint &to, double fat, GraphicalColor &color) = 0;
		virtual void OutText (char* text, int length, Allign allign, GraphicalPoint &pos, GraphicalColor &color) = 0;
};

/*******************************
*        OpenGLCanvas
********************************/

class OpenGLCanvas : public GraphicalCanvas
{
	protected:
		const double mulCoef;
		HDC hdc;
		HGLRC glrc;
		GraphicalPoint realSize;
		GraphicalPoint canvasSize;
		GraphicalPoint maxRealSize;
		GraphicalPoint maxCanvasSize;
		GraphicalPoint scroll;
		GraphicalPoint translate;
		void GLSetPixelFormatDescriptor (HDC hdc);
		void GLInitialize (HDC hdc);
		void GLUnInitialize ();
		void GLSetViewport (const int W, const int H);
		void GLLoadFont ();
		void GLUnloadFont ();

	public:
		OpenGLCanvas (HDC hdc);
		~OpenGLCanvas ();
		void SetVisibleSize (GraphicalPoint &size);
		void SetMaxSize (GraphicalPoint &size);
		GraphicalPoint GetMaxCanvasSize ();
		GraphicalPoint GetScroll ();
		void SetVScroll (double vScroll);
		void SetHScroll (double hScroll);
		GraphicalPoint GetCanvasPoint (GraphicalPoint &realPoint);
		GraphicalPoint GetRealPoint (GraphicalPoint &canvasPoint);
		void Flush ();
		void Circle (GraphicalPoint &center, double R, const GraphicalColor &color);
		void Rectangle (GraphicalPoint &upperLeft, GraphicalPoint &size, GraphicalColor &color);
		void Box (GraphicalPoint &upperLeft, GraphicalPoint &size, double fat, GraphicalColor &color);
		void Line (GraphicalPoint &from, GraphicalPoint &to, double fat, GraphicalColor &color);
		void OutText (char* text, int length, Allign allign, GraphicalPoint &pos, GraphicalColor &color);
};

/*
class WinAPICanvas : public GraphicalCanvas
{
	protected:
		HDC bufDC;
		HDC DC;
		GraphicalPoint addTransform;
		GraphicalPoint mulTransform;
		GraphicalPoint size;

	public:
		WinAPICanvas (HDC bufDC, HDC DC, GraphicalPoint &size);
		void SetSize (GraphicalPoint &size);
		void Flush ();
		void Circle (GraphicalPoint &center, double R);
		void Rectangle (GraphicalPoint &upperLeft, GraphicalPoint &size);
		void Line (GraphicalPoint &from, GraphicalPoint &to);
};
*/

#endif