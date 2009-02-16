#ifndef BLOCKS_H
#define BLOCKS_H

#include "GraphicalCanvas.h"
#include <list>
#include <vector>
#include <string>
#include "Common.h"

enum ObjectType
{
	otBlock = 0,
	otCommandBlock,
	otInputCommandBlock,
	otOutputCommandBlock,
	otOutputCommandBufBlock,
	otOutputCommandConstBlock,
	otConnectionPoint,
	otBlockInputPoint,
	otBlockOutputPoint
};

/*******************************
*       GraphicalObject
********************************/

class GraphicalObject
{
	protected:
		GraphicalPoint pos;
		bool selected;
		std::string description;

	public:
		GraphicalObject () : selected (false) {};
		GraphicalObject (GraphicalObject* graphicalObject);
		virtual void SetSelected (bool selected);
		bool GetSelected ();
		virtual void SetPos (GraphicalPoint& pos);
		virtual GraphicalPoint GetPos ();
		virtual bool TestIntersect (GraphicalPoint& pos) = 0;
		virtual void Draw (GraphicalCanvas* canvas) = 0;
		virtual bool CanMove () = 0;
		virtual ObjectType GetType () = 0;
		virtual bool IsOfType (ObjectType type) = 0;
};

/*******************************
*       ConnectionPoint
********************************/

enum Orientation
{
	orHoriz = 0,
	orVert
};

class ConnectionPoint : public GraphicalObject
{
	public:
		struct CompileData
		{
			int variableIndex;
			bool processed;
		} compileData;

		void SetCompileProcessedRecursive ();				// каскадно отмечает все дочерние точки как обработанные
		void SetCompileVariableRecursive (int varIndex);	// каскадно вешает индекс переменной на все дочерние точки

	protected:
		static const double R;
		std::list<ConnectionPoint*> outputs;
		std::list<ConnectionPoint*>::iterator outputIterator;
		int dataType;
		Orientation orientation;
		bool signaledState;
		std::string name;
		virtual void SetSignaled (bool signaled);

	public:
		static double GetR () {return R;}

		ConnectionPoint (int dataType, Orientation orientation, std::string name);
		ConnectionPoint (ConnectionPoint* connectionPoint);
		void Connect (ConnectionPoint* connectionPoint);
		void Disconnect (ConnectionPoint* connectionPoint);
		void DisconnectAll ();
		ConnectionPoint* GetFirstOutputPoint ();
		ConnectionPoint* GetNextOutputPoint ();
		void SetCurrentOutputPoint (ConnectionPoint* connectionPoint);
		int GetOutputPointsCount ();
		int GetDataType ();
		void SetDataType (int dataType);
		virtual std::string GetName ();
		void OnOutputSetSelected (ConnectionPoint* output, bool selected);
		virtual bool TestIntersect (GraphicalPoint& pos);
		virtual void Draw (GraphicalCanvas* canvas);
		virtual bool CanMove ();
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
		virtual void SetSelected (bool selected);
};

/*******************************
*      BlockConnectionPoint
********************************/

class Block;

class BlockConnectionPoint : public ConnectionPoint
{
	protected:
		Block* block;
		
	public:
		BlockConnectionPoint (Block* block, std::string& name, GraphicalPoint &pos, Orientation orientation, int dataType);
		BlockConnectionPoint (BlockConnectionPoint * blockConnectionPoint);
		Block* GetBlock ();
		void SetBlock (Block* block);
		void DisconnectAll ();
		virtual GraphicalPoint GetPos ();
		virtual bool CanMove ();
};

/*******************************
*        BlockInputPoint
********************************/

class BlockInputPoint : public BlockConnectionPoint
{
	public:
		BlockInputPoint (Block* block, std::string& name, GraphicalPoint &pos, Orientation orientation, int dataType) : BlockConnectionPoint (block, name, pos, orientation, dataType) {}
		BlockInputPoint (BlockInputPoint* inputPoint);
		virtual void Draw (GraphicalCanvas* canvas);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
};

/*******************************
*        BlockOutputPoint
********************************/

