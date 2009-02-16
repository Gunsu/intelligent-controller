// ChildView.h : interface of the CChildView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_CHILDVIEW_H__5F3DCA81_21C3_482B_A0DB_68824C1B3DC3__INCLUDED_)
#define AFX_CHILDVIEW_H__5F3DCA81_21C3_482B_A0DB_68824C1B3DC3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "DrawPanel.h"
#include "Project.h"

/////////////////////////////////////////////////////////////////////////////
// CChildView window

class CChildView : public CWnd
{
// Construction
public:
	CChildView();

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CChildView)
	protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//}}AFX_VIRTUAL

// Implementation
public:
	CDrawPanel drawPanel;
	CToolPanel toolPanel;
	bool toolPanelEnabled;

	virtual ~CChildView();

	// Generated message map functions
protected:
	//{{AFX_MSG(CChildView)
	afx_msg int OnCreate (LPCREATESTRUCT crs);
	afx_msg  void OnSize (UINT, int cx, int cy);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

public:
	void OnUpdateToolAddBlock (CCmdUI* pCmdUI);
	void OnUpdateToolAddPoint (CCmdUI* pCmdUI);
	void OnUpdateToolCompile (CCmdUI* pCmdUI);
	void OnUpdateSaveProject (CCmdUI* pCmdUI);
	void OnUpdateToolPanel (CCmdUI* pCmdUI);
	void OnSetToolAddBlock ();
	void OnSetToolAddPoint ();
	void OnCompile ();
	void OnNewProject ();
	void OnOpenProject ();
	void OnSaveProject ();
	void OnToolPanel ();
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CHILDVIEW_H__5F3DCA81_21C3_482B_A0DB_68824C1B3DC3__INCLUDED_)
