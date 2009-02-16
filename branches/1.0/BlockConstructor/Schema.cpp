#include "StdAfx.h"
#include "Schema.h"

/*******************************
*            Schema
********************************/

Schema::Schema (GraphicalCanvas* canvas, std::string name)
{
	this->canvas = canvas;
	selectedObject = NULL;
	firstDrawPoint = NULL;
	state = State::doNothing;
	tool = Tool::addBlock;
	this->name = name;
	objectSelectedCallback = NULL;
	scrollCallback = NULL;
}

Schema::~Schema ()
{
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
		delete (*i);
	for (std::list<ConnectionPoint*>::iterator j = points.begin(); j != points.end(); j++)
		delete (*j);
}

std::string Schema::GetName ()
{
	return name;
}

void Schema::SetName (std::string name)
{
	this->name = name;
}

GraphicalObject* Schema::TestIntersect (GraphicalPoint &pos)
{
	int inputPointsCount;
	int outputPointsCount;
	
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		inputPointsCount = (*i)->GetInputPointsCount ();
		outputPointsCount = (*i)->GetOutputPointsCount ();
		
		for (int j = 0; j < inputPointsCount; j++)
			if ((*i)->GetInputPoint (j)->TestIntersect (pos))
				return (*i)->GetInputPoint (j);

		for (int k = 0; k < outputPointsCount; k++)
			if ((*i)->GetOutputPoint (k)->TestIntersect (pos))
				return (*i)->GetOutputPoint (k);
		
		if ((*i)->TestIntersect (pos))
			return (*i);
	}

	for (std::list<ConnectionPoint*>::iterator j = points.begin(); j != points.end(); j++)
		if ((*j)->TestIntersect (pos))
			return (*j);

	return 0;
}

ConnectionPoint* Schema::FindParentPoint (ConnectionPoint* connectionPoint)
{
	int inputPointsCount;
	int outputPointsCount;
	ConnectionPoint* outputPoint;
	ConnectionPoint* currentConnectionPoint;
	
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		inputPointsCount = (*i)->GetInputPointsCount ();
		outputPointsCount = (*i)->GetOutputPointsCount ();
		
		for (int j = 0; j < inputPointsCount; j++)
		{
			currentConnectionPoint = (*i)->GetInputPoint (j);
			outputPoint = currentConnectionPoint->GetFirstOutputPoint();
			while (outputPoint != 0)
			{
				if (outputPoint == connectionPoint)
					return currentConnectionPoint;
				outputPoint = currentConnectionPoint->GetNextOutputPoint();
			}
		}

		for (int k = 0; k < outputPointsCount; k++)
		{
			currentConnectionPoint = (*i)->GetOutputPoint (k);
			outputPoint = currentConnectionPoint->GetFirstOutputPoint();
			while (outputPoint != 0)
			{
				if (outputPoint == connectionPoint)
					return currentConnectionPoint;
				outputPoint = currentConnectionPoint->GetNextOutputPoint();
			}
		}
	}

	for (std::list<ConnectionPoint*>::iterator m = points.begin(); m != points.end(); m++)
	{
			currentConnectionPoint = (*m);
			outputPoint = currentConnectionPoint->GetFirstOutputPoint();
			while (outputPoint != 0)
			{
				if (outputPoint == connectionPoint)
					return currentConnectionPoint;
				outputPoint = currentConnectionPoint->GetNextOutputPoint();
			}
	}

	return 0;
}

void Schema::Draw ()
{
	canvas->Box (canvas->GetMaxCanvasSize () * -0.5 + GraphicalPoint (0.1, 0.1), canvas->GetMaxCanvasSize () - GraphicalPoint (0.2, 0.2), 0.1, GraphicalColor (0.5, 0.5, 0.5));
	
	if (firstDrawPoint)
		firstDrawPoint->Draw (canvas);
	
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
		(*i)->Draw (canvas);
	for (std::list<ConnectionPoint*>::iterator j = points.begin(); j != points.end(); j++)
		if ((*j) != firstDrawPoint)
			(*j)->Draw (canvas);

	canvas->Flush ();
}

void Schema::SetTool (Tool tool)
{
	this->tool = tool;
}

Schema::Tool Schema::GetTool ()
{
	return tool;
}

void Schema::OnMouseMove (GraphicalPoint &pos)
{
	if ((state == moveSelected) && (selectedObject))
	{
		selectedObject->SetPos (canvas->GetCanvasPoint (pos) + addToSelected);
		Draw ();
	}
	else if (state == moveSchema)
	{
		canvas->SetVScroll (canvas->GetScroll ().y - (LBDownPos.y - canvas->GetCanvasPoint (pos).y) / canvas->GetMaxCanvasSize ().y);
		canvas->SetHScroll (canvas->GetScroll ().x + (LBDownPos.x - canvas->GetCanvasPoint (pos).x) / canvas->GetMaxCanvasSize ().x);
		if (scrollCallback)
			scrollCallback (canvas->GetScroll ());
		Draw ();
	}
}

void Schema::OnObjectSetSelected (GraphicalObject* object, bool selected)
{
	if (object == NULL)
	{
		firstDrawPoint = NULL;
		return;
	}
	
	if ((object->GetType () == ObjectType::otConnectionPoint) ||
		(object->GetType () == ObjectType::otBlockInputPoint))
	{
		ConnectionPoint* parentPoint = FindParentPoint ((ConnectionPoint*) object);
		if (parentPoint)
		{
			parentPoint->OnOutputSetSelected ((ConnectionPoint*) object, selected);
			firstDrawPoint = parentPoint;
		}
	}
	else if ((object->GetType () == ObjectType::otOutputCommandBlock) || (object->GetType () == ObjectType::otOutputCommandBufBlock))
	{
		bool inputConnected;
		inputConnected = (FindParentPoint (((OutputCommandBlock*)object)->GetInputPoint (1)) != NULL);
		((OutputCommandBlock*)object)->SetInputConnected (inputConnected);
	}
}

