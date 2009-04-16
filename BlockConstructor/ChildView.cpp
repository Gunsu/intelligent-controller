// ChildView.cpp : implementation of the CChildView class
//

#include "stdafx.h"
#include "BlockConstructor.h"
#include "ChildView.h"
#include "NewProject.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CChildView

CChildView::CChildView()
{
}

CChildView::~CChildView()
{
}


BEGIN_MESSAGE_MAP(CChildView,CWnd )
	//{{AFX_MSG_MAP(CChildView)
	ON_WM_CREATE ()
	ON_WM_SIZE ()
	ON_UPDATE_COMMAND_UI(ID_TOOL_ADD_BLOCK, OnUpdateToolAddBlock)
	ON_UPDATE_COMMAND_UI(ID_TOOL_ADD_POINT, OnUpdateToolAddPoint)
	ON_UPDATE_COMMAND_UI(ID_TOOL_COMPILE, OnUpdateToolCompile)
	ON_UPDATE_COMMAND_UI(ID_TOOL_SAVE, OnUpdateSaveProject)
	ON_UPDATE_COMMAND_UI(ID_TOOL_PANEL, OnUpdateToolPanel)
	ON_COMMAND (ID_TOOL_ADD_BLOCK, OnSetToolAddBlock)
	ON_COMMAND (ID_TOOL_ADD_POINT, OnSetToolAddPoint)
	ON_COMMAND (ID_TOOL_COMPILE, OnCompile)
	ON_COMMAND (ID_TOOL_NEW, OnNewProject)
	ON_COMMAND (ID_TOOL_OPEN, OnOpenProject)
	ON_COMMAND (ID_TOOL_SAVE, OnSaveProject)
	ON_COMMAND (ID_TOOL_PANEL, OnToolPanel)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CChildView message handlers

BOOL CChildView::PreCreateWindow(CREATESTRUCT& cs) 
{
	if (!CWnd::PreCreateWindow(cs))
		return FALSE;

	cs.dwExStyle |= WS_EX_CLIENTEDGE;
	cs.style &= ~WS_BORDER;
	cs.lpszClass = AfxRegisterWndClass(CS_HREDRAW|CS_VREDRAW|CS_DBLCLKS, 
		::LoadCursor(NULL, IDC_ARROW), HBRUSH(COLOR_WINDOW+1), NULL);

	return TRUE;
}

int CChildView::OnCreate (LPCREATESTRUCT crs)
{
	drawPanel.Create (this);
	toolPanel.Create (IDD_TOOL_PANEL, this);
	toolPanel.ShowWindow (SW_SHOW);
	toolPanelEnabled = true;
	return 0;
}

void CChildView::OnSize (UINT, int, int)
{
	int toolPanelWidth;
	RECT rect;

	toolPanelWidth = toolPanelEnabled ? 250 : 5;

	GetClientRect (&rect);
	drawPanel.SetWindowPos (0, toolPanelWidth, 0, rect.right - toolPanelWidth, rect.bottom, SWP_NOZORDER);
	toolPanel.SetWindowPos (0, 0, 0, toolPanelWidth, rect.bottom, SWP_NOZORDER);
}

void CChildView::OnUpdateToolCompile (CCmdUI* pCmdUI)
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
	{
		pCmdUI->Enable (TRUE);
	}
	else
	{
		pCmdUI->Enable (FALSE);
	}
}

void CChildView::OnUpdateSaveProject (CCmdUI* pCmdUI)
{
	if (Project::GetProject ()->IsLoad ())
	{
		pCmdUI->Enable (TRUE);
	}
	else
	{
		pCmdUI->Enable (FALSE);
	}
}

void CChildView::OnUpdateToolAddBlock (CCmdUI* pCmdUI)
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
	{
		pCmdUI->SetCheck((UINT)(SchemaManager::GetSchemaManager()->GetCurSchema ()->GetTool () == Schema::Tool::addBlock));
		pCmdUI->Enable (TRUE);
	}
	else
	{
		pCmdUI->Enable (FALSE);
	}
}

void CChildView::OnUpdateToolAddPoint (CCmdUI* pCmdUI)
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
	{
		pCmdUI->SetCheck((UINT)(SchemaManager::GetSchemaManager()->GetCurSchema ()->GetTool () == Schema::Tool::addPoint));
		pCmdUI->Enable (TRUE);
	}
	else
	{
		pCmdUI->Enable (FALSE);
	}
}

void CChildView::OnUpdateToolPanel (CCmdUI* pCmdUI)
{
	pCmdUI->SetCheck((UINT)(toolPanelEnabled));
	pCmdUI->Enable (TRUE);
}

void CChildView::OnSetToolAddBlock ()
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->SetTool (Schema::Tool::addBlock);
}

void CChildView::OnSetToolAddPoint ()
{
	if (SchemaManager::GetSchemaManager()->GetCurSchema ())
		SchemaManager::GetSchemaManager()->GetCurSchema ()->SetTool (Schema::Tool::addPoint);
}

void CChildView::OnCompile ()
{
	std::string compileInfo;
	std::string romPath;

	romPath = GetModuleFolder () + "rom.dat";
	
	try
	{
		SchemaManager::GetSchemaManager()->Validate ();
		compileInfo = SchemaManager::GetSchemaManager()->Compile (romPath);
		MessageBox (("Компиляция успешно завершена\n\n" + compileInfo).c_str(), "Редактор схем");
	}
	catch (CompilationException e)
	{
		MessageBox (e.GetErrorMessage ().c_str (), "Редактор схем", MB_ICONWARNING);
	}
	catch (ValidationException e)
	{
		MessageBox (e.GetErrorMessage ().c_str (), "Редактор схем", MB_ICONWARNING);
	}
	catch (...)
	{
		MessageBox ("Неизвестная ошибка", "Редактор схем", MB_ICONWARNING);
	}

}

void CChildView::OnNewProject ()
{
	CNewProject dialog;
	dialog.DoModal ();
}

void CChildView::OnOpenProject ()
{
	try
	{
		CHAR initialDir [1024];
		CFileDialog dialog(TRUE, _T("prj"), _T(""), OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT | OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST, _T("prj|*.prj|"), this);
		GetModuleFolder (initialDir, 1024);
		dialog.GetOFN().lpstrInitialDir = initialDir;
	
		if (dialog.DoModal () == IDOK)
			Project::GetProject ()->Load (std::string(dialog.GetPathName ()), std::string(GetModuleFolder () + "components\\"));
	}
	catch (ProjectException e)
	{
		MessageBox (e.GetErrorMessage ().c_str (), "Редактор схем", MB_ICONWARNING);
	}
}

void CChildView::OnSaveProject ()
{
	try
	{
		Project::GetProject ()->Save ();
	}
	catch (ProjectException e)
	{
		MessageBox (e.GetErrorMessage ().c_str (), "Редактор схем", MB_ICONWARNING);
	}
}

void CChildView::OnToolPanel ()
{
	toolPanelEnabled = !toolPanelEnabled;
	OnSize(0, 0, 0);
}


