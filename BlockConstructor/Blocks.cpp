#include "StdAfx.h"
#include "Blocks.h"

/*******************************
*       GraphicalObject
********************************/

GraphicalObject::GraphicalObject (GraphicalObject* graphicalObject)
{
	pos = graphicalObject->pos;
	selected = graphicalObject->selected;
}

void GraphicalObject::SetSelected (bool selected)
{
	this->selected = selected;
}

bool GraphicalObject::GetSelected ()
{
	return selected;
}

void GraphicalObject::SetPos (GraphicalPoint& pos)
{
	this->pos = pos;
}

GraphicalPoint GraphicalObject::GetPos ()
{
	return pos;
}

/*******************************
*        ConnectionPoint
********************************/

const double ConnectionPoint::R = 0.05;

ConnectionPoint::ConnectionPoint (int dataType, Orientation orientation, std::string name) 
{
	outputIterator = outputs.end ();
	this->dataType = dataType;
	this->orientation = orientation;
	signaledState = false;
	this->name = name;
	compileData.processed = false;
}

ConnectionPoint::ConnectionPoint (ConnectionPoint* connectionPoint) : GraphicalObject (connectionPoint)
{
	outputIterator = outputs.end ();
	this->dataType = connectionPoint->dataType;
	this->orientation = connectionPoint->orientation;
	this->signaledState = connectionPoint->signaledState;
	this->name = connectionPoint->name;
}

void ConnectionPoint::Connect (ConnectionPoint* connectionPoint)
{
	outputs.push_back (connectionPoint);
}

void ConnectionPoint::Disconnect (ConnectionPoint* connectionPoint)
{
	for (std::list<ConnectionPoint*>::iterator i = outputs.begin (); i != outputs.end (); i++)
		if ((*i) == connectionPoint)
		{
			outputs.erase (i);
			return;
		}
}

void ConnectionPoint::DisconnectAll ()
{
	outputs.clear ();
}

ConnectionPoint* ConnectionPoint::GetFirstOutputPoint ()
{
	outputIterator = outputs.begin ();
	if (outputIterator != outputs.end ())
		return (*outputIterator);
	return 0;
}

ConnectionPoint* ConnectionPoint::GetNextOutputPoint ()
{
	outputIterator++;
	if (outputIterator != outputs.end ())
		return (*outputIterator);
	return 0;
}

void ConnectionPoint::SetCurrentOutputPoint (ConnectionPoint* connectionPoint)
{
	if (outputIterator != outputs.end ())
		(*outputIterator) = connectionPoint;
}

int ConnectionPoint::GetOutputPointsCount ()
{
	return outputs.size ();
}

int ConnectionPoint::GetDataType ()
{
	return dataType;
}

void ConnectionPoint::SetDataType (int dataType)
{
	this->dataType = dataType;
}

bool ConnectionPoint::CanMove ()
{
	return true;
}

ObjectType ConnectionPoint::GetType ()
{
	return ObjectType::otConnectionPoint;
}

bool ConnectionPoint::IsOfType (ObjectType type)
{
	return (ObjectType::otConnectionPoint == type);
}

bool ConnectionPoint::TestIntersect (GraphicalPoint& pos)
{
	return (GetPos () - pos).Mod () <= R;
}

void ConnectionPoint::OnOutputSetSelected (ConnectionPoint* output, bool selected)
{
	outputs.remove (output);
	outputs.push_front (output);
}

