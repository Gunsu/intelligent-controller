#ifndef DRAW_PANEL_H
#define DRAW_PANEL_H

#include "Blocks.h"
#include "BlockConstructor.h"
#include "SchemaManager.h"
#include "ToolPanel.h"

class CDrawPanel: public CWnd
{
	protected:
		//GraphicalCanvas* canvas;

		int OnCreate (LPCREATESTRUCT lpCrs);
		void OnPaint ();
		void OnSize (UINT type, int cx, int cy);
		void OnClose ();
		void OnSetToolAddBlock ();
		void OnLButtonDown (UINT nFlags, CPoint point);
		void OnRButtonDown (UINT nFlags, CPoint point);
		void OnMouseMove (UINT nFlags, CPoint point);
		void OnLButtonUp (UINT nFlags, CPoint point);
		void OnLButtonDblClk (UINT nFlags, CPoint point);
		void OnRButtonDblClk (UINT nFlags, CPoint point);
		void OnKeyDown (UINT nChar, UINT nRepCnt, UINT nFlags);
		void OnChar (UINT nChar, UINT nRepCnt, UINT nFlags);
		void OnHScroll (UINT code, UINT pos, CScrollBar* sb);
		void OnVScroll (UINT code, UINT pos, CScrollBar* sb);

		DECLARE_MESSAGE_MAP () 

	public:
		CDrawPanel ();
		BOOL Create (CWnd* pParentWnd);
		void SetWindowRect (CRect &rect);
		//GraphicalCanvas* GetGraphicalCanvas () {return canvas;}
};

#endif