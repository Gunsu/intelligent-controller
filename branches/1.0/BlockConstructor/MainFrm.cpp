// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "BlockConstructor.h"
#include "Exceptions.h"

#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

void objectSelectedCallback (GraphicalObject* object)
{
	CMainFrame* mainFrame = (CMainFrame*)AfxGetApp()->m_pMainWnd;
	std::string info1 = "Объект:  ";
	std::string info2 = "Имя:  ";
	std::string info3 = "Тип данных:  ";

	if (object != NULL)
	{
		switch (object->GetType ())
		{
			case ObjectType::otBlock:
				info1 += "Блок";
				info2 += ((Block*) object)->GetName ();
				break;
			case ObjectType::otInputCommandBlock: 
				info1 += "Блок входной команды";
				info2 += ((InputCommandBlock*) object)->GetName ();
				break;
			case ObjectType::otOutputCommandBlock: 
				info1 += "Блок выходной команды";
				info2 += ((OutputCommandBlock*) object)->GetName ();
				break;
			case ObjectType::otOutputCommandConstBlock: 
				info1 += "Блок выходной команды (константа)";
				info2 += ((OutputCommandBlock*) object)->GetName ();
				break;
			case ObjectType::otOutputCommandBufBlock: 
				info1 += "Блок выходной команды (буфер)";
				info2 += ((OutputCommandBlock*) object)->GetName ();
				break;
			case ObjectType::otConnectionPoint: 
				info1 += "Точка соединения";
				info3 += IntToString (((ConnectionPoint*) object)->GetDataType ()) + "Б";
				break;
			case ObjectType::otBlockInputPoint: 
				info1 += "Вход блока";
				info2 += ((BlockInputPoint*) object)->GetName ();
				if (((BlockInputPoint*) object)->GetDataType () >= 0)
					info3 += IntToString (((BlockInputPoint*) object)->GetDataType ()) + "Б";
				else
					info3 += "Соединение блоков команд";
				break;
			case ObjectType::otBlockOutputPoint: 
				info1 += "Выход блока ";
				info2 += ((BlockOutputPoint*) object)->GetName ();
				if (((BlockOutputPoint*) object)->GetDataType () >= 0)
					info3 += IntToString (((BlockOutputPoint*) object)->GetDataType ()) + "Б";
				else
					info3 += "Соединение блоков команд";
				break;
		}
	}

	mainFrame->m_wndStatusBar.GetStatusBarCtrl ().SetText (info1.c_str (), 0, 0);
	mainFrame->m_wndStatusBar.GetStatusBarCtrl ().SetText (info2.c_str (), 1, 0);
	mainFrame->m_wndStatusBar.GetStatusBarCtrl ().SetText (info3.c_str (), 2, 0);
}

void scrollCallback (GraphicalPoint &scroll)
{
	CMainFrame* mainFrame = (CMainFrame*)AfxGetApp()->m_pMainWnd;
	mainFrame->m_wndView.drawPanel.SetScrollPos (SB_HORZ, scroll.x * 100);
	mainFrame->m_wndView.drawPanel.SetScrollPos (SB_VERT, scroll.y * 100);
}

void projectClosedCallback ()
{
	CMainFrame* mainFrame = (CMainFrame*)AfxGetApp()->m_pMainWnd;
	mainFrame->m_wndView.toolPanel.ReloadBlockTypeBox ();
	mainFrame->m_wndView.toolPanel.ReloadSchemasBox ();
	mainFrame->m_wndView.toolPanel.Enable (false);
	mainFrame->m_wndView.drawPanel.RedrawWindow ();
}

void projectOpenedCallback (std::string projectFilePath)
{
	CMainFrame* mainFrame = (CMainFrame*)AfxGetApp()->m_pMainWnd;
	mainFrame->m_wndView.toolPanel.ReloadBlockTypeBox ();
	mainFrame->m_wndView.toolPanel.ReloadSchemasBox ();
	mainFrame->m_wndView.toolPanel.Enable (true);
	mainFrame->m_wndView.drawPanel.RedrawWindow ();
}

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	//{{AFX_MSG_MAP(CMainFrame)
	ON_WM_CREATE()
	ON_WM_SETFOCUS()
	ON_WM_SIZE ()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

static int statusBarWidths [3] = 
{
	250, 
	500, 
	750,
};

static UINT BASED_CODE tools [] =
{
	ID_TOOL_NEW,
	ID_TOOL_OPEN,
	ID_TOOL_SAVE,
	ID_TOOL_PANEL,
	ID_SEPARATOR,
	ID_TOOL_ADD_BLOCK,
	ID_TOOL_ADD_POINT,
	ID_SEPARATOR,
	ID_TOOL_COMPILE
};

/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
}

CMainFrame::~CMainFrame()
{
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	SchemaManager* schemaManager;
	
	if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	m_wndView.Create(NULL, NULL, AFX_WS_DEFAULT_VIEW, CRect(0, 0, 0, 0), this, AFX_IDW_PANE_FIRST, NULL);
	m_wndView.toolPanel.Enable (false);
	
	m_wndToolBar.CreateEx (this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP | CBRS_GRIPPER | CBRS_SIZE_DYNAMIC | CBRS_TOOLTIPS | CBRS_FLYBY);
	m_wndToolBar.LoadToolBar (IDR_MAINFRAME_1);
	m_wndToolBar.SetButtons (tools, 9);
	m_wndToolBar.SetButtonStyle (4, TBBS_SEPARATOR);
	m_wndToolBar.SetButtonStyle (5, TBBS_CHECKGROUP);
	m_wndToolBar.SetButtonStyle (6, TBBS_CHECKBOX | TBBS_GROUP);
	m_wndToolBar.SetButtonStyle (7, TBBS_SEPARATOR);

	m_wndStatusBar.Create (this);
	m_wndStatusBar.GetStatusBarCtrl ().SetParts (3, statusBarWidths);

	m_wndStatusBar.GetStatusBarCtrl ().SetText ("Тип объекта:", 0, 0);
	m_wndStatusBar.GetStatusBarCtrl ().SetText ("Имя:", 1, 0);
	m_wndStatusBar.GetStatusBarCtrl ().SetText ("Тип данных:", 2, 0);

	m_wndToolBar.EnableDocking(CBRS_ALIGN_ANY);
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndToolBar);

	schemaManager = SchemaManager::GetSchemaManager();

	Project::GetProject ()->SetProjectClosedCallback (projectClosedCallback);
	Project::GetProject ()->SetProjectOpenedCallback (projectOpenedCallback);
	SchemaManager::GetSchemaManager ()->SetObjectSelectedCallback (objectSelectedCallback);
	SchemaManager::GetSchemaManager ()->SetScrollCallback (scrollCallback);

	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	cs.dwExStyle &= ~WS_EX_CLIENTEDGE;
	cs.lpszClass = AfxRegisterWndClass(0);
	return TRUE;
}

void CMainFrame::OnSize (UINT type, int cx, int cy)
{
	CFrameWnd::OnSize (type, cx, cy);

	statusBarWidths [2] = cx;
	m_wndStatusBar.GetStatusBarCtrl ().SetParts (3, statusBarWidths);
}


/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers
void CMainFrame::OnSetFocus(CWnd* pOldWnd)
{
	// forward focus to the view window
	m_wndView.SetFocus();
}

BOOL CMainFrame::OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo)
{
	// let the view have first crack at the command
	if (m_wndView.OnCmdMsg(nID, nCode, pExtra, pHandlerInfo))
		return TRUE;

	// otherwise, do default handling
	return CFrameWnd::OnCmdMsg(nID, nCode, pExtra, pHandlerInfo);
}