void Schema::OnLBDown (GraphicalPoint &pos)
{		
	if (selectedObject)
	{
		selectedObject->SetSelected (false);
		OnObjectSetSelected (selectedObject, false);
	}

	selectedObject = TestIntersect (canvas->GetCanvasPoint (pos));
	if (selectedObject)
	{
		addToSelected = selectedObject->GetPos () - canvas->GetCanvasPoint (pos);
		selectedObject->SetSelected (true);
		OnObjectSetSelected (selectedObject, true);
		if (selectedObject->CanMove ())
			state = moveSelected;
	}
	else
	{
		LBDownPos = canvas->GetCanvasPoint (pos);
		state = moveSchema;
	}
	
	if (objectSelectedCallback)
		objectSelectedCallback (selectedObject);
	Draw ();
}

void Schema::OnLBUp (GraphicalPoint &pos)
{
	state = doNothing;
}

void Schema::OnRBDown (GraphicalPoint &pos)
{
	if (state != doNothing)
		return;
	
	if (tool == Tool::addBlock)
	{
		Block* curBlock = BlockFactory::GetBlockFactory()->GetCurBlock ();
		if (curBlock)
		{
			Block* block = curBlock->GetCopy ();
			block->SetPos (canvas->GetCanvasPoint (pos));
			blocks.push_back (block);
		}
	}
	else if (tool == Tool::addPoint)
	{
		if (selectedObject != 0)
		{
			if ((selectedObject->GetType () == otConnectionPoint) || (selectedObject->GetType () == otBlockOutputPoint))
			{
				GraphicalObject* object = TestIntersect (canvas->GetCanvasPoint (pos));
				if (object)
				{
					if ((object->GetType () == otConnectionPoint) || (object->GetType () == otBlockInputPoint))
					{
						if (((ConnectionPoint*)selectedObject)->GetDataType () == ((ConnectionPoint*)object)->GetDataType ())
						{
							if ((((ConnectionPoint*)selectedObject)->GetDataType () > 0) || (((ConnectionPoint*)selectedObject)->GetOutputPointsCount () < 1))
							{
								if (FindParentPoint ((ConnectionPoint*) object) == 0)
									((ConnectionPoint*) selectedObject)->Connect ((ConnectionPoint*) object);
							}
						}
					}
				}
				else
				{
					if (((ConnectionPoint*)selectedObject)->GetDataType () > 0)
					{
						ConnectionPoint* connectionPoint = new ConnectionPoint(((ConnectionPoint*)selectedObject)->GetDataType (), Orientation::orHoriz, std::string(""));
						connectionPoint->SetPos (canvas->GetCanvasPoint (pos));
						points.push_back (connectionPoint);
						((ConnectionPoint*) selectedObject)->Connect (connectionPoint);
					}
				}
			}
		}
	}

	Draw ();
}

void Schema::OnRBUp (GraphicalPoint &pos)
{
}

void Schema::CascadeDeleteConnectionPoint (ConnectionPoint* point)
{
	ConnectionPoint* tmpPoint = point->GetFirstOutputPoint ();
	while (tmpPoint)
	{
		CascadeDeleteConnectionPoint (tmpPoint);
		tmpPoint = point->GetNextOutputPoint ();
	}

	point->DisconnectAll ();

	if (point->GetType () == ObjectType::otConnectionPoint)
	{
		std::list<ConnectionPoint*>::iterator pointIterator = FindPointIterator (point);
		if (pointIterator != points.end ())
		{
			delete (*pointIterator);
			points.erase (pointIterator);
		}
	}
}
		
std::list<ConnectionPoint*>::iterator Schema::FindPointIterator (ConnectionPoint* connectionPoint)
{
	for (std::list<ConnectionPoint*>::iterator i = points.begin(); i != points.end(); i++)
		if ((*i) == connectionPoint)
			return i;
	return points.end();
}

std::list<Block*>::iterator Schema::FindBlockIterator (Block* block)
{
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
		if ((*i) == block)
			return i;
	return blocks.end();
}

void Schema::OnDelKeyPressed ()
{
	bool deleted = false;
	
	if (!selectedObject)
		return;

	if (state != State::doNothing)
		return;

	if (selectedObject->IsOfType (ObjectType::otConnectionPoint))
	{
		selectedObject->SetSelected (false);
		
		ConnectionPoint* point = (ConnectionPoint*) selectedObject;
		ConnectionPoint* parentPoint = FindParentPoint (point);
		if (parentPoint)
			parentPoint->Disconnect (point);

		CascadeDeleteConnectionPoint (point);

		deleted = true;
	}
	else if (selectedObject->IsOfType (ObjectType::otBlock))
	{
		selectedObject->SetSelected (false);

		Block* block = (Block*) selectedObject;
		for (int i = 0; i < block->GetInputPointsCount (); i++)
		{
			ConnectionPoint* parentPoint = FindParentPoint (block->GetInputPoint (i));
			if (parentPoint)
				parentPoint->Disconnect (block->GetInputPoint (i));
		}

		for (int i = 0; i < block->GetOutputPointsCount (); i++)
			CascadeDeleteConnectionPoint (block->GetOutputPoint (i));

		std::list<Block*>::iterator blockIterator = FindBlockIterator (block);
		if (blockIterator != blocks.end ())
		{
			delete (*blockIterator);
			blocks.erase (blockIterator);
		}

		deleted = true;
	}

	if (deleted)
	{
		selectedObject = NULL;
		OnObjectSetSelected (NULL, false);
		if (objectSelectedCallback)
			objectSelectedCallback (NULL);
		Draw ();
	}
}

