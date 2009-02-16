#ifndef SCHEMA_PROPERTIES_H
#define SCHEMA_PROPERTIES_H

#include "Resource.h"
#include "SchemaManager.h"
#include <string>

class CSchemaProperties: public CDialog
{
	public:	
		CSchemaProperties (CWnd* pParent = NULL);
		virtual ~CSchemaProperties ();
		virtual INT_PTR DoModal (bool createSchema);

		enum { IDD = IDD_SCHEMA_PROPERTIES_DIALOG };

	protected:
		CEdit nameEdit;
		bool createSchema;

		virtual int OnInitDialog ();
		virtual void OnClose ();
		virtual void OnOK ();
		virtual void OnCancel ();
		DECLARE_MESSAGE_MAP ()

	public:	
};

#endif