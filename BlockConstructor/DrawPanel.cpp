#include "stdafx.h"
#include "DrawPanel.h"

BEGIN_MESSAGE_MAP (CDrawPanel, CWnd)
	ON_WM_CREATE()
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_WM_QUERYDRAGICON()
	ON_WM_CLOSE()
	ON_WM_LBUTTONDOWN()
	ON_WM_RBUTTONDOWN()
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONUP()
	ON_WM_LBUTTONDBLCLK()
	ON_WM_KEYDOWN()
	ON_WM_CHAR()
	ON_WM_RBUTTONDBLCLK()
	ON_WM_HSCROLL ()
	ON_WM_VSCROLL ()
END_MESSAGE_MAP ()

CDrawPanel::CDrawPanel ()
{
}

BOOL CDrawPanel::Create (CWnd* pParentWnd)
{
	return CWnd::Create (0, 0, AFX_WS_DEFAULT_VIEW | WS_HSCROLL | WS_VSCROLL, CRect(0, 0, 500, 500), pParentWnd, AFX_IDW_PANE_FIRST);
}

void CDrawPanel::SetWindowRect (CRect &rect)
{
}

int CDrawPanel::OnCreate (LPCREATESTRUCT lpCrs)
{
	int res = CWnd::OnCreate (lpCrs);

	GraphicalCanvas::SetCanvas (new OpenGLCanvas (this->GetDC ()->m_hDC));
	GraphicalCanvas::GetCanvas()->SetMaxSize (GraphicalPoint (3000, 3000));

	SetScrollRange (SB_HORZ, 0, 100);
	SetScrollRange (SB_VERT, 0, 100);

	SetScrollPos(SB_HORZ, 50);
	SetScrollPos(SB_VERT, 50);

	return res;
}

void CDrawPanel::OnPaint() 
{
	CWnd::OnPaint();
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->Draw ();
	else
		GraphicalCanvas::GetCanvas()->Flush();
}

void CDrawPanel::OnSize (UINT type, int cx, int cy)
{
	CRect clientRect;
	GetClientRect (&clientRect);
	GraphicalCanvas::GetCanvas()->SetVisibleSize (GraphicalPoint (clientRect.Width(), clientRect.Height()));
	OnPaint ();
}

void CDrawPanel::OnClose() 
{
	if (GraphicalCanvas::GetCanvas() != NULL)
		delete GraphicalCanvas::GetCanvas();

	CWnd::OnClose();
}

void CDrawPanel::OnLButtonDown(UINT nFlags, CPoint point) 
{
	this->SetFocus ();
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->OnLBDown (GraphicalPoint (point.x, point.y));
	CWnd::OnLButtonDown(nFlags, point);
}

void CDrawPanel::OnRButtonDown(UINT nFlags, CPoint point) 
{
	this->SetFocus ();
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->OnRBDown (GraphicalPoint (point.x, point.y));
	CWnd::OnRButtonDown(nFlags, point);
}

void CDrawPanel::OnMouseMove(UINT nFlags, CPoint point) 
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->OnMouseMove (GraphicalPoint (point.x, point.y));
	CWnd::OnMouseMove(nFlags, point);
}

void CDrawPanel::OnLButtonUp(UINT nFlags, CPoint point) 
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->OnLBUp (GraphicalPoint (point.x, point.y));
	CWnd::OnLButtonUp(nFlags, point);
}

void CDrawPanel::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
	this->SetFocus ();
	CWnd::OnLButtonDblClk(nFlags, point);
}

void CDrawPanel::OnRButtonDblClk(UINT nFlags, CPoint point) 
{
	this->SetFocus ();
	CWnd::OnRButtonDblClk(nFlags, point);
}

void CDrawPanel::OnKeyDown (UINT nChar, UINT nRepCnt, UINT nFlags)
{
	if (nChar == 46)
	{
		if (SchemaManager::GetSchemaManager()->GetCurSchema ())
			SchemaManager::GetSchemaManager()->GetCurSchema ()->OnDelKeyPressed ();
	}
	//CWnd::OnKeyDown (nChar, nRepCnt, nFlags);
}

void CDrawPanel::OnChar (UINT nChar, UINT nRepCnt, UINT nFlags)
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->OnKeyPressed (nChar);
}

void CDrawPanel::OnHScroll (UINT code, UINT pos, CScrollBar* sb)
{
	int curPos = GetScrollPos (SB_HORZ);
	switch (code)
	{
		case SB_PAGELEFT:
		case SB_LINELEFT:
			curPos -= 1;
			break;
		case SB_PAGERIGHT:
		case SB_LINERIGHT:
			curPos += 1;
			break;
		case SB_THUMBTRACK:
			curPos = pos;
			break;
	}
	SetScrollPos(SB_HORZ, curPos);
	
	GraphicalCanvas::GetCanvas()->SetHScroll (curPos / 100.0);
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->Draw ();
}


void CDrawPanel::OnVScroll (UINT code, UINT pos, CScrollBar* sb)
{
	int curPos = GetScrollPos (SB_VERT);
	switch (code)
	{
		case SB_PAGEUP:
		case SB_LINEUP:
			curPos -= 1;
			break;
		case SB_PAGEDOWN:
		case SB_LINEDOWN:
			curPos += 1;
			break;
		case SB_THUMBTRACK:
			curPos = pos;
			break;
	}
	SetScrollPos(SB_VERT, curPos);
	
	GraphicalCanvas::GetCanvas()->SetVScroll (curPos / 100.0);
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->Draw ();
}