void Schema::OnKeyPressed (char key)
{
	static std::string abc ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-+\\/<>:;\"\'?,.=1234567890_");
	int found;

	if (selectedObject && (selectedObject->IsOfType (ObjectType::otCommandBlock)))
	{
		CommandBlock* block = (CommandBlock*) selectedObject;
		std::string mask = block->GetMask ();

		found = abc.find_first_of (key);

		if (key == 8) // Backspase
		{
			if ((mask.length () > 0))
				mask = mask.substr (0, mask.length () - 1);
		}
		else if ((mask.length () < 8) && (found >= 0))
		{
			mask += key;
		}

		block->SetMask (mask);
		Draw ();
	}
}

void Schema::SetObjectSelectedCallback (ObjectSelectedCallback* callback)
{
	objectSelectedCallback = callback;
}

void Schema::SetScrollCallback (ScrollCallback* callback)
{
	scrollCallback = callback;
}

// ********* ���������� *********

// ���� ���� ��������� ���� ������� �� ����� ������������ ����� ���������� ��� ������
Block* Schema::FindBlockWithoutInputs (ObjectType objectType)
{
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->GetType () == objectType)
		{
			int j;
			
			for (j = 0; j < (*i)->GetInputPointsCount (); j++)
			{
				if (FindParentPoint ((*i)->GetInputPoint (j)))
					break;
			}

			if (j < (*i)->GetInputPointsCount ())
				continue;

			return (*i);
		}
	}

	return NULL;
}

// ���� ������ �� ������� ������� ����� �������� ������ ���� �������� �������
OutputCommandBlock* Schema::FindFirstOutputCommandBlock ()
{
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->IsOfType (ObjectType::otOutputCommandBlock))
		{
			if (FindParentPoint ((*i)->GetInputPoint (0)) == NULL)
				return (OutputCommandBlock*)(*i);
		}
	}

	return NULL;
}

// ���������� ��������� ������������� ����� ������� �������
std::string Schema::GetCommandMask ()
{
	std::string mask;
	InputCommandBlock* block = (InputCommandBlock*) FindBlockWithoutInputs (ObjectType::otInputCommandBlock);
	BlockConnectionPoint* point;
		
	while (block)
	{
		mask += block->GetMask ();
		
		point = (BlockConnectionPoint*) block->GetOutputPoint(0)->GetFirstOutputPoint ();

		if (point)
			block = (InputCommandBlock*) point->GetBlock ();
		else
			block = NULL;
	}

	return mask;
}

// ������� ���������� ������� ������
int Schema::GetBlocksCount ()
{
	int res = 0;
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->GetType () == ObjectType::otBlock)
			res++;
	}
	return res;
}

// ����� ������ ���������� ��� ���� �������� � �������� ��������
void Schema::ResetCompileData ()
{
	int inputPointsCount;
	int outputPointsCount;
	ConnectionPoint* currentConnectionPoint;

	variables.clear ();
	
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		(*i)->compileData.processed = false;
		(*i)->compileData.order = -1;
		
		inputPointsCount = (*i)->GetInputPointsCount ();
		outputPointsCount = (*i)->GetOutputPointsCount ();
		
		for (int j = 0; j < inputPointsCount; j++)
		{
			currentConnectionPoint = (*i)->GetInputPoint (j);
			currentConnectionPoint->compileData.processed = false;
			currentConnectionPoint->compileData.variableIndex = -1;
		}

		for (int k = 0; k < outputPointsCount; k++)
		{
			currentConnectionPoint = (*i)->GetOutputPoint (k);
			currentConnectionPoint->compileData.processed = false;
			currentConnectionPoint->compileData.variableIndex = -1;
		}
	}

	for (std::list<ConnectionPoint*>::iterator m = points.begin(); m != points.end(); m++)
	{
			currentConnectionPoint = (*m);
			currentConnectionPoint->compileData.processed = false;
			currentConnectionPoint->compileData.variableIndex = -1;
	}
}

// ������� ����, � �������� ��� ����� �������� ��� ������������
// ��������������� ����� ����� ������� ������� � ������� �����
// ����� ���� ������, �� ���������� ��� ������������
// ��� ��� ������ ���������� ���������� ��� ������������
Block* Schema::FindBlockWithProcessedInputs ()
{
	int inputPointsCount;
	int outputPointsCount;
	ConnectionPoint* currentConnectionPoint;
	
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->compileData.processed)
			continue;

		// ����� �������� ������ ������ ����������
		if ((*i)->GetType () == ObjectType::otOutputCommandBlock)
			continue;

		// ���� ������� ������� ����� ������� ��� �����������
		if ((*i)->GetType () != ObjectType::otInputCommandBlock)
		{
			int j;
			
			// ��� �������� ����� ����������� �������� ������ �� ��������������
			inputPointsCount = (*i)->GetInputPointsCount ();
			
			for (j = 0; j < inputPointsCount; j++)
			{
				currentConnectionPoint = (*i)->GetInputPoint (j);
				if (!currentConnectionPoint->compileData.processed)
					break;
			}

			if (j < inputPointsCount)
				continue;
		}

		// ���� �������. ������� ��� ������ ��� ������������
		(*i)->compileData.processed = true;
		outputPointsCount = (*i)->GetOutputPointsCount ();
		for (int k = 0; k < outputPointsCount; k++)
		{
			currentConnectionPoint = (*i)->GetOutputPoint (k);
			currentConnectionPoint->SetCompileProcessedRecursive ();
		}
		
		return (*i);
	}

	return NULL;
}

// ������ ��� �������� ����� ���������� ������ ������, � �������� ��� ��������� 
void Schema::GetEndBlocksRecursive (ConnectionPoint* point, std::vector<Block*> *endBlocks)
{
	ConnectionPoint* out;
	
	if (point->GetType () == ObjectType::otBlockInputPoint)
	{
		endBlocks->push_back (((BlockInputPoint*) point)->GetBlock ());
		return;
	}

	out = point->GetFirstOutputPoint ();

	while (out)
	{
		GetEndBlocksRecursive (out, endBlocks);
		out = point->GetNextOutputPoint ();
	}
}

