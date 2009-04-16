#include "StdAfx.h"
#include "SchemaManager.h"

/*******************************
*         SchemaManager
********************************/

SchemaManager SchemaManager::schemaManager;

SchemaManager::SchemaManager ()
{
	objectSelectedCallback = NULL;
	scrollCallback = NULL;
	curSchemaIter = schemas.end();
}

SchemaManager::~SchemaManager ()
{
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
		delete (*i);
}

std::list<Schema*>::iterator SchemaManager::FindSchema (std::string name)
{
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
		if ((*i)->GetName () == name)
			return i;
	return schemas.end();
}

Schema* SchemaManager::GetSchema (std::string name)
{
	std::list<Schema*>::iterator i = FindSchema (name);

	if (i == schemas.end ())
		return NULL;
	return (*i);
}

int SchemaManager::GetSchemasCount ()
{
	return schemas.size ();
}

bool SchemaManager::AddSchema (Schema* schema)
{
	if (FindSchema (schema->GetName ()) != schemas.end ())
		return false;
	schemas.push_front(schema);
	curSchemaIter = schemas.begin ();
	return true;
}

bool SchemaManager::SetCurSchema (std::string name)
{
	std::list<Schema*>::iterator schemaIter = FindSchema (name);
	if (schemaIter == schemas.end ())
		return false;
	curSchemaIter = schemaIter;
	return true;
}

Schema* SchemaManager::GetCurSchema ()
{
	return (curSchemaIter == schemas.end()) ? NULL : (*curSchemaIter);
}

bool SchemaManager::RenameCurSchema (std::string name)
{
	if (curSchemaIter == schemas.end())
		return false;

	if ((*curSchemaIter)->GetName () == name)
		return true;
	
	if (FindSchema (name) != schemas.end ())
		return false;

	(*curSchemaIter)->SetName (name);
	return true;
}

bool SchemaManager::DelCurSchema ()
{
	if (curSchemaIter == schemas.end())
		return false;
	delete (*curSchemaIter);
	schemas.erase (curSchemaIter);
	curSchemaIter = schemas.begin();
	return true;
}

void SchemaManager::SetCurSchemaToFirst ()
{
	curSchemaIter = schemas.begin();
}

void SchemaManager::SetCurSchemaToNext ()
{
	curSchemaIter++;
}

void SchemaManager::Validate ()
{
	const std::string separator ("--------------------------------------------------------------\n");
	std::vector<std::string> masks;
	std::vector<std::string> schemaNames;
	std::string mask1;
	std::string mask2;
	int k;
	std::string errorReport;
	bool conflicts = false;
	
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
	{
		try
		{
			(*i)->Validate ();
		}
		catch (ValidationException e)
		{
			errorReport +=  e.GetErrorMessage ();
		}
		masks.push_back ((*i)->GetCommandMask ());
		schemaNames.push_back ((*i)->GetName ());
	}

	// �������� ��������� ����� ������� ������
	for (int i = 0; i < masks.size () - 1; i++)
	{
		for (int j = i + 1; j < masks.size (); j++)
		{
			mask1 = masks[i];
			mask2 = masks[j];
			if ((mask1.length () > 0) && (mask2.length () > 0) && (mask1.length () == mask2.length ()))
			{
				for (k = 0; k < mask1.length (); k++)
				{
					if ((mask1[k] != '_') && (mask2[k] != '_') && (mask1[k] != mask2[k]))
						break;
				}
				if (k >= mask1.length ())
				{
					if (!conflicts)
					{
						errorReport += separator  + "���������� ��������� ������\n" + separator;
						conflicts = true;
					}
					
					errorReport += "> ������� ������� ����� " + schemaNames[i] + " ����������� � ������� �������� ����� " + schemaNames[j] + "\n";
				}
			}
		}
	}

	if (errorReport.size () > 0)
		throw ValidationException (errorReport);
}