void ConnectionPoint::Draw (GraphicalCanvas* canvas)
{
	static const double linePointR = R * 0.5;
	static const double lineFat = 0.01;

	static const double breaketFat = 0.01;
	static const double breaketLength = 0.035;
	static const double breaketDistance = 0.07;
	static GraphicalColor breaketColor (1.0, 0.0, 0.0);

	GraphicalPoint point1;
	GraphicalPoint point2;
	double x;
	double y;
	
	const GraphicalColor nosignaledLineColor (0.0, 0.0, 0.0);
	const GraphicalColor nosignaledLinePointColor (0.0, 0.0, 0.0);
	const GraphicalColor signaledLineColor1 (0.0, 0.9, 0.0);
	const GraphicalColor signaledLineColor2 (0.0, 0.0, 1.0);
	GraphicalColor pointColor (0.6, 0.6, 0.6);

	GraphicalColor lineColor (0.0, 0.0, 0.0);
	GraphicalColor linePointColor (0.0, 0.0, 0.0);


	if (orientation == Orientation::orHoriz)
	{										
		for (std::list<ConnectionPoint*>::iterator i = outputs.begin (); i != outputs.end (); i++)
		{		
			if (signaledState)
			{
				lineColor = signaledLineColor1;
				linePointColor = signaledLineColor1;
			}
			else if ((*i)->signaledState)
			{
				lineColor = signaledLineColor2;
				linePointColor = signaledLineColor2;
			}
			else
			{
				lineColor = nosignaledLineColor;
				linePointColor = nosignaledLinePointColor;
			}
			
			if ((*i)->orientation == Orientation::orHoriz) // horiz -> horiz
			{
				x = Min (GetPos ().x, (*i)->GetPos ().x) + Abs (GetPos ().x - (*i)->GetPos ().x) * 0.5; 
		
				point1.x = x;
				point1.y = GetPos ().y;
				point2.x = x;
				point2.y = (*i)->GetPos ().y;
				
				canvas->Line (GetPos (), point1, lineFat, lineColor);
				canvas->Line (point1, point2, lineFat, lineColor);
				canvas->Line (point2, (*i)->GetPos (), lineFat, lineColor);

				if (Abs (point1.y - point2.y) > 0.01)
				{
					canvas->Circle (point1, linePointR, linePointColor);
					canvas->Circle (point2, linePointR, linePointColor);
				}
			}
			else if ((*i)->orientation == Orientation::orVert) // horiz -> vert
			{
				point1.y = GetPos ().y;
				point1.x = (*i)->GetPos ().x;
			
				canvas->Line (GetPos (), point1, lineFat, lineColor);
				canvas->Line (point1, (*i)->GetPos (), lineFat, lineColor);

				canvas->Circle (point1, linePointR, linePointColor);
			}
		}
	}
	else if (orientation == Orientation::orVert)
	{
		for (std::list<ConnectionPoint*>::iterator i = outputs.begin (); i != outputs.end (); i++)
		{
			if (signaledState)
			{
				lineColor = signaledLineColor1;
				linePointColor = signaledLineColor1;
			}
			else if ((*i)->signaledState)
			{
				lineColor = signaledLineColor2;
				linePointColor = signaledLineColor2;
			}
			else
			{
				lineColor = nosignaledLineColor;
				linePointColor = nosignaledLinePointColor;
			}
				
			if ((*i)->orientation == Orientation::orHoriz) // vert -> horiz
			{
				point1.x = GetPos ().x;
				point1.y = (*i)->GetPos ().y;
			
				canvas->Line (GetPos (), point1, lineFat, lineColor);
				canvas->Line (point1, (*i)->GetPos (), lineFat, lineColor);

				canvas->Circle (point1, linePointR, linePointColor);
			}
			else if ((*i)->orientation == Orientation::orVert) // vert -> vert
			{
				y = Min (GetPos ().y, (*i)->GetPos ().y) + Abs (GetPos ().y - (*i)->GetPos ().y) * 0.5; 
		
				point1.y = y;
				point1.x = GetPos ().x;
				point2.y = y;
				point2.x = (*i)->GetPos ().x;
				
				canvas->Line (GetPos (), point1, lineFat, lineColor);
				canvas->Line (point1, point2, lineFat, lineColor);
				canvas->Line (point2, (*i)->GetPos (), lineFat, lineColor);

				if (Abs (point1.x - point2.x) > 0.01)
				{
					canvas->Circle (point1, linePointR, linePointColor);
					canvas->Circle (point2, linePointR, linePointColor);
				}
			}
		}
	}

	canvas->Circle (GetPos (), R, pointColor);

	if (selected)
	{
		GraphicalPoint pos = GetPos ();
		GraphicalPoint breaketPoint1 = GraphicalPoint (pos.x - breaketDistance, pos.y - breaketDistance);
		GraphicalPoint breaketPoint2 = GraphicalPoint (pos.x - breaketDistance, pos.y + breaketDistance);
		GraphicalPoint breaketPoint3 = GraphicalPoint (pos.x + breaketDistance, pos.y - breaketDistance);
		GraphicalPoint breaketPoint4 = GraphicalPoint (pos.x + breaketDistance, pos.y + breaketDistance);
		
		canvas->Line (breaketPoint1, breaketPoint1 + GraphicalPoint (breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint1, breaketPoint1 + GraphicalPoint (0, breaketLength), breaketFat, breaketColor);
		
		canvas->Line (breaketPoint2, breaketPoint2 + GraphicalPoint (breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint2, breaketPoint2 + GraphicalPoint (0, -breaketLength), breaketFat, breaketColor);

		canvas->Line (breaketPoint3, breaketPoint3 + GraphicalPoint (-breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint3, breaketPoint3 + GraphicalPoint (0, breaketLength), breaketFat, breaketColor);
		
		canvas->Line (breaketPoint4, breaketPoint4 + GraphicalPoint (-breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint4, breaketPoint4 + GraphicalPoint (0, -breaketLength), breaketFat, breaketColor);
	}
}

void ConnectionPoint::SetSignaled (bool signaled)
{
	signaledState = signaled;
	for (std::list<ConnectionPoint*>::iterator i = outputs.begin (); i != outputs.end (); i++)
		(*i)->SetSignaled (signaled);
}

void ConnectionPoint::SetSelected (bool selected)
{
	SetSignaled (selected);
	GraphicalObject::SetSelected (selected);
}

std::string ConnectionPoint::GetName ()
{
	return name;
}

// каскадно отмечает все дочерние точки как обработанные
void ConnectionPoint::SetCompileProcessedRecursive ()
{
	compileData.processed = true;
	for (std::list<ConnectionPoint*>::iterator i = outputs.begin (); i != outputs.end (); i++)
		(*i)->SetCompileProcessedRecursive ();
}

// каскадно вешает индекс переменной на все дочерние точки
void ConnectionPoint::SetCompileVariableRecursive (int varIndex)
{
	compileData.variableIndex = varIndex;
	for (std::list<ConnectionPoint*>::iterator i = outputs.begin (); i != outputs.end (); i++)
		(*i)->SetCompileVariableRecursive (varIndex);
}

/*******************************
*     BlockConnectionPoint
********************************/

BlockConnectionPoint::BlockConnectionPoint (Block* block, std::string& name, GraphicalPoint &pos, Orientation orientation, int dataType) : ConnectionPoint (dataType, orientation, name)
{
	this->block = block;
	this->pos = pos;
}

BlockConnectionPoint::BlockConnectionPoint (BlockConnectionPoint* blockConnectionPoint) : ConnectionPoint (blockConnectionPoint)
{
	block = blockConnectionPoint->block;
	name = blockConnectionPoint->name;
}

bool BlockConnectionPoint::CanMove ()
{
	return false;
}

Block* BlockConnectionPoint::GetBlock ()
{
	return block;
}

void BlockConnectionPoint::SetBlock (Block* block)
{
	this->block = block;
}

void BlockConnectionPoint::DisconnectAll ()
{
}

GraphicalPoint BlockConnectionPoint::GetPos ()
{
	return (pos + block->GetPos ());
}

/*******************************
*        BlockInputPoint
********************************/

BlockInputPoint::BlockInputPoint (BlockInputPoint* inputPoint) : BlockConnectionPoint (inputPoint)
{
}

void BlockInputPoint::Draw (GraphicalCanvas* canvas)
{
	static GraphicalColor textColor1 = GraphicalColor (0.0, 0.0, 0.0);
	static GraphicalColor textColor2 = GraphicalColor (1.0, 1.0, 1.0);
	std::string dataTypeText;

	BlockConnectionPoint::Draw (canvas);

	if (name.length () > 0)
	{
		if (orientation == Orientation::orHoriz)
			canvas->OutText ((char*) name.c_str(), 6, Allign::alRight,  GetPos () - GraphicalPoint (0.40, 0.1), textColor1);
		else if (orientation == Orientation::orVert)
			canvas->OutText ((char*) name.c_str(), 6, Allign::alRight,  GetPos () + GraphicalPoint (0.07, 0.1), textColor1);
	}

	if (dataType >= 0)
	{
		dataTypeText = IntToString (dataType);
		if (orientation == Orientation::orHoriz)
			canvas->OutText ((char*) dataTypeText.c_str(), 4, Allign::alLeft,  GetPos () + GraphicalPoint (0.07, -0.1), textColor2);
		else if (orientation == Orientation::orVert)
			canvas->OutText ((char*) dataTypeText.c_str(), 4, Allign::alCenter,  GetPos () - GraphicalPoint (0.1, 0.13), textColor2);
	}
}

ObjectType BlockInputPoint::GetType ()
{
	return ObjectType::otBlockInputPoint;
}

bool BlockInputPoint::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otBlockInputPoint == type);
	if (!res)
		res = BlockConnectionPoint::IsOfType (type);
	return res;
}

/*******************************
*        BlockOutputPoint
********************************/

BlockOutputPoint::BlockOutputPoint (BlockOutputPoint* outputPoint) : BlockConnectionPoint (outputPoint)
{
}

void BlockOutputPoint::Draw (GraphicalCanvas* canvas)
{
	static GraphicalColor textColor1 = GraphicalColor (0.0, 0.0, 0.0);
	static GraphicalColor textColor2 = GraphicalColor (1.0, 1.0, 1.0);
	
	std::string dataTypeText;
	
	BlockConnectionPoint::Draw (canvas);

	if (name.length () > 0)
		canvas->OutText ((char*) name.c_str(), 6, Allign::alLeft,  GetPos () + GraphicalPoint (0.04, -0.1), textColor1);

	if (dataType >= 0)
	{
		dataTypeText = IntToString (dataType);

		if (orientation == Orientation::orHoriz)
			canvas->OutText ((char*) dataTypeText.c_str(), 4, Allign::alRight,  GetPos () - GraphicalPoint (0.3, 0.1), textColor2);
		else if (orientation == Orientation::orVert)
			canvas->OutText ((char*) dataTypeText.c_str(), 4, Allign::alCenter,  GetPos () + GraphicalPoint (-0.1, 0.07), textColor2);
	}
}

ObjectType BlockOutputPoint::GetType ()
{
	return ObjectType::otBlockOutputPoint;
}

bool BlockOutputPoint::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otBlockOutputPoint == type);
	if (!res)
		res = BlockConnectionPoint::IsOfType (type);
	return res;
}

