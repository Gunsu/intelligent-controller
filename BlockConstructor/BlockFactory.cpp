#include "StdAfx.h"
#include "BlockFactory.h"

/*******************************
*          BlockFactory
********************************/

BlockFactory BlockFactory::blockFactory;

BlockFactory::BlockFactory ()
{
	curBlockIter = blocks.end ();
	filePath = "";
}

BlockFactory::~BlockFactory ()
{
	for (std::vector<Block*>::iterator i = blocks.begin (); i != blocks.end (); i++)
		delete (*i);
}

std::vector<Block*>::iterator BlockFactory::FindBlock (std::string name)
{
	for (std::vector<Block*>::iterator i = blocks.begin (); i != blocks.end (); i++)
		if ((*i)->GetName () == name)
			return i;
	return blocks.end ();
}

void BlockFactory::Load (std::string filePath)
{	
	try
	{
		VARIANT_BOOL success;
		CComPtr<xml::IXMLDOMDocument2> xmlDoc = NULL;
		CComPtr<xml::IXMLDOMNode> xmlRootNode = NULL;
		CComPtr<xml::IXMLDOMNodeList> xmlBlockNodeList = NULL;
		CComPtr<xml::IXMLDOMNodeList> xmlPointNodeList = NULL;
		CComPtr<xml::IXMLDOMNode> xmlBlockNode = NULL;
		CComPtr<xml::IXMLDOMNode> xmlPointNode = NULL;
		CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
		long blocksCount;
		long pointsCount;
		CComBSTR name;
		CComBSTR dataType;
		CComBSTR blockId;
		GraphicalPoint commandBlockSize (0.7, 0.35);
		const double minBlockDeltaSizeY = 0.25;
		double blockDeltaSizeY;
		const double blockSizeX = 0.6;
		double blockSizeY;
		int n;
		double halfPoint = ConnectionPoint::GetR () * 0.5;
		std::vector<BlockInputPoint*> blockInputPoints;
		std::vector<BlockOutputPoint*> blockOutputPoints;
		std::vector<int> ids;

		// очистка

		Clear ();

		// добавление блоков для построения входной и выходной команд
		
		blockInputPoints.push_back (new BlockInputPoint (NULL, std::string(""), GraphicalPoint (0 - halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -1));
		blockOutputPoints.push_back (new BlockOutputPoint (NULL, std::string(""), GraphicalPoint (commandBlockSize.x + halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -1));
		blockOutputPoints.push_back (new BlockOutputPoint (NULL, std::string(""), GraphicalPoint (commandBlockSize.x / 2, 0 - halfPoint), Orientation::orVert, 0));
		blocks.push_back (new InputCommandBlock (commandBlockSize, std::string("In"), &blockInputPoints, &blockOutputPoints));
		
		blockInputPoints.clear ();
		blockOutputPoints.clear ();

		blockInputPoints.push_back (new BlockInputPoint (NULL, std::string(""), GraphicalPoint (0 - halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -2));
		blockOutputPoints.push_back (new BlockOutputPoint (NULL, std::string(""), GraphicalPoint (commandBlockSize.x + halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -2));
		blockInputPoints.push_back (new BlockInputPoint (NULL, std::string(""), GraphicalPoint (commandBlockSize.x / 2, commandBlockSize.y + halfPoint), Orientation::orVert, 0));
		blocks.push_back (new OutputCommandBlock (commandBlockSize, std::string("Out"), &blockInputPoints, &blockOutputPoints));

		blockInputPoints.clear ();
		blockOutputPoints.clear ();

		blockInputPoints.push_back (new BlockInputPoint (NULL, std::string(""), GraphicalPoint (0 - halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -2));
		blockOutputPoints.push_back (new BlockOutputPoint (NULL, std::string(""), GraphicalPoint (commandBlockSize.x + halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -2));
		//blockInputPoints.push_back (new BlockInputPoint (NULL, std::string(""), GraphicalPoint (commandBlockSize.x / 2, commandBlockSize.y + halfPoint), Orientation::orVert, 0));
		blocks.push_back (new OutputCommandConstBlock (commandBlockSize, std::string("OutConst"), &blockInputPoints, &blockOutputPoints));

		blockInputPoints.clear ();
		blockOutputPoints.clear ();

		blockInputPoints.push_back (new BlockInputPoint (NULL, std::string(""), GraphicalPoint (0 - halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -2));
		blockOutputPoints.push_back (new BlockOutputPoint (NULL, std::string(""), GraphicalPoint (blockSizeX + halfPoint, commandBlockSize.y / 2), Orientation::orHoriz, -2));
		blockInputPoints.push_back (new BlockInputPoint (NULL, std::string("BuffID"), GraphicalPoint (blockSizeX / 2, commandBlockSize.y + halfPoint), Orientation::orVert, 1));
		blocks.push_back (new OutputCommandBufBlock (GraphicalPoint (blockSizeX, commandBlockSize.y), std::string("OutBuff"), &blockInputPoints, &blockOutputPoints));

		// загрузка блоков из xml

		xmlDoc.CoCreateInstance(__uuidof(xml::DOMDocument));
		if (!xmlDoc) 
			throw BlockFactoryException ("Ошибка при инициализации XML парсера");

		success = xmlDoc->load (CComVariant (filePath.c_str ()));
		if (success != VARIANT_TRUE) 
			throw BlockFactoryException ("Документ не существует или имеет неверный формат");

		xmlDoc->get_firstChild(&xmlRootNode);
		if (!xmlRootNode)
			throw BlockFactoryException ("Корневой узел не найден :)");

		xmlBlockNodeList = xmlRootNode->selectNodes(_bstr_t("./block"));
		if (!xmlBlockNodeList)
			throw BlockFactoryException ("Узел ./block не найден");

		xmlBlockNodeList->get_length(&blocksCount);

		for (long i = 0; i < blocksCount; i++)
		{
			blockInputPoints.clear ();
			blockOutputPoints.clear ();
				
			if (xmlBlockNode) 
				xmlBlockNode.Release ();
			xmlBlockNodeList->get_item(i, &xmlBlockNode);

			// inputs

			if (xmlPointNodeList)
				xmlPointNodeList.Release ();

			xmlPointNodeList = xmlBlockNode->selectNodes(_bstr_t("./input"));
			if (!xmlPointNodeList)
				throw BlockFactoryException ("Узел ./block/input не найден");

			xmlPointNodeList->get_length(&pointsCount);

			for (long j = 0; j < pointsCount; j++)
			{
				if (xmlPointNode) 
					xmlPointNode.Release ();

				xmlPointNodeList->get_item (j, &xmlPointNode);

				if (xmlTempNode) 
					xmlTempNode.Release ();

				xmlTempNode = xmlPointNode->selectSingleNode (_bstr_t("./name"));
				if (!xmlTempNode)
					throw BlockFactoryException ("Узел ./block/input/name не найден");

				xmlTempNode->get_text (&name);

				xmlTempNode = xmlPointNode->selectSingleNode (_bstr_t("./datatype"));
				if (!xmlTempNode)
					throw BlockFactoryException ("Узел ./block/input/datatype не найден");

				xmlTempNode->get_text (&dataType);

				int dataTypeNum = StringToInt((char*)_bstr_t(dataType));
				if (dataTypeNum <= 0)
					throw BlockFactoryException ("Узел ./block/input/datatype содержит некорректные данные");

				blockInputPoints.push_back (new BlockInputPoint (NULL, std::string((char*)_bstr_t(name)), GraphicalPoint (0, 0), Orientation::orHoriz, dataTypeNum));
			}

			// outputs

			if (xmlPointNodeList)
				xmlPointNodeList.Release ();

			xmlPointNodeList = xmlBlockNode->selectNodes(_bstr_t("./output"));
			if (!xmlPointNodeList)
				throw BlockFactoryException ("Узел ./block/output не найден");

			xmlPointNodeList->get_length(&pointsCount);

			for (long j = 0; j < pointsCount; j++)
			{
				if (xmlPointNode) 
					xmlPointNode.Release ();

				xmlPointNodeList->get_item (j, &xmlPointNode);

				if (xmlTempNode) 
					xmlTempNode.Release ();

				// name
				xmlTempNode = xmlPointNode->selectSingleNode (_bstr_t("./name"));
				if (!xmlTempNode)
					throw BlockFactoryException ("Узел ./block/output/name не найден");

				xmlTempNode->get_text (&name);

				// dataType
				xmlTempNode.Release ();
				xmlTempNode = xmlPointNode->selectSingleNode (_bstr_t("./datatype"));
				if (!xmlTempNode)
					throw BlockFactoryException ("Узел ./block/output/datatype не найден");

				xmlTempNode->get_text (&dataType);

				int dataTypeNum = StringToInt((char*)_bstr_t(dataType));
				if (dataTypeNum <= 0)
					throw BlockFactoryException ("Узел ./block/output/datatype содержит некорректные данные");

				blockOutputPoints.push_back (new BlockOutputPoint (NULL, std::string((char*)_bstr_t(name)), GraphicalPoint (0, 0), Orientation::orHoriz, dataTypeNum));
			}

			// вычисляем координаты входов и выходов

			blockSizeY = minBlockDeltaSizeY * Max (blockInputPoints.size () + 1, blockOutputPoints.size () + 1);

			blockDeltaSizeY = blockSizeY / (blockInputPoints.size () + 1);
			n = 1;
			for (std::vector<BlockInputPoint*>::iterator i = blockInputPoints.begin (); i != blockInputPoints.end (); i++)
			{
				(*i)->SetPos (GraphicalPoint (0 - halfPoint, n * blockDeltaSizeY));
				n++;
			}

			n = 1;
			blockDeltaSizeY = blockSizeY / (blockOutputPoints.size () + 1);
			for (std::vector<BlockOutputPoint*>::iterator i = blockOutputPoints.begin (); i != blockOutputPoints.end (); i++)
			{
				(*i)->SetPos (GraphicalPoint (blockSizeX + halfPoint, n * blockDeltaSizeY));
				n++;
			}

			// создаем блок

			// name
			if (xmlTempNode) 
				xmlTempNode.Release ();

			xmlTempNode = xmlBlockNode->selectSingleNode (_bstr_t("./name"));
			if (!xmlTempNode)
				throw BlockFactoryException ("Узел ./block/name не найден");

			xmlTempNode->get_text (&name);

			if (FindBlock (std::string((char*)_bstr_t(name))) != blocks.end ())
				throw BlockFactoryException ("Обнаружны блоки с одинаковым именем " + std::string((char*)_bstr_t(name)));

			if (FindBlock (std::string((char*)_bstr_t(name))) != blocks.end ())
				throw BlockFactoryException ("Обнаружны блоки с одинаковым именем " + std::string((char*)_bstr_t(name)));

			// id
			if (xmlTempNode) 
				xmlTempNode.Release ();

			xmlTempNode = xmlBlockNode->selectSingleNode (_bstr_t("./id"));
			if (!xmlTempNode)
				throw BlockFactoryException ("Узел ./block/id не найден");

			xmlTempNode->get_text (&blockId);
			int id = StringToInt ((char*)_bstr_t(blockId));
			if (id <= 1)
				throw BlockFactoryException ("Узел ./block/id должен содержать числовые значения > 1");

			ids.push_back(id);

			blocks.push_back (new Block (GraphicalPoint (blockSizeX, blockSizeY), std::string((char*)_bstr_t(name)), &blockInputPoints, &blockOutputPoints, id));
		}

		// проверка id блоков
		int j;
		for (int i = 0; i < ids.size (); i++)
		{
			for (j = 0; j < ids.size (); j++)
			{
				if (ids[j] == (i + 2))
					break;
			}
			if (j >= ids.size ())
			{
				throw BlockFactoryException ("Необходим блок с id " + IntToString(i + 2));
			}
		}

		curBlockIter = blocks.begin ();
		this->filePath = filePath;
	}
	catch (BlockFactoryException e)
	{
		Clear ();
		throw BlockFactoryException (std::string ("Ошибка при загрузке библиотеки компонентов ") + filePath + std::string (". ") + e.GetErrorMessage ());
	}
	catch (...)
	{
		Clear ();
		throw BlockFactoryException (std::string ("Ошибка при загрузке библиотеки компонентов ") + filePath + std::string (". Неизвестная ошибка."));
	}
}

bool BlockFactory::SetCurBlock (std::string name)
{
	std::vector<Block*>::iterator blockIter = FindBlock (name);
	if (blockIter == blocks.end ())
		return false;
	curBlockIter = blockIter;
	return true;
}

Block* BlockFactory::GetCurBlock ()
{
	return (curBlockIter == blocks.end ()) ? NULL : (*curBlockIter);
}

void BlockFactory::SetCurBlockToFirst ()
{
	curBlockIter = blocks.begin ();
}

void BlockFactory::SetCurBlockToNext ()
{
	curBlockIter++;
}

void BlockFactory::Clear ()
{
	for (std::vector<Block*>::iterator i = blocks.begin (); i != blocks.end (); i++)
		delete (*i);
	blocks.clear ();
	curBlockIter = blocks.end ();
	filePath = "";
}

std::string BlockFactory::GetFilePath ()
{
	return filePath;
}