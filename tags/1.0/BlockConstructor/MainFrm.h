// MainFrm.h : interface of the CMainFrame class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINFRM_H__0589C5FA_76CC_423C_8C5F_A0FBFEC2887A__INCLUDED_)
#define AFX_MAINFRM_H__0589C5FA_76CC_423C_8C5F_A0FBFEC2887A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "ChildView.h"

void objectSelectedCallback (GraphicalObject* object);
void scrollCallback (GraphicalPoint &scroll);

class CMainFrame : public CFrameWnd
{
	friend void objectSelectedCallback (GraphicalObject* object);
	friend void scrollCallback (GraphicalPoint &scroll);
	friend void projectClosedCallback ();
	friend void projectOpenedCallback (std::string projectFilePath);
	
public:
	CMainFrame();
protected: 
	DECLARE_DYNAMIC(CMainFrame)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainFrame)
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);
	//}}AFX_VIRTUAL

// Implementation
public:
	CStatusBar m_wndStatusBar;
	CToolBar m_wndToolBar;

	virtual ~CMainFrame();

protected:  // control bar embedded members
	CChildView m_wndView;

// Generated message map functions
protected:
	//{{AFX_MSG(CMainFrame)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSetFocus(CWnd *pOldWnd);
	void OnSize (UINT, int, int);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINFRM_H__0589C5FA_76CC_423C_8C5F_A0FBFEC2887A__INCLUDED_)