// ������ ��� �������� ����� ���������� ����� ������� � ������� ��������� ���� 
Block* Schema::GetOldestBlockOrderRecursive (ConnectionPoint* point)
{
	std::vector<Block*> endBlocks;
	Block* res;

	GetEndBlocksRecursive (point, &endBlocks);
	res = (endBlocks.size () > 0) ? endBlocks [0] : NULL;
	for (std::vector<Block*>::iterator i = endBlocks.begin(); i != endBlocks.end(); i++)
	{ 
		if ((*i)->compileData.order > res->compileData.order)
			res = (*i);
	}

	return res;
}

// ����������� ������ � ���� ������ ��� ����� ����������
// ���������� ����� ���������� ������
int Schema::AllocateMemoryForVariable (int varSize)
{
	int freeBytesCount = 0;
	int freeBlockAddress;
	
	for (int i = 0; i < memoryPool->GetSize (); i++)
	{
		if (memoryPool->IsFreeByte (i))
		{
			freeBytesCount++;

			if (freeBytesCount == varSize)
			{
				freeBlockAddress = i - varSize + 1;
				for (int j = freeBlockAddress; j < freeBlockAddress + varSize; j++)
					memoryPool->AllocateByte (j);
				return freeBlockAddress;
			}
		}
		else
		{
			freeBytesCount = 0;
		}
	}

	throw CompilationException ("���������� �������� ������ ��� ����������. ��� ������ ��������. ������ ���� ������ " + IntToString (memoryPool->GetSize ()) + " ����. ���������� ��������� ���������� ������ � �����.");
}

// ��������� � ���� ������ ����� �� ��� ����������, ����� ����� ������� �������
// � ������� variables ������ ���������� ��������� �� �����, �.�. � ���������� ���� ��������� �� �������
// ������ ������ ���������� ����� ��������� ������� �� ������� ������
void Schema::GarbageCollect (int lifeTime)
{
	for (int i = 0; i < variables.size (); i++)
	{
		if (variables[i].active && (variables[i].lifeTime <= lifeTime))
		{
			variables[i].active = false;
			for (int j = variables[i].address; j < variables[i].address + variables[i].size; j++)
			{
				memoryPool->FreeByte (j);
			}
		}
	}
}

// ������� ����� ����������
// ������ �� �� point � ��������� �� ��� �������� �����
Schema::Variable Schema::AddVariable (ConnectionPoint* point)
{
	Variable var;
	Block* oldestBlock;
	int varIndex;
	ConnectionPoint* out;

	oldestBlock = GetOldestBlockOrderRecursive (point);
	var.size = point->GetDataType ();
	var.address = AllocateMemoryForVariable (var.size);
	var.active = true;
	
	var.lifeTime = oldestBlock ? oldestBlock->compileData.order : 0;
	
	variables.push_back (var);
	varIndex = variables.size () - 1;

	// ���������� ������� ���������� �� ������ � ��� �������� �����
	point->SetCompileVariableRecursive (varIndex);

	return var;
}