/*******************************
*            Block
********************************/

Block::Block (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints, int id)
{
	this->size = size;
	this->name = name;
	this->id = id;
	
	for (std::vector<BlockInputPoint*>::iterator i = inputPoints->begin (); i != inputPoints->end (); i++)
	{
		(*i)->SetBlock (this);
		this->inputPoints.push_back (*i);
	}

	for (std::vector<BlockOutputPoint*>::iterator i = outputPoints->begin (); i != outputPoints->end (); i++)
	{
		(*i)->SetBlock (this);
		this->outputPoints.push_back (*i);
	}
}

Block::Block (Block* block)
{
	BlockInputPoint* inputPoint;
	BlockOutputPoint* outputPoint;
	
	name = block->name;
	size = block->size;
	id = block->id;

	for (std::vector<BlockInputPoint*>::iterator i = block->inputPoints.begin (); i != block->inputPoints.end(); i++)
	{
		inputPoint = new BlockInputPoint (*i);
		inputPoint->SetBlock (this);
		inputPoints.push_back (inputPoint);
	}

	for (std::vector<BlockOutputPoint*>::iterator i = block->outputPoints.begin (); i != block->outputPoints.end(); i++)
	{
		outputPoint = new BlockOutputPoint (*i);
		outputPoint->SetBlock (this);
		outputPoints.push_back (outputPoint);
	}
}

