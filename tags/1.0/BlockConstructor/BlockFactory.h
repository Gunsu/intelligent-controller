#ifndef BLOCK_FACORY_H
#define BLOCK_FACORY_H

#include "Blocks.h"
#include "Exceptions.h"
#include <vector>
#include <string>

/*******************************
*          BlockFactory
********************************/

class BlockFactory
{
	protected:
		static BlockFactory blockFactory;

	public:
		static BlockFactory* GetBlockFactory () {return &blockFactory;}

	protected:
		std::vector<Block*> blocks;
		std::vector<Block*>::iterator curBlockIter;
		std::string filePath;

		std::vector<Block*>::iterator FindBlock (std::string name);

	public:
		BlockFactory ();
		~BlockFactory ();
		void Load (std::string filePath);
		bool SetCurBlock (std::string name);
		Block* GetCurBlock ();
		void SetCurBlockToFirst ();
		void SetCurBlockToNext ();
		void Clear ();
		std::string GetFilePath ();
};

#endif