// BlockConstructor.h : main header file for the BLOCKCONSTRUCTOR application
//

#if !defined(AFX_BLOCKCONSTRUCTOR_H__18CF9202_B858_4E33_B2FD_BBA0D8B10282__INCLUDED_)
#define AFX_BLOCKCONSTRUCTOR_H__18CF9202_B858_4E33_B2FD_BBA0D8B10282__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CBlockConstructorApp:
// See BlockConstructor.cpp for the implementation of this class
//

class CBlockConstructorApp : public CWinApp
{
public:
	CBlockConstructorApp();
	~CBlockConstructorApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBlockConstructorApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

public:
	//{{AFX_MSG(CBlockConstructorApp)
	afx_msg void OnAppAbout();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BLOCKCONSTRUCTOR_H__18CF9202_B858_4E33_B2FD_BBA0D8B10282__INCLUDED_)