void Schema::Compile (RomData &romData, MemoryPool *memoryPool, int *pos)
{
	std::vector <Block*> sortedBlocks;	// ������ ������ ������������� �� ����������� ��������� ������
	int schemaHeaderPos;				// ������� � ���� �������� ������ �����
	int schemaBlockHeaderPos;			// ������� � ���� �������� �����					
	int inMaskPos;						// ������� � ����� ������� ��� �������� �������

	Block* block;						// ��������������� ����������
	BlockConnectionPoint* point;
	int order;
	Variable var;
	int blocksCount;

	try
	{

// ----- ���������� -----

		this->memoryPool = memoryPool;
		memoryPool->Free ();

		// ������� ��������� ������ ����������
		ResetCompileData ();

		// �������� ������ ������� � ��� �������, � ������� ��� ����� ��������������
		block = FindBlockWithProcessedInputs ();
		while (block)
		{
			if (block->GetType () == ObjectType::otBlock)
			{
				block->compileData.order = sortedBlocks.size ();
				sortedBlocks.push_back (block);
			}
			block = FindBlockWithProcessedInputs ();
		}

		// ������� ������� ��������� �������� ������ ������ ���������� �������
		order = sortedBlocks.size ();
		for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
		{
			if ((*i)->IsOfType(ObjectType::otOutputCommandBlock))
			{
				(*i)->compileData.processed = true;
				(*i)->compileData.order = order;
				order++;
			}
		}

// ----- ��������� ����� -----

		blocksCount = GetBlocksCount () + 2;
		schemaHeaderPos = (*pos);
		// ������ ���� ��������� ����� �������� ���������� ������ (+ 2 ��������� ����� 0 � 1)
		romData [schemaHeaderPos] = blocksCount;
		schemaHeaderPos++;

		// ��������� ��������� �����
		(*pos) += 1 + blocksCount * 3; // 1 ���� ���������� ������ + 3 ����� �� ������ ���� (1 ���� id ����� + 2 ����� ����� ����������)

// ----- ������� �������. �������� 0 -----

		// ������� �������� 0 � ��������� �����
		romData [schemaHeaderPos] = 0; // id �����
		schemaHeaderPos++;
		// ����� ����������
		romData [schemaHeaderPos] = (unsigned char)((*pos) >> 8); // ������� ���� ������
		schemaHeaderPos++;
		romData [schemaHeaderPos] = (unsigned char) (*pos); // ������� ���� ������
		schemaHeaderPos++;

		// ���������� ��������������� ����� ������� �������
		// ��� ������� ����� ������� ������� ������� ���������
		// ����� ����� � ��� ���������� �� ��������� 0
		blocksCount = 0;
		inMaskPos = 0;
		schemaBlockHeaderPos = (*pos);
		(*pos)++;
		block = FindBlockWithoutInputs (ObjectType::otInputCommandBlock);
		while (block)
		{
			// ���� ����� ����� � ���-������ ��������, �� ����� ��������� ����������
			if (block->GetOutputPoint(1)->GetFirstOutputPoint ())
			{		
				// ������� ����������
				var = AddVariable (block->GetOutputPoint(1));
				blocksCount++;

				// ����� ���������� �� ��������� 0 � ���
				romData [(*pos)] = (unsigned char) inMaskPos; // ������
				(*pos)++;
				romData [(*pos)] = (unsigned char) var.address; // ����
				(*pos)++;
				romData [(*pos)] = (unsigned char) var.size; // �������
				(*pos)++;
			}

			inMaskPos += ((InputCommandBlock*)block)->GetMask ().length ();
			
			point = (BlockConnectionPoint*) block->GetOutputPoint(0)->GetFirstOutputPoint ();
			if (point)
				block = (InputCommandBlock*) point->GetBlock ();
			else
				block = NULL;
		}

		// ���-�� ���������� ��������� 0
		romData [schemaBlockHeaderPos] = blocksCount;


// ----- �������� ����� -----

		// ��������� �������� ����� � ������� ���������
		// ������� ����������, ����� ����� ������ � ���

		for (std::vector<Block*>::iterator i = sortedBlocks.begin(); i != sortedBlocks.end(); i++)
		{
			// ������� ���� � ��������� �����
			int xxx = (*i)->getId ();
			romData [schemaHeaderPos] = (*i)->getId (); // id �����
			schemaHeaderPos++;
			// ����� ����������
			romData [schemaHeaderPos] = (unsigned char)((*pos) >> 8); // ������� ���� ������
			schemaHeaderPos++;
			romData [schemaHeaderPos] = (unsigned char) (*pos); // ������� ���� ������
			schemaHeaderPos++;

			// ������� ���������
			for (int j = 0; j < (*i)->GetInputPointsCount (); j++)
			{
				romData [(*pos)] = (unsigned char) variables [(*i)->GetInputPoint (j)->compileData.variableIndex].address;
				(*pos)++;
			}

			// ���������� ���������� �� ����� � ����� ��������� �������� ���������
			for (int j = 0; j < (*i)->GetOutputPointsCount (); j++)
			{
				var = AddVariable ((*i)->GetOutputPoint (j));

				romData [(*pos)] = (unsigned char) var.address;
				(*pos)++;
			}

			// ����������, �������� ������ � ������ ���� ������ �� �����
			GarbageCollect ((*i)->compileData.order);
		}

// ----- �������� �������. �������� 1 -----

		// ������� �������� 1 � ��������� �����
		romData [schemaHeaderPos] = 1; // id �����
		schemaHeaderPos++;
		// ����� ����������
		romData [schemaHeaderPos] = (unsigned char)((*pos) >> 8); // ������� ���� ������
		schemaHeaderPos++;
		romData [schemaHeaderPos] = (unsigned char) (*pos); // ������� ���� ������
		schemaHeaderPos++;

		// ���������� ��������������� ����� �������� �������
		// ����� � ��� ���������� �� ��������� 1
		blocksCount = 0;
		schemaBlockHeaderPos = (*pos);
		(*pos)++;
		block = FindFirstOutputCommandBlock ();
		while (block)
		{
			// ����� ���������� �� ��������� 1 � ���
			// ��� ���� ����� �������� ������ ������� ������ ����������
			if (block->GetType () == ObjectType::otOutputCommandBlock)
			{
				blocksCount++;
				
				var = variables [block->GetInputPoint (1)->compileData.variableIndex];

				romData [(*pos)] = (unsigned char) var.address; // ������
				(*pos)++;
				romData [(*pos)] = OutParamModifier::outVar; // ����������� ���������
				(*pos)++;
				romData [(*pos)] = (unsigned char) var.size; // �������
				(*pos)++;
			}
			else if (block->GetType () == ObjectType::otOutputCommandBufBlock)
			{
				blocksCount++;
				
				// ��� ������ ��� ���������� ������ ����������
				var = variables [block->GetInputPoint (1)->compileData.variableIndex];

				romData [(*pos)] = (unsigned char) var.address; // ������
				(*pos)++;
				romData [(*pos)] = OutParamModifier::outBuf; // ����������� ���������
				(*pos)++;
				romData [(*pos)] = (unsigned char) var.size; // �������
				(*pos)++;
			}
			else if (block->GetType () == ObjectType::otOutputCommandConstBlock)
			{
				// ��� ��������� ������ ������� ����� �� ���

				for (int i= 0; i < ((OutputCommandConstBlock*)block)->GetMask ().length (); i++)
				{
					blocksCount++;

					romData [(*pos)] = (unsigned char) ((OutputCommandConstBlock*)block)->GetMask ().c_str ()[i]; // ������ (�� ����� ���� ���������� ���� ���������)
					(*pos)++;
					romData [(*pos)] = OutParamModifier::outConst; // ����������� ���������
					(*pos)++;
					romData [(*pos)] = (unsigned char) 1; // �������
					(*pos)++;
				}
			}

			point = (BlockConnectionPoint*) block->GetOutputPoint (0)->GetFirstOutputPoint ();
			if (point)
				block = (OutputCommandBlock*) point->GetBlock ();
			else
				block = NULL;
		}

		// ���-�� ���������� ��������� 1
		romData [schemaBlockHeaderPos] = blocksCount;
	}
	catch (CompilationException e)
	{
		throw CompilationException ("��� ���������� ����� " + GetName () + " ��������� ������. " + e.GetErrorMessage ());
	}
}