Block::~Block ()
{
	for (std::vector<BlockInputPoint*>::iterator i = inputPoints.begin(); i != inputPoints.end(); i++)
		delete (*i);
	for (std::vector<BlockOutputPoint*>::iterator j = outputPoints.begin(); j != outputPoints.end(); j++)
		delete (*j);
}

std::string Block::GetName ()
{
	return name;
}

int Block::GetInputPointsCount ()
{
	return inputPoints.size ();
}

BlockInputPoint* Block::GetInputPoint (int index)
{
	return inputPoints[index];
}

int Block::GetOutputPointsCount ()
{
	return outputPoints.size ();
}

BlockOutputPoint* Block::GetOutputPoint (int index)
{
	return outputPoints[index];
}

bool Block::TestIntersect (GraphicalPoint& pos)
{
	static GraphicalPoint zeroPoint;
	GraphicalPoint resultPoint = pos - this->pos;
	return (resultPoint > zeroPoint) && (resultPoint < size);
}

void Block::Draw (GraphicalCanvas* canvas)
{
	static const double breaketFat = 0.01;
	static const double breaketLength = 0.1;
	static const double breaketDistance = 0.07;
	static GraphicalColor breaketColor (1.0, 0.0, 0.0);
	static GraphicalColor blockNameColor (0.0, 0.0, 0.0);
	static GraphicalColor blockColor (0.5, 0.5, 0.5);
	static GraphicalColor maskBackColor = GraphicalColor (1.0, 1.0, 1.0);

	if (selected)
	{
		GraphicalPoint breaketPoint1 = GraphicalPoint (pos.x - breaketDistance, pos.y - breaketDistance);
		GraphicalPoint breaketPoint2 = GraphicalPoint (pos.x - breaketDistance, pos.y + size.y + breaketDistance);
		GraphicalPoint breaketPoint3 = GraphicalPoint (pos.x + size.x + breaketDistance, pos.y - breaketDistance);
		GraphicalPoint breaketPoint4 = GraphicalPoint (pos.x + size.x + breaketDistance, pos.y + size.y + breaketDistance);
		
		canvas->Line (breaketPoint1, breaketPoint1 + GraphicalPoint (breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint1, breaketPoint1 + GraphicalPoint (0, breaketLength), breaketFat, breaketColor);
		
		canvas->Line (breaketPoint2, breaketPoint2 + GraphicalPoint (breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint2, breaketPoint2 + GraphicalPoint (0, -breaketLength), breaketFat, breaketColor);

		canvas->Line (breaketPoint3, breaketPoint3 + GraphicalPoint (-breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint3, breaketPoint3 + GraphicalPoint (0, breaketLength), breaketFat, breaketColor);
		
		canvas->Line (breaketPoint4, breaketPoint4 + GraphicalPoint (-breaketLength, 0), breaketFat, breaketColor);
		canvas->Line (breaketPoint4, breaketPoint4 + GraphicalPoint (0, -breaketLength), breaketFat, breaketColor);
	}
	
	canvas->Rectangle (pos, size, blockColor);

	for (std::vector<BlockInputPoint*>::iterator i = inputPoints.begin(); i != inputPoints.end(); i++)
		(*i)->Draw (canvas);
	for (std::vector<BlockOutputPoint*>::iterator j = outputPoints.begin(); j != outputPoints.end(); j++)
		(*j)->Draw (canvas);

	if (ShowNameAllowed ())
	{
		canvas->Rectangle (GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.2), GraphicalPoint (size.x - 0.1, 0.15), maskBackColor);
		canvas->OutText ((char*) name.c_str (), 10, Allign::alCenter, GraphicalPoint (pos.x + 0.00, pos.y + size.y - 0.15), blockNameColor); 
	}
}

void Block::SetPos (GraphicalPoint& pos)
{
	GraphicalObject::SetPos (pos);
}

ObjectType Block::GetType ()
{
	return ObjectType::otBlock;
}

bool Block::IsOfType (ObjectType type)
{
	return (ObjectType::otBlock == type);
}

bool Block::CanMove ()
{
	return true;
}

Block* Block::GetCopy ()
{
	return new Block (this);
}

/*******************************
*          CommandBlock
********************************/

CommandBlock::CommandBlock (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints) : Block (size, name, inputPoints, outputPoints, -1)
{
	this->mask = "";
}

CommandBlock::CommandBlock (CommandBlock* block) : Block (block)
{
	SetMask (block->mask);
}

void CommandBlock::SetMask (std::string mask)
{
	this->mask = mask;
}

ObjectType CommandBlock::GetType ()
{
	return ObjectType::otCommandBlock;
}

bool CommandBlock::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otCommandBlock == type);
	if (!res)
		res = Block::IsOfType (type);
	return res;
}

