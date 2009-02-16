#include "StdAfx.h"
#include "Project.h"

/*******************************
*          Project
********************************/

Project Project::project;

void Project::SetProjectClosedCallback (ProjectClosedCallback* projectClosedCallback)
{
	this->projectClosedCallback = projectClosedCallback;
}

void Project::SetProjectOpenedCallback (ProjectOpenedCallback* projectOpenedCallback)
{
	this->projectOpenedCallback = projectOpenedCallback;
}

void Project::Close (bool callCallback)
{
	isLoad = false;
	projectFilePath.clear ();
	blockFactoryFile.clear ();
	SchemaManager::GetSchemaManager ()->Clear ();
	BlockFactory::GetBlockFactory ()->Clear ();
	if (callCallback && projectClosedCallback)
		projectClosedCallback ();
}

void Project::New (std::string projectFilePath, std::string blockFactoryFilePath, std::string blockFactoryFile)
{
	try
	{
		CComPtr<xml::IXMLDOMDocument2> xmlDoc = NULL;
		CComPtr<xml::IXMLDOMNode> xmlRootNode = NULL;
		CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
		VARIANT_BOOL success;
		HRESULT result;

		Close (true);
		
		BlockFactory::GetBlockFactory ()->Load (blockFactoryFilePath);

		xmlDoc.CoCreateInstance(__uuidof(xml::DOMDocument));
		if (!xmlDoc) 
			throw ProjectException ("������ ��� ������������� XML �������");

		success = xmlDoc->loadXML(L"<Project></Project>");
		if (success != VARIANT_TRUE) 
			throw ProjectException ("������ ��� �������� ��������� ���� ���������");

		xmlRootNode = xmlDoc->firstChild;

		xmlTempNode = xmlDoc->createElement("Components");
		if (xmlTempNode.p == NULL)
			throw ProjectException ("������ ��� �������� ���� Project/Components");
		xmlTempNode->text = blockFactoryFile.c_str ();
		xmlRootNode->appendChild (xmlTempNode);

		xmlTempNode = xmlDoc->createElement("Schemas");
		if (xmlTempNode.p == NULL)
			throw ProjectException ("������ ��� �������� ���� Project/Schemas");
		xmlRootNode->appendChild (xmlTempNode);

		result = xmlDoc->save (CComVariant (projectFilePath.c_str ()));
		if (result != 0) 
			throw ProjectException ("������ ��� ���������� ��������� �� ����");

		this->projectFilePath = projectFilePath;
		this->blockFactoryFile = blockFactoryFile;
		isLoad = true;
		if (projectOpenedCallback)
			projectOpenedCallback (projectFilePath);
	}
	catch (Exception e)
	{
		Close (true);
		throw ProjectException ("������ ��� �������� ������� " + projectFilePath + ". " + e.GetErrorMessage ());
	}
}

void Project::Load (std::string projectFilePath, std::string componentsFolderPath)
{
	try
	{
		CComPtr<xml::IXMLDOMDocument2> xmlDoc = NULL;
		CComPtr<xml::IXMLDOMNode> xmlRootNode = NULL;
		CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
		VARIANT_BOOL success; 
		
		Close (false);

		xmlDoc.CoCreateInstance(__uuidof(xml::DOMDocument));
		if (!xmlDoc) 
			throw ProjectException ("������ ��� ������������� XML �������");

		success = xmlDoc->load (CComVariant (projectFilePath.c_str ()));
		if (success != VARIANT_TRUE)
			throw ProjectException ("�������� �� ���������� ��� ����� �������� ������");

		xmlRootNode = xmlDoc->GetfirstChild ();
		xmlTempNode = xmlRootNode->selectSingleNode ("./Components");
		if (xmlTempNode == NULL)
			throw ProjectException ("���� �� ������ Project/Components");
		blockFactoryFile = (char*)xmlTempNode->text;
		
		BlockFactory::GetBlockFactory ()->Load (componentsFolderPath + blockFactoryFile + ".cmp");

		xmlTempNode = xmlRootNode->selectSingleNode ("./Schemas");
		if (xmlTempNode == NULL)
			throw ProjectException ("���� �� ������ Project/Schemas");

		SchemaManager::GetSchemaManager ()->Load (xmlTempNode);

		this->projectFilePath = projectFilePath;
		isLoad = true;
		if (projectOpenedCallback)
			projectOpenedCallback (projectFilePath);
	}
	catch (Exception e)
	{
		Close (true);
		throw ProjectException ("������ ��� �������� ������� " + projectFilePath + ". " + e.GetErrorMessage ());
	}
}

void Project::SaveAs (std::string projectFilePath)
{
	try
	{
		CComPtr<xml::IXMLDOMDocument2> xmlDoc = NULL;
		CComPtr<xml::IXMLDOMNode> xmlRootNode = NULL;
		CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
		VARIANT_BOOL success;
		HRESULT result;

		xmlDoc.CoCreateInstance(__uuidof(xml::DOMDocument));
		if (!xmlDoc) 
			throw ProjectException ("������ ��� ������������� XML �������");

		success = xmlDoc->loadXML(L"<Project></Project>");
		if (success != VARIANT_TRUE) 
			throw ProjectException ("������ ��� �������� ��������� ���� ���������");

		xmlRootNode = xmlDoc->firstChild;

		xmlTempNode = xmlDoc->createElement("Components");
		if (xmlTempNode.p == NULL)
			throw ProjectException ("������ ��� �������� ���� Project/Components");
		xmlTempNode->text = blockFactoryFile.c_str ();
		xmlRootNode->appendChild (xmlTempNode);

		xmlTempNode = xmlDoc->createElement("Schemas");
		if (xmlTempNode.p == NULL)
			throw ProjectException ("������ ��� �������� ���� Project/Schemas");
		xmlRootNode->appendChild (xmlTempNode);

		SchemaManager::GetSchemaManager ()->Save (xmlTempNode);

		result = xmlDoc->save (CComVariant (projectFilePath.c_str ()));
		if (result != 0) 
			throw ProjectException ("������ ��� ���������� ��������� �� ����");
		
		this->projectFilePath = projectFilePath;
	}
	catch (Exception e)
	{
		throw ProjectException ("������ ��� ���������� ������� " + projectFilePath + ". " + e.GetErrorMessage ());
	}
}

void Project::Save ()
{
	if (!IsLoad())
		return;

	if (projectFilePath.length () <= 0)
		throw ProjectException ("������ ��� ���������� �������. ���� � ����� ������� �� �����");
	SaveAs (projectFilePath);
}