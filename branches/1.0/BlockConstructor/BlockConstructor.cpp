// BlockConstructor.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "BlockConstructor.h"

#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CBlockConstructorApp

BEGIN_MESSAGE_MAP(CBlockConstructorApp, CWinApp)
	//{{AFX_MSG_MAP(CBlockConstructorApp)
	ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBlockConstructorApp construction

CBlockConstructorApp::CBlockConstructorApp()
{
	CoInitialize (NULL);
}

CBlockConstructorApp::~CBlockConstructorApp()
{
	CoUninitialize ();
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CBlockConstructorApp object

CBlockConstructorApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CBlockConstructorApp initialization

BOOL CBlockConstructorApp::InitInstance()
{
	// Standard initialization

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif

	// Change the registry key under which our settings are stored.
	SetRegistryKey(_T("Block Constructor"));


	CMainFrame* pFrame = new CMainFrame;
	m_pMainWnd = pFrame;

	// create and load the frame with its resources

	pFrame->LoadFrame(IDR_MAINFRAME,
		WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, NULL,
		NULL);

	m_pMainWnd->SetWindowText("Редактор схем");
	m_pMainWnd->SetIcon ((HICON)LoadImage (m_hInstance, (char*)IDR_MAINFRAME, IMAGE_ICON, 32, 32, 0), 1);
	m_pMainWnd->SetIcon ((HICON)LoadImage (m_hInstance, (char*)IDR_MAINFRAME, IMAGE_ICON, 16, 16, 0), 0);

	pFrame->ShowWindow(SW_SHOW);
	pFrame->UpdateWindow();

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CBlockConstructorApp message handlers


// App command to run the dialog
void CBlockConstructorApp::OnAppAbout()
{
}