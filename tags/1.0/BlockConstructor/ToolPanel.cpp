#include "stdafx.h"
#include "ToolPanel.h"
#include "Resource.h"
#include "SchemaManager.h"
#include "SchemaProperties.h"

BEGIN_MESSAGE_MAP(CToolPanel, CDialog)
	ON_LBN_SELCHANGE (IDC_BLOCK_TYPE_LIST, OnSelectBlockType)
	ON_LBN_SELCHANGE (IDC_SCHEMA_LIST, OnSelectSchema)
	ON_BN_CLICKED (IDC_CREATE_SCHEMA_BUTTON, OnCreateSchema)
	ON_BN_CLICKED (IDC_DELETE_SCHEMA_BUTTON, OnDeleteSchema)
	ON_BN_CLICKED (IDC_SCHEMA_PROPERTIES_BUTTON, OnSchemaProperties)
	ON_WM_SIZE()
END_MESSAGE_MAP()

BOOL CToolPanel::OnInitDialog ()
{	
	blockTypeBox.Attach (this->GetDlgItem(IDC_BLOCK_TYPE_LIST)->m_hWnd);
	schemasBox.Attach (this->GetDlgItem(IDC_SCHEMA_LIST)->m_hWnd);
	return TRUE;
}

void CToolPanel::OnSize (UINT type, int cx, int cy)
{
	const int border = 5;

	CRect clientRect;
	CRect buttonRect;
	CRect labelRect;

	GetClientRect (&clientRect);
	int halfHeight = clientRect.Height () / 2;
	int deltaButton;

	if (this->GetDlgItem(IDC_BLOCKS_STATIC))
	{
		GetDlgItem (IDC_BLOCKS_STATIC)->GetClientRect (&labelRect);
		GetDlgItem (IDC_BLOCK_PROPERTIES_BUTTON)->GetClientRect (&buttonRect);
		deltaButton = (clientRect.Width () - border - 3 * buttonRect.Width ()) / 3;

		GetDlgItem (IDC_BLOCKS_STATIC)->SetWindowPos (0, border, border, labelRect.Width (), labelRect.Height (), SWP_NOZORDER);
		blockTypeBox.SetWindowPos (0, border, border + labelRect.Height () + border, clientRect.Width () - 2 * border, halfHeight - labelRect.Height () /*- buttonRect.Height ()*/ - 4 * border, SWP_NOZORDER);
		GetDlgItem (IDC_BLOCK_PROPERTIES_BUTTON)->SetWindowPos (0, clientRect.Width () - border - buttonRect.Width (), halfHeight - buttonRect.Height () - border, buttonRect.Width (), buttonRect.Height (), SWP_NOZORDER);

		GetDlgItem (IDC_SCHEMAS_STATIC)->SetWindowPos (0, border, halfHeight + border, clientRect.Width () - 2 * border, labelRect.Height (), SWP_NOZORDER);
		schemasBox.SetWindowPos (0, border, halfHeight + labelRect.Height () + 2 * border, clientRect.Width () - 2 * border, halfHeight - labelRect.Height () - buttonRect.Height () - 4 * border, SWP_NOZORDER);	
		
		GetDlgItem (IDC_CREATE_SCHEMA_BUTTON)->SetWindowPos (0, border, clientRect.Height () - buttonRect.Height () - border, buttonRect.Width (), buttonRect.Height (), SWP_NOZORDER);
		GetDlgItem (IDC_DELETE_SCHEMA_BUTTON)->SetWindowPos (0, border + buttonRect.Width () + deltaButton, clientRect.Height () - buttonRect.Height () - border, buttonRect.Width (), buttonRect.Height (), SWP_NOZORDER);
		GetDlgItem (IDC_SCHEMA_PROPERTIES_BUTTON)->SetWindowPos (0, border + 2 * buttonRect.Width () + 2 * deltaButton, clientRect.Height () - buttonRect.Height () - border, buttonRect.Width (), buttonRect.Height (), SWP_NOZORDER);
	}

	CWnd::OnSize (type, cx, cy);
}

void CToolPanel::OnSelectBlockType ()
{
	CString blockType;
	blockTypeBox.GetText(blockTypeBox.GetCurSel (), blockType);
	BlockFactory::GetBlockFactory ()->SetCurBlock ((LPCTSTR)blockType);
}