class BlockOutputPoint : public BlockConnectionPoint
{
	public:
		BlockOutputPoint (Block* block, std::string& name, GraphicalPoint &pos, Orientation orientation, int dataType) : BlockConnectionPoint (block, name, pos, orientation, dataType) {}
		BlockOutputPoint (BlockOutputPoint* outputPoint);
		virtual void Draw (GraphicalCanvas* canvas);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
};

/*******************************
*             Block
********************************/

class Block : public GraphicalObject
{
	public:
		struct CompileData
		{
			int order;
			bool processed;
		} compileData;

	protected:
		std::vector<BlockInputPoint*> inputPoints;
		std::vector<BlockOutputPoint*> outputPoints;
		std::string name;
		GraphicalPoint size;
		int id;

		virtual bool ShowNameAllowed () {return true;}

	public:
		Block (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints, int id);
		Block (Block* block);
		~Block ();
		int GetInputPointsCount ();
		int GetOutputPointsCount ();
		BlockInputPoint* GetInputPoint (int index);
		BlockOutputPoint* GetOutputPoint (int index);
		std::string GetName ();
		int getId () {return id;}
		virtual Block* GetCopy ();
		virtual void SetPos (GraphicalPoint& pos);
		virtual bool TestIntersect (GraphicalPoint& pos);
		virtual void Draw (GraphicalCanvas* canvas);
		virtual bool CanMove ();
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
};

/*******************************
*         CommandBlock
********************************/

class CommandBlock : public Block
{
	protected:	
		std::string mask;

		virtual bool ShowNameAllowed () {return false;}

	public:
		CommandBlock (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints);
		CommandBlock (CommandBlock* block);
		std::string GetMask () {return mask;}
		virtual void SetMask (std::string mask);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
		virtual Block* GetCopy ();
		virtual void Draw (GraphicalCanvas* canvas);
};

/*******************************
*       InputCommandBlock
********************************/

class InputCommandBlock : public CommandBlock
{
	public:
		InputCommandBlock (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints) : CommandBlock (size, name, inputPoints, outputPoints) {}
		InputCommandBlock (InputCommandBlock* block): CommandBlock (block) {}
		virtual void SetMask (std::string mask);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
		virtual Block* GetCopy ();
		virtual void Draw (GraphicalCanvas* canvas);
};

/*******************************
*      OutputCommandBlock
********************************/

class OutputCommandBlock : public CommandBlock
{
	protected:
		bool inputConnected;

	public:
		OutputCommandBlock (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints) : CommandBlock (size, name, inputPoints, outputPoints), inputConnected (false) {}
		OutputCommandBlock (OutputCommandBlock* block) : CommandBlock (block), inputConnected (false) {}
		void SetInputConnected (bool connected);
		virtual void SetMask (std::string mask);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
		virtual Block* GetCopy ();
		virtual void Draw (GraphicalCanvas* canvas);
};

/*******************************
*     OutputCommandBufBlock
********************************/

class OutputCommandBufBlock : public OutputCommandBlock
{
	protected:
		virtual bool ShowNameAllowed () { return true; }

	public:
		OutputCommandBufBlock (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints) : OutputCommandBlock (size, name, inputPoints, outputPoints) {}
		OutputCommandBufBlock (OutputCommandBufBlock* block) : OutputCommandBlock (block) {}
		virtual void SetMask (std::string mask);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
		virtual Block* GetCopy ();
		virtual void Draw (GraphicalCanvas* canvas);
};

/*******************************
*     OutputCommandConstBlock
********************************/

class OutputCommandConstBlock : public OutputCommandBlock
{
	public:
		OutputCommandConstBlock (GraphicalPoint& size, std::string name, std::vector<BlockInputPoint*> *inputPoints, std::vector<BlockOutputPoint*> *outputPoints) : OutputCommandBlock (size, name, inputPoints, outputPoints) {}
		OutputCommandConstBlock (OutputCommandConstBlock* block) : OutputCommandBlock (block) {}
		virtual void SetMask (std::string mask);
		virtual ObjectType GetType ();
		virtual bool IsOfType (ObjectType type);
		virtual Block* GetCopy ();
		virtual void Draw (GraphicalCanvas* canvas);
};

#endif