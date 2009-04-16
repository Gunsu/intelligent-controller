#ifndef TOOL_PANEL_H
#define TOOL_PANEL_H

#include "DrawPanel.h"
#include "BlockFactory.h"
#include "Project.h"

class CDrawPanel;

class CToolPanel: public CDialog
{
	protected:
		BOOL OnInitDialog();
		void OnSelectBlockType ();
		void OnCreateSchema ();
		void OnSelectSchema ();
		void OnDeleteSchema ();
		void OnSchemaProperties ();
		void OnSize (UINT type, int cx, int cy);

		DECLARE_MESSAGE_MAP () 

	public:
		CListBox blockTypeBox;
		CListBox schemasBox;
		void ReloadBlockTypeBox ();
		void ReloadSchemasBox ();
		void Enable (bool enable);
};

#endif