// ��������� ��������� �� ������� �����
// ������������� ����� ������ ���������� �����
void Schema::Validate ()
{
	Block* block;
	std::string errorReport;
	const std::string separator ("--------------------------------------------------------------\n");
	
	// ����� ������� � �������� ������ ������ ���� ��������� � �������
	block = NULL;
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->IsOfType (ObjectType::otInputCommandBlock))
		{
			if (FindParentPoint ((*i)->GetInputPoint (0)) == NULL)
			{
				if (block != NULL)
				{
					errorReport += "> �� ��� ����� ������� ������� ��������� � �������\n";
					break;
				}
				block = (*i);
			}
		}
	}
	if (block == NULL)
		errorReport += "> ��������� ���� �� ���� ���� ������� �������\n";

	block = NULL;
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->IsOfType (ObjectType::otOutputCommandBlock))
		{
			if (FindParentPoint ((*i)->GetInputPoint (0)) == NULL)
			{
				if (block != NULL)
				{
					errorReport += "> �� ��� ����� �������� ������� ��������� � �������\n";
					break;
				}
				block = (*i);
			}	
		}
	}
	if (block == NULL)
		errorReport += "> ��������� ���� �� ���� ���� �������� �������\n";

	// �� ������ ���� ������� ������
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if (((*i)->GetType () == ObjectType::otOutputCommandBlock) || ((*i)->GetType () == ObjectType::otOutputCommandBufBlock))
		{
			if (FindParentPoint(((OutputCommandBlock*)(*i))->GetInputPoint (1)) == NULL)
				errorReport += "> ������ ���� �������� ������� � ������� ������\n";
		}
		else if ((*i)->GetType () == ObjectType::otBlock)
		{
			for (int j = 0; j < (*i)->GetInputPointsCount (); j++)
			{
				if (FindParentPoint((*i)->GetInputPoint (j)) == NULL)
					errorReport += "> ������ ������� ���� � ����� " + (*i)->GetName () + "\n";
			}
		}
	}

	// �� ������ ���� ����� ��������� ��� �������
	for (std::list<ConnectionPoint*>::iterator i = points.begin(); i != points.end(); i++)
		if ((*i)->GetOutputPointsCount () <= 0)
			errorReport += "> ������� ����� ���������� � ������� �������\n";

	// �� ������ ���� ����� ������� �����
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		if ((*i)->GetType () == ObjectType::otInputCommandBlock)
		{
			if (((CommandBlock*)(*i))->GetMask ().length () <= 0)
				errorReport += "> ������ ���� ������� ������� � ������ ������� �����\n";
		}
		else if (((*i)->GetType () == ObjectType::otOutputCommandBlock) || ((*i)->GetType () == ObjectType::otOutputCommandConstBlock) )
		{
			if (((CommandBlock*)(*i))->GetMask ().length () <= 0)
				errorReport += "> ������ ���� �������� ������� � ������ ������� �����\n";
		}
	}

	if (errorReport.size () > 0)
		throw ValidationException (separator + "� ����� " + GetName () + " ���������� ������\n" + separator + errorReport + "\n");
}

