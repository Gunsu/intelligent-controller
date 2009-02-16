#ifndef NEW_PROJECT_H
#define NEW_PROJECT_H

#include "Resource.h"
#include "Project.h"

class CNewProject: public CDialog
{
	public:	
		CNewProject (CWnd* pParent = NULL);
		virtual ~CNewProject ();

		enum { IDD = IDD_NEW_PROJECT_DIALOG };

	protected:
		CComboBox blocksPathCombo;
		CEdit projectPathEdit;

		virtual int OnInitDialog ();
		virtual void OnClose ();
		virtual void OnOK ();
		virtual void OnCancel ();

		void OnBrowseProject ();
		DECLARE_MESSAGE_MAP ()

	public:	
};

#endif