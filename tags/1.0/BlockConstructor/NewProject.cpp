#include "stdafx.h"
#include "NewProject.h"

CNewProject::CNewProject(CWnd* pParent /*=NULL*/)
	: CDialog(CNewProject::IDD, pParent)
{
}

CNewProject::~CNewProject()
{
}

BEGIN_MESSAGE_MAP(CNewProject, CDialog)
	ON_BN_CLICKED (IDC_PROJECT_BROWSE_BUTTON, OnBrowseProject)
	ON_WM_CLOSE ()
END_MESSAGE_MAP()

void CNewProject::OnClose ()
{
	blocksPathCombo.Detach ();
	projectPathEdit.Detach ();
	
	CDialog::OnClose ();
}

void CNewProject::OnCancel ()
{
	OnClose ();
	EndDialog (IDCANCEL);
}

void CNewProject::OnOK ()
{
	CString projectPath;
	CString blocksPath;

	blocksPathCombo.GetWindowText (blocksPath);
	projectPathEdit.GetWindowText (projectPath);

	if (blocksPath.GetLength () <= 0)
	{
		MessageBox ("Выберите библиотеку компонентов", "Создание проекта", MB_ICONWARNING);
		return;
	}

	if (projectPath.GetLength () <= 0)
	{
		MessageBox ("Укажите имя файла проекта", "Создание проекта", MB_ICONWARNING);
		return;
	}
	
	try
	{
		Project::GetProject()->New (std::string(projectPath), std::string(GetModuleFolder () + "components\\" + blocksPath + ".cmp"), std::string(blocksPath));
	}
	catch (ProjectException e)
	{
		MessageBox (e.GetErrorMessage ().c_str (), "Создание проекта", MB_ICONWARNING);
		return;
	}
	
	OnClose ();
	EndDialog (IDOK);
}

int CNewProject::OnInitDialog ()
{
	int result;
	HANDLE hFind;
	WIN32_FIND_DATA findData;

	result = CDialog::OnInitDialog ();
	if (result)
	{
		blocksPathCombo.Attach (this->GetDlgItem(IDC_BLOCKS_PATH_COMBO)->m_hWnd);
		projectPathEdit.Attach (this->GetDlgItem(IDC_PROJECT_PATH_EDIT)->m_hWnd);

		hFind = FindFirstFile (GetModuleFolder () + "components\\*.cmp", &findData);
		if (hFind != INVALID_HANDLE_VALUE)
		{
			do
			{
				blocksPathCombo.AddString (GetFileTitle(findData.cFileName));
			}
			while (FindNextFile(hFind, &findData));

			FindClose (hFind);
		}
	}
	return result;
}

void CNewProject::OnBrowseProject ()
{	
	CHAR initialDir [1024];
	CFileDialog dialog(FALSE, _T("prj"), _T("DefaultProject"), OFN_OVERWRITEPROMPT | OFN_PATHMUSTEXIST, _T("prj|*.prj|"), this);
	GetModuleFolder (initialDir, 1024);
	dialog.GetOFN().lpstrInitialDir = initialDir;
	
	if (dialog.DoModal () == IDOK)
		projectPathEdit.SetWindowText (dialog.GetPathName ());
}