void Schema::Save (CComPtr<xml::IXMLDOMNode> xmlSchemaNode)
{
	std::map <GraphicalObject*, int> uniqueIdMap;
	int inputPointsCount;
	int outputPointsCount;
	ConnectionPoint *connection;
	int curId = 1;
	CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlBlockNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlConnectionsNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlConnectionNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlConnectionsConnectionsNode = NULL;

	// �������� ����� ������������ ������ - id
	
	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		inputPointsCount = (*i)->GetInputPointsCount ();
		outputPointsCount = (*i)->GetOutputPointsCount ();

		uniqueIdMap[(*i)] = curId;
		curId++;
		
		for (int j = 0; j < inputPointsCount; j++)
		{
			uniqueIdMap[(*i)->GetInputPoint (j)] = curId;
			curId++;
		}

		for (int k = 0; k < outputPointsCount; k++)
		{
			uniqueIdMap[(*i)->GetOutputPoint (k)] = curId;
			curId++;
		}
	}

	for (std::list<ConnectionPoint*>::iterator m = points.begin(); m != points.end(); m++)
	{
		uniqueIdMap[(*m)] = curId;
		curId++;	
	}

	// ����������

	if (xmlTempNode)
		xmlTempNode.Release();
	xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("Name");
	if (xmlTempNode.p == NULL)
		throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Name");
	xmlTempNode->text = this->name.c_str ();
	xmlSchemaNode->appendChild (xmlTempNode);

	// �����

	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		inputPointsCount = (*i)->GetInputPointsCount ();
		outputPointsCount = (*i)->GetOutputPointsCount ();
		
		if (xmlBlockNode)
			xmlBlockNode.Release();
		xmlBlockNode = xmlSchemaNode->GetownerDocument()->createElement("Block");
		if (xmlBlockNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block");
		xmlSchemaNode->appendChild (xmlBlockNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("Name");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Name");
		xmlTempNode->text = (*i)->GetName ().c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("UniqueId");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/UniqueId");
		xmlTempNode->text = IntToString (uniqueIdMap[(*i)]).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("X");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/X");
		xmlTempNode->text = DoubleToString ((*i)->GetPos().x).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("Y");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Y");
		xmlTempNode->text = DoubleToString ((*i)->GetPos().y).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if ((*i)->IsOfType(ObjectType::otInputCommandBlock) || (*i)->IsOfType(ObjectType::otOutputCommandBlock))
		{
			if (xmlTempNode)
				xmlTempNode.Release();
			xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("Mask");
			if (xmlTempNode.p == NULL)
				throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Mask");
			xmlTempNode->text = ((CommandBlock*)(*i))->GetMask ().c_str ();
			xmlBlockNode->appendChild (xmlTempNode);
		}

		if (xmlConnectionsNode)
			xmlConnectionsNode.Release();
		xmlConnectionsNode = xmlSchemaNode->GetownerDocument()->createElement("Inputs");
		if (xmlConnectionsNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Inputs");
		xmlBlockNode->appendChild (xmlConnectionsNode);
		
		for (int j = 0; j < inputPointsCount; j++)
		{
			if (xmlConnectionNode)
				xmlConnectionNode.Release();
			xmlConnectionNode = xmlSchemaNode->GetownerDocument()->createElement("UniqueId");
			if (xmlConnectionNode.p == NULL)
				throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Inputs/UniqueId");
			xmlConnectionNode->text = IntToString (uniqueIdMap[(*i)->GetInputPoint (j)]).c_str ();
			xmlConnectionsNode->appendChild (xmlConnectionNode);	
		}

		if (xmlConnectionsNode)
			xmlConnectionsNode.Release();
		xmlConnectionsNode = xmlSchemaNode->GetownerDocument()->createElement("Outputs");
		if (xmlConnectionsNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Outputs");
		xmlBlockNode->appendChild (xmlConnectionsNode);

		for (int k = 0; k < outputPointsCount; k++)
		{
			if (xmlConnectionNode)
				xmlConnectionNode.Release();
			xmlConnectionNode = xmlSchemaNode->GetownerDocument()->createElement("Output");
			if (xmlConnectionNode.p == NULL)
				throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Outputs/Output");
			xmlConnectionsNode->appendChild (xmlConnectionNode);	

			if (xmlTempNode)
				xmlTempNode.Release();
			xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("UniqueId");
			if (xmlTempNode.p == NULL)
				throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Outputs/Output/UniqueId");
			xmlTempNode->text = IntToString (uniqueIdMap[(*i)->GetOutputPoint (k)]).c_str ();
			xmlConnectionNode->appendChild (xmlTempNode);
			
			if (xmlConnectionsConnectionsNode)
				xmlConnectionsConnectionsNode.Release();
			xmlConnectionsConnectionsNode = xmlSchemaNode->GetownerDocument()->createElement("Connections");
			if (xmlConnectionsConnectionsNode.p == NULL)
				throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Outputs/Output/Connections");
			xmlConnectionNode->appendChild (xmlConnectionsConnectionsNode);

			connection = (*i)->GetOutputPoint (k)->GetFirstOutputPoint ();
			while (connection)
			{
				if (xmlTempNode)
					xmlTempNode.Release();
				xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("UniqueId");
				if (xmlTempNode.p == NULL)
					throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Block/Outputs/Output/Connections/UniqueId");
				xmlTempNode->text = IntToString (uniqueIdMap[connection]).c_str ();
				xmlConnectionsConnectionsNode->appendChild (xmlTempNode);

				connection = (*i)->GetOutputPoint (k)->GetNextOutputPoint ();
			}
		}
	}

	// ����� ����������

	for (std::list<ConnectionPoint*>::iterator m = points.begin(); m != points.end(); m++)
	{
		if (xmlBlockNode)
			xmlBlockNode.Release();
		xmlBlockNode = xmlSchemaNode->GetownerDocument()->createElement("Point");
		if (xmlBlockNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point");
		xmlSchemaNode->appendChild (xmlBlockNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("X");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point/X");
		xmlTempNode->text = DoubleToString ((*m)->GetPos ().x).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("Y");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point/Y");
		xmlTempNode->text = DoubleToString ((*m)->GetPos ().y).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("UniqueId");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point/UniqueId");
		xmlTempNode->text = IntToString (uniqueIdMap[(*m)]).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlTempNode)
			xmlTempNode.Release();
		xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("DataType");
		if (xmlTempNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point/DataType");
		xmlTempNode->text = IntToString ((*m)->GetDataType ()).c_str ();
		xmlBlockNode->appendChild (xmlTempNode);

		if (xmlConnectionsConnectionsNode)
			xmlConnectionsConnectionsNode.Release();
		xmlConnectionsConnectionsNode = xmlSchemaNode->GetownerDocument()->createElement("Connections");
		if (xmlConnectionsConnectionsNode.p == NULL)
			throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point/Connections");
		xmlBlockNode->appendChild (xmlConnectionsConnectionsNode);

		connection = (*m)->GetFirstOutputPoint ();
		while (connection)
		{
			if (xmlTempNode)
				xmlTempNode.Release();
			xmlTempNode = xmlSchemaNode->GetownerDocument()->createElement("UniqueId");
			if (xmlTempNode.p == NULL)
				throw Exception ("������ ��� �������� ���� Project/Schemas/Schema/Point/Connections/UniqueId");
			xmlTempNode->text = IntToString (uniqueIdMap[connection]).c_str ();
			xmlConnectionsConnectionsNode->appendChild (xmlTempNode);

			connection = (*m)->GetNextOutputPoint ();
		}
	}
}

void Schema::Load (CComPtr<xml::IXMLDOMNode> xmlSchemaNode)
{
	CComPtr<xml::IXMLDOMNodeList> xmlObjectsNodeList = NULL;
	CComPtr<xml::IXMLDOMNodeList> xmlBlockConnectionPointsNodeList = NULL;
	CComPtr<xml::IXMLDOMNodeList> xmlConnectionsNodeList = NULL;
	CComPtr<xml::IXMLDOMNode> xmlObjectNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlTempNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlBlockConnectionPointNode = NULL;
	CComPtr<xml::IXMLDOMNode> xmlConnectionNode = NULL;
	std::map<int, GraphicalObject*> uniqueIdMap; 
	GraphicalPoint graphicalPoint;
	ConnectionPoint* connectionPoint;
	Block* block;
	std::string name;
	int uniqueId;
	int dataType;

	// �������� ����� ����������

	xmlObjectsNodeList = xmlSchemaNode->selectNodes("./Point");
	if (xmlObjectsNodeList == NULL)
		throw Exception ("���� �� ������ Project/Schemas/Schema/Point");

	for (long j = 0; j < xmlObjectsNodeList->Getlength (); j++)
	{
		xmlObjectNode = xmlObjectsNodeList->Getitem (j);

		xmlTempNode = xmlObjectNode->selectSingleNode("./X");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Point/X");
		graphicalPoint.x = StringToDouble (std::string (xmlTempNode->text));

		xmlTempNode = xmlObjectNode->selectSingleNode("./Y");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Point/Y");
		graphicalPoint.y = StringToDouble (std::string (xmlTempNode->text));

		xmlTempNode = xmlObjectNode->selectSingleNode("./DataType");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Point/DataType");
		dataType = StringToInt (std::string (xmlTempNode->text));

		xmlTempNode = xmlObjectNode->selectSingleNode("./UniqueId");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Point/UniqueId");
		uniqueId = StringToInt (std::string (xmlTempNode->text));

		connectionPoint = new ConnectionPoint(dataType, Orientation::orHoriz, "");
		connectionPoint->SetPos(graphicalPoint);
		uniqueIdMap[uniqueId] = connectionPoint;
		this->points.push_back (connectionPoint);

		xmlConnectionsNodeList = xmlObjectNode->selectNodes("./Connections/UniqueId");
		if (xmlConnectionsNodeList == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Point/Connections/UniqueId");

		for (long k = 0; k < xmlConnectionsNodeList->Getlength (); k++)
		{
			xmlTempNode = xmlConnectionsNodeList->Getitem (k);
			uniqueId = StringToInt (std::string (xmlTempNode->text));
			connectionPoint->Connect ((ConnectionPoint*)uniqueId);
		}
	}

	// �������� ������

	xmlObjectsNodeList = xmlSchemaNode->selectNodes("./Block");
	if (xmlObjectsNodeList == NULL)
		throw Exception ("���� �� ������ Project/Schemas/Schema/Block");

	for (long j = 0; j < xmlObjectsNodeList->Getlength (); j++)
	{
		xmlObjectNode = xmlObjectsNodeList->Getitem (j);

		xmlTempNode = xmlObjectNode->selectSingleNode("./X");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Block/X");
		graphicalPoint.x = StringToDouble (std::string (xmlTempNode->text));

		xmlTempNode = xmlObjectNode->selectSingleNode("./Y");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Y");
		graphicalPoint.y = StringToDouble (std::string (xmlTempNode->text));

		xmlTempNode = xmlObjectNode->selectSingleNode("./Name");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Name");
		name = std::string (xmlTempNode->text);

		xmlTempNode = xmlObjectNode->selectSingleNode("./UniqueId");
		if (xmlTempNode == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Block/UniqueId");
		uniqueId = StringToInt (std::string (xmlTempNode->text));

		if (!BlockFactory::GetBlockFactory ()->SetCurBlock (name))
			throw Exception ("���� " + name + " �� ������ � ���������� ����������� " + BlockFactory::GetBlockFactory ()->GetFilePath ());

		block = BlockFactory::GetBlockFactory ()->GetCurBlock ()->GetCopy ();
		block->SetPos (graphicalPoint);
		uniqueIdMap[uniqueId] = block;
		this->blocks.push_back (block);

		if (block->IsOfType(ObjectType::otInputCommandBlock) || block->IsOfType(ObjectType::otOutputCommandBlock))
		{
			xmlTempNode = xmlObjectNode->selectSingleNode("./Mask");
			if (xmlTempNode == NULL)
				throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Mask");
			((CommandBlock*)block)->SetMask (std::string (xmlTempNode->text));
		}

		xmlBlockConnectionPointsNodeList = xmlObjectNode->selectNodes("./Inputs/UniqueId");
		if (xmlBlockConnectionPointsNodeList == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Inputs/UniqueId");

		for (long k = 0; k < xmlBlockConnectionPointsNodeList->Getlength (); k++)
		{
			xmlBlockConnectionPointNode = xmlBlockConnectionPointsNodeList->Getitem (k);
			uniqueId = StringToInt (std::string (xmlBlockConnectionPointNode->text));

			uniqueIdMap[uniqueId] = block->GetInputPoint(k);
		}

		xmlBlockConnectionPointsNodeList = xmlObjectNode->selectNodes("./Outputs/Output");
		if (xmlBlockConnectionPointsNodeList == NULL)
			throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Outputs/Output");

		for (long k = 0; k < xmlBlockConnectionPointsNodeList->Getlength (); k++)
		{
			xmlBlockConnectionPointNode = xmlBlockConnectionPointsNodeList->Getitem (k);

			xmlTempNode = xmlBlockConnectionPointNode->selectSingleNode("./UniqueId");
			if (xmlTempNode == NULL)
				throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Outputs/Output/UniqueId");
			uniqueId = StringToInt (std::string (xmlTempNode->text));

			uniqueIdMap[uniqueId] = block->GetOutputPoint(k);

			xmlConnectionsNodeList = xmlBlockConnectionPointNode->selectNodes("./Connections/UniqueId");
			if (xmlConnectionsNodeList == NULL)
				throw Exception ("���� �� ������ Project/Schemas/Schema/Block/Outputs/Output/Connections/UniqueId");

			for (long q = 0; q < xmlConnectionsNodeList->Getlength (); q++)
			{
				xmlTempNode = xmlConnectionsNodeList->Getitem (q);
				uniqueId = StringToInt (std::string (xmlTempNode->text));
				block->GetOutputPoint(k)->Connect ((ConnectionPoint*)uniqueId);
			}
		}
	}

	// ������ Id ��������� �������� ��� ����� ����������

	for (std::list<ConnectionPoint*>::iterator m = points.begin(); m != points.end(); m++)
	{
		connectionPoint = (*m)->GetFirstOutputPoint ();
		while ((int)connectionPoint > 0)
		{
			(*m)->SetCurrentOutputPoint((ConnectionPoint*)uniqueIdMap[(int)connectionPoint]);
			connectionPoint = (*m)->GetNextOutputPoint ();
		}
	}

	for (std::list<Block*>::iterator i = blocks.begin(); i != blocks.end(); i++)
	{
		block = (*i);

		for (int j = 0; j < block->GetOutputPointsCount (); j++)
		{
			connectionPoint = block->GetOutputPoint (j)->GetFirstOutputPoint ();
			while ((int)connectionPoint > 0)
			{
				block->GetOutputPoint (j)->SetCurrentOutputPoint((ConnectionPoint*)uniqueIdMap[(int)connectionPoint]);
				connectionPoint = block->GetOutputPoint (j)->GetNextOutputPoint ();
			}
		}
	}
}