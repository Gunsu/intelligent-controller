#include "stdafx.h"
#include "SchemaProperties.h"
#include "MainFrm.h"

CSchemaProperties::CSchemaProperties(CWnd* pParent /*=NULL*/)
	: CDialog(CSchemaProperties::IDD, pParent)
{
	createSchema = false;
}

CSchemaProperties::~CSchemaProperties()
{
}


BEGIN_MESSAGE_MAP(CSchemaProperties, CDialog)
	ON_WM_CLOSE ()
END_MESSAGE_MAP()


INT_PTR CSchemaProperties::DoModal (bool createSchema)
{
	this->createSchema = createSchema;
	return CDialog::DoModal ();
}

void CSchemaProperties::OnClose ()
{
	nameEdit.Detach ();
	CDialog::OnClose ();
}

void CSchemaProperties::OnCancel ()
{
	OnClose ();
	EndDialog (IDCANCEL);
}

void CSchemaProperties::OnOK ()
{
	const int schemaNameMaxLen = 1024;
	TCHAR schemaName[schemaNameMaxLen];
	Schema* schema;

	nameEdit.GetLine (0, schemaName, schemaNameMaxLen);

	if (_tcslen(schemaName) == 0)
	{
		MessageBox ("¬ведите им€ схемы", "–едактор схем", MB_ICONWARNING);
		return;
	}
	
	if (createSchema)
	{
		if (SchemaManager::GetSchemaManager ()->GetSchema (schemaName))
		{
			MessageBox ("—хема с таким именем уже существует", "–едактор схем", MB_ICONWARNING);
			return;
		}
		else
		{
			schema = new Schema (GraphicalCanvas::GetCanvas(), schemaName);
			schema->SetObjectSelectedCallback (&objectSelectedCallback);
			schema->SetScrollCallback (&scrollCallback);
			SchemaManager::GetSchemaManager ()->AddSchema (schema);
		}
	}
	else
	{
		if (!SchemaManager::GetSchemaManager ()->RenameCurSchema (schemaName))
		{
			MessageBox ("—хема с таким именем уже существует", "–едактор схем", MB_ICONWARNING);
			return;
		}
	}
	
	OnClose ();
	EndDialog (IDOK);
}

int CSchemaProperties::OnInitDialog ()
{
	int result;

	result = CDialog::OnInitDialog ();
	if (result)
	{
		nameEdit.Attach (this->GetDlgItem(IDC_SCHEMA_PROPERTIES_DIALOG_NAME_EDIT)->m_hWnd);

		if (createSchema)
			this->SetWindowText (_T("—оздание схемы"));
		else
		{
			nameEdit.SetWindowText (SchemaManager::GetSchemaManager ()->GetCurSchema ()->GetName ().c_str ());
			this->SetWindowText (_T("—войства схемы"));
		}
	}
	return result;
}