void CToolPanel::OnCreateSchema ()
{
	CSchemaProperties schemaProperties (this);
	if (schemaProperties.DoModal (true) == IDOK)
	{
		schemasBox.AddString (SchemaManager::GetSchemaManager ()->GetCurSchema ()->GetName ().c_str ());
		schemasBox.SelectString (-1, SchemaManager::GetSchemaManager ()->GetCurSchema ()->GetName ().c_str ());
		SchemaManager::GetSchemaManager ()->GetCurSchema ()->Draw ();
	}
}

void CToolPanel::OnSelectSchema ()
{
	CString schemaName;
	schemasBox.GetText(schemasBox.GetCurSel (), schemaName);
	SchemaManager::GetSchemaManager ()->SetCurSchema ((LPCTSTR)schemaName);
	SchemaManager::GetSchemaManager ()->GetCurSchema ()->Draw ();
}

void CToolPanel::OnDeleteSchema ()
{
	if (SchemaManager::GetSchemaManager ()->GetSchemasCount () <= 0)
		return;

	SchemaManager::GetSchemaManager ()->DelCurSchema ();
	schemasBox.DeleteString (schemasBox.GetCurSel ());
	if (SchemaManager::GetSchemaManager ()->GetSchemasCount () > 0)
	{
		schemasBox.SelectString (-1, SchemaManager::GetSchemaManager ()->GetCurSchema ()->GetName ().c_str ());
		SchemaManager::GetSchemaManager ()->GetCurSchema ()->Draw ();
	}
	else
	{
		GraphicalCanvas::GetCanvas()->Flush ();
	}
}

void CToolPanel::OnSchemaProperties ()
{
	if (SchemaManager::GetSchemaManager ()->GetCurSchema () == NULL)
		return;
	
	int curSel;
	CSchemaProperties schemaProperties (this);
	if (schemaProperties.DoModal (false) == IDOK)
	{
		curSel = schemasBox.GetCurSel ();
		schemasBox.DeleteString (curSel);
		schemasBox.InsertString (curSel, SchemaManager::GetSchemaManager ()->GetCurSchema ()->GetName ().c_str ());
		schemasBox.SelectString (-1, SchemaManager::GetSchemaManager ()->GetCurSchema ()->GetName ().c_str ());
	}
}

void CToolPanel::ReloadBlockTypeBox ()
{
	try
	{
		blockTypeBox.ResetContent ();
		BlockFactory::GetBlockFactory()->SetCurBlockToFirst ();
		while (BlockFactory::GetBlockFactory()->GetCurBlock ())
		{
			blockTypeBox.AddString (BlockFactory::GetBlockFactory()->GetCurBlock ()->GetName ().c_str ());
			BlockFactory::GetBlockFactory()->SetCurBlockToNext ();
		}
		BlockFactory::GetBlockFactory()->SetCurBlockToFirst ();
		blockTypeBox.SetCurSel (0);
	}
	catch (BlockFactoryException e)
	{
		MessageBox (e.GetErrorMessage ().c_str (), "Редактор схем");
	}
}

void CToolPanel::ReloadSchemasBox ()
{
	schemasBox.ResetContent ();
	SchemaManager::GetSchemaManager()->SetCurSchemaToFirst ();
	while (SchemaManager::GetSchemaManager()->GetCurSchema ())
	{
		schemasBox.AddString (SchemaManager::GetSchemaManager()->GetCurSchema ()->GetName ().c_str ());
		SchemaManager::GetSchemaManager()->SetCurSchemaToNext ();
	}
	SchemaManager::GetSchemaManager()->SetCurSchemaToFirst ();
	schemasBox.SetCurSel (0);
}

void CToolPanel::Enable (bool enable)
{
	BOOL e = enable ? TRUE : FALSE;
	
	this->GetDlgItem(IDC_CREATE_SCHEMA_BUTTON)->EnableWindow (e);
	this->GetDlgItem(IDC_DELETE_SCHEMA_BUTTON)->EnableWindow (e);
	this->GetDlgItem(IDC_SCHEMA_PROPERTIES_BUTTON)->EnableWindow (e);
	this->GetDlgItem(IDC_SCHEMA_LIST)->EnableWindow (e);
	this->GetDlgItem(IDC_BLOCK_PROPERTIES_BUTTON)->EnableWindow (e);
	this->GetDlgItem(IDC_BLOCK_TYPE_LIST)->EnableWindow (e);
}