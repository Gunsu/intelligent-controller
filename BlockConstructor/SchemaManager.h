#ifndef SCHEMAS_MANAGER_H
#define SCHEMAS_MANAGER_H

#include "Schema.h"
#include "Common.h"
#include "Exceptions.h"
#include "CompileHelper.h"
#include <list>
#include <string>

/*******************************
*         SchemaManager
********************************/

class SchemaManager
{
	protected:
		static SchemaManager schemaManager;

	public:
		static SchemaManager* GetSchemaManager() {return &schemaManager;}

	protected:
		std::list<Schema*> schemas;
		std::list<Schema*>::iterator curSchemaIter;
		ObjectSelectedCallback* objectSelectedCallback;
		ScrollCallback* scrollCallback;

		std::list<Schema*>::iterator FindSchema (std::string name);

	public:
		SchemaManager ();
		~SchemaManager ();
		bool AddSchema (Schema* schema);
		bool SetCurSchema (std::string name);
		Schema* GetCurSchema ();
		Schema* GetSchema (std::string name);
		bool DelCurSchema ();
		int GetSchemasCount ();
		bool RenameCurSchema (std::string name);
		void SetCurSchemaToFirst ();
		void SetCurSchemaToNext ();
		void Clear ();

	public:
		void Validate ();
		std::string Compile (std::string romFilePath);
		void Save (CComPtr<xml::IXMLDOMNode> xmlSchemasNode);
		void Load (CComPtr<xml::IXMLDOMNode> xmlSchemasNode);

		void SetObjectSelectedCallback (ObjectSelectedCallback* callback);
		void SetScrollCallback (ScrollCallback* callback);
};

#endif