void CommandBlock::Draw (GraphicalCanvas* canvas)
{
	Block::Draw (canvas);
}

Block* CommandBlock::GetCopy ()
{
	return new CommandBlock (this);
}

/*******************************
*       InputCommandBlock
********************************/

ObjectType InputCommandBlock::GetType ()
{
	return ObjectType::otInputCommandBlock;
}

bool InputCommandBlock::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otInputCommandBlock == type);
	if (!res)
		res = CommandBlock::IsOfType (type);
		return res;
}

Block* InputCommandBlock::GetCopy ()
{
	return new InputCommandBlock (this);
}

void InputCommandBlock::Draw (GraphicalCanvas* canvas)
{
	static GraphicalColor maskColor = GraphicalColor (0.0, 0.0, 0.0);
	static GraphicalColor maskBackColor = GraphicalColor (1.0, 1.0, 1.0);
	std::string maskText;
	
	CommandBlock::Draw (canvas);

	if (GetSelected () && (outputPoints[1]->GetOutputPointsCount () == 0))
		maskText = mask + "|";
	else
		maskText = "[" + mask + "]";
	canvas->Rectangle (GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.2), GraphicalPoint (size.x - 0.1, 0.15), maskBackColor);
	canvas->OutText ((char*) maskText.c_str (), 10, Allign::alCenter, GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.15), maskColor); 
}

