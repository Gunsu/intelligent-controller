#ifndef BLOCK_MANAGER_H
#define BLOCK_MANAGER_H

#include "Blocks.h"
#include "BlockFactory.h"
#include "Common.h"
#include "CompileHelper.h"
#include <list>
#include <map>

/*******************************
*            Schema
********************************/

typedef void ((ObjectSelectedCallback)(GraphicalObject* object));
typedef void ((ScrollCallback)(GraphicalPoint& scroll));

class Schema
{
	public:
		enum Tool
		{
			addBlock = 0,
			addPoint
		};

	protected:
		enum State
		{
			doNothing = 0,
			moveSelected,
			moveSchema
		};

		std::list<Block*> blocks;
		std::list<ConnectionPoint*> points;
		GraphicalCanvas* canvas;
		Tool tool;
		State state;
		GraphicalPoint addToSelected;
		GraphicalPoint LBDownPos;
		GraphicalObject* selectedObject;
		ConnectionPoint* firstDrawPoint;
		GraphicalObject* TestIntersect (GraphicalPoint &pos);
		ConnectionPoint* FindParentPoint (ConnectionPoint* connectionPoint);
		std::list<ConnectionPoint*>::iterator FindPointIterator (ConnectionPoint* connectionPoint);
		std::list<Block*>::iterator FindBlockIterator (Block* block);
		void CascadeDeleteConnectionPoint (ConnectionPoint* point);
		void OnObjectSetSelected (GraphicalObject* object, bool selected);
		Block* FindBlockWithoutInputs (ObjectType objectType);
		std::string name;
		ObjectSelectedCallback* objectSelectedCallback;
		ScrollCallback* scrollCallback;

	protected:
		// структура представл€юща€ переменную в пуле пам€ти
		struct Variable
		{
			int address;	// адрес переменной в пуле пам€ти
			int size;		// размер переменной в байтах
			int lifeTime;	// врем€ жизни переменной. ‘актически это пор€дковый номер блока, после обработки которого переменна€ становитс€ не нужна
			bool active;
		};

		// модификатор выходного параметра. —оответствует типу выходного блока. «аписываетс€ в ѕ«”
		enum OutParamModifier
		{
			outVar		= 0,
			outConst	= 1,
			outBuf		= 2
		};

		MemoryPool *memoryPool;
		std::vector <Variable> variables;
		void ResetCompileData ();
		int GetBlocksCount ();
		Block* FindBlockWithProcessedInputs ();
		OutputCommandBlock* FindFirstOutputCommandBlock ();
		void GetEndBlocksRecursive (ConnectionPoint* point, std::vector<Block*> *endBlocks);
		Block* GetOldestBlockOrderRecursive (ConnectionPoint* point);
		Variable AddVariable (ConnectionPoint* point);
		int AllocateMemoryForVariable (int varSize);
		void GarbageCollect (int lifeTime);

	public:
		void Validate ();
		void Compile (RomData &romData, MemoryPool *memoryPool, int *pos);
		std::string GetCommandMask ();

	public:
		Schema (GraphicalCanvas* canvas, std::string name);
		~Schema ();
		std::string GetName ();
		void SetName (std::string name);
		void Draw ();
		void SetTool (Tool tool);
		Tool GetTool ();
		void OnMouseMove (GraphicalPoint &pos);
		void OnLBDown (GraphicalPoint &pos);
		void OnLBUp (GraphicalPoint &pos);
		void OnRBDown (GraphicalPoint &pos);
		void OnRBUp (GraphicalPoint &pos);
		void OnDelKeyPressed ();
		void OnKeyPressed (char key);
		void SetObjectSelectedCallback (ObjectSelectedCallback* callback);
		void SetScrollCallback (ScrollCallback* callback);
		void Save (CComPtr<xml::IXMLDOMNode> xmlSchemaNode);
		void Load (CComPtr<xml::IXMLDOMNode> xmlSchemaNode);
};

#endif