std::string SchemaManager::Compile (std::string romFilePath)
{
	RomData romData (1024 * 16);
	MemoryPool memoryPool (64);
	int pos = 1;				// ������� ������� � ��� �� ������� ��������� ������ 
	int posInMasksArray = 1;	// ������� ������� � ��� �� ������� �����
	std::string compileInfo;

	// ������ ���� ��� �������� ���������� ����� ������
	romData [0] = (unsigned char) schemas.size ();

	// �������� ������� � ��� ����� �������� ����� ������
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
	{
		// 1 ���� ����� ����� + ���� ����� + 2 ����� ������ ����� ��������� �������
		pos += (*i)->GetCommandMask ().length () + 3; 
	}

	// ����������� ������ ����� ��������� �������
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
	{
		std::string mask = (*i)->GetCommandMask ();
		
		// ����� � ��� 1 ���� ����� ������� �����
		romData [posInMasksArray] = (unsigned char) mask.length ();
		posInMasksArray++;

		// ����� � ��� ���� ������� �����
		for (int j = 0; j < mask.length (); j++)
			romData [posInMasksArray + j] = ((char*) mask.c_str ())[j];
		posInMasksArray += mask.length ();

		// ����� � ��� 2 ����� ������ ����� ��������� �������
		romData [posInMasksArray] = (unsigned char) (pos >> 8);
		posInMasksArray++;
		romData [posInMasksArray] = (unsigned char) pos;
		posInMasksArray++;
		
		// ����������� ����� ��������� �������
		(*i)->Compile (romData, &memoryPool, &pos);
	}

	romData.SaveBin(romFilePath);

	compileInfo =	"� ��� �������� " + IntToString (romData.GetUsedSize ()) + " ����\n";
	compileInfo +=	"������ ��� " + IntToString (romData.GetSize ()) + " ����\n";
	compileInfo +=	"������� �������� �� ��� ������ " + IntToString (memoryPool.GetMaxUsedSize ()) + " ����\n";
	compileInfo +=	"������ ���� ������ " + IntToString (memoryPool.GetSize ()) + " ����";
	return compileInfo;
}

void SchemaManager::Clear ()
{
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
		delete (*i);
	schemas.clear ();
	curSchemaIter = schemas.end ();
}

void SchemaManager::SetObjectSelectedCallback (ObjectSelectedCallback* callback)
{
	objectSelectedCallback = callback;
}

void SchemaManager::SetScrollCallback (ScrollCallback* callback)
{
	scrollCallback = callback;
}

void SchemaManager::Save (CComPtr<xml::IXMLDOMNode> xmlSchemasNode)
{
	CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
	
	for (std::list<Schema*>::iterator i = schemas.begin(); i != schemas.end(); i++)
	{
		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemasNode->GetownerDocument()->createElement("Schema");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema");
		xmlSchemasNode->appendChild (xmlTempNode);
		(*i)->Save (xmlTempNode);
	}
}

void SchemaManager::Load (CComPtr<xml::IXMLDOMNode> xmlSchemasNode)
{
	CComPtr<xml::IXMLDOMNodeList> xmlSchemasNodeList = NULL;
	CComPtr<xml::IXMLDOMNode> xmlSchemaNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlNameNode = NULL;
	Schema* schema;
	
	xmlSchemasNodeList = xmlSchemasNode->selectNodes("./Schema");
	if (xmlSchemasNodeList.p == NULL)
		throw Exception ("���� �� ������ Project/Schemas/Schema");

	for (long i = 0; i < xmlSchemasNodeList->Getlength (); i++)
	{
		xmlSchemaNode = xmlSchemasNodeList->Getitem (i);
		xmlNameNode = xmlSchemaNode->selectSingleNode ("./Name");
		if (xmlNameNode.p == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Name");

		schema = new Schema (GraphicalCanvas::GetCanvas (), std::string(xmlNameNode->text));
		schema->SetObjectSelectedCallback (this->objectSelectedCallback);
		schema->SetScrollCallback (this->scrollCallback);
		schema->Load (xmlSchemaNode);
		AddSchema (schema);
	}
}