void InputCommandBlock::SetMask (std::string mask)
{
	if (outputPoints[1]->GetOutputPointsCount () == 0)
	{
		CommandBlock::SetMask (mask);
		outputPoints [1]->SetDataType (mask.length ());
	}
}

/*******************************
*      OutputCommandBlock
********************************/

ObjectType OutputCommandBlock::GetType ()
{
	return ObjectType::otOutputCommandBlock;
}

bool OutputCommandBlock::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otOutputCommandBlock == type);
	if (!res)
		res = CommandBlock::IsOfType (type);
	return res;
}

Block* OutputCommandBlock::GetCopy ()
{
	return new OutputCommandBlock (this);
}

void OutputCommandBlock::Draw (GraphicalCanvas* canvas)
{
	static GraphicalColor maskColor = GraphicalColor (0.0, 0.0, 0.0);
	static GraphicalColor maskBackColor = GraphicalColor (1.0, 1.0, 1.0);
	
	std::string maskText (mask);
	
	CommandBlock::Draw (canvas);

	if (GetSelected () && (inputPoints[1]->GetOutputPointsCount () == 0) && !inputConnected)
		maskText += "|";
	else
		maskText = "[" + mask + "]";
	canvas->Rectangle (GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.3), GraphicalPoint (size.x - 0.1, 0.15), maskBackColor);
	canvas->OutText ((char*) maskText.c_str (), 10, Allign::alCenter, GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.25), maskColor); 
}

void OutputCommandBlock::SetMask (std::string mask)
{	
	int found;
	
	if ((inputPoints[1]->GetOutputPointsCount () == 0) && !inputConnected)
	{
		found = mask.find_first_not_of ('_');

		if (found >= 0)
			return;
		CommandBlock::SetMask (mask);
		inputPoints [1]->SetDataType (mask.length ());
	}
}

void OutputCommandBlock::SetInputConnected (bool connected)
{
	inputConnected = connected;
}

/*******************************
*     OutputCommandBufBlock
********************************/

void OutputCommandBufBlock::SetMask (std::string mask)
{
}

ObjectType OutputCommandBufBlock::GetType ()
{
	return ObjectType::otOutputCommandBufBlock;
}

bool OutputCommandBufBlock::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otOutputCommandBufBlock == type);
	if (!res)
		res = OutputCommandBlock::IsOfType (type);
	return res;
}

Block* OutputCommandBufBlock::GetCopy ()
{
	return new OutputCommandBufBlock (this);
}

void OutputCommandBufBlock::Draw (GraphicalCanvas* canvas)
{
	CommandBlock::Draw (canvas);
}

/*******************************
*     OutputCommandConstBlock
********************************/

void OutputCommandConstBlock::SetMask (std::string mask)
{
	int found;
	
	if (!inputConnected)
	{
		found = mask.find_first_of ('_');

		if (found >= 0)
			return;
		CommandBlock::SetMask (mask);
	}
}

ObjectType OutputCommandConstBlock::GetType ()
{
	return ObjectType::otOutputCommandConstBlock;
}

bool OutputCommandConstBlock::IsOfType (ObjectType type)
{
	bool res;
	res = (ObjectType::otOutputCommandConstBlock == type);
	if (!res)
		res = OutputCommandBlock::IsOfType (type);
	return res;
}

Block* OutputCommandConstBlock::GetCopy ()
{
	return new OutputCommandConstBlock (this);
}

void OutputCommandConstBlock::Draw (GraphicalCanvas* canvas)
{
	static GraphicalColor maskColor = GraphicalColor (0.0, 0.0, 0.0);
	static GraphicalColor maskBackColor = GraphicalColor (1.0, 1.0, 1.0);
	std::string maskText (mask);
	
	CommandBlock::Draw (canvas);

	if (GetSelected () && !inputConnected)
		maskText += "|";
	else
		maskText = "[" + mask + "]";
	canvas->Rectangle (GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.3), GraphicalPoint (size.x - 0.1, 0.15), maskBackColor);
	canvas->OutText ((char*) maskText.c_str (), 10, Allign::alCenter, GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.25), maskColor); 

	canvas->OutText ("Const", 10, Allign::alCenter, GraphicalPoint (pos.x + 0.05, pos.y + size.y - 0.1), GraphicalColor (1.0, 1.0, 1.0)); 
}