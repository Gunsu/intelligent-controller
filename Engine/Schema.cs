using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class Schema
	{
		private List<Block> blocks;
		private List<ConnectionPoint> points;
		private Point addToSelected;
		private Point LBDownPos;
		private GraphicalObject SelectedObject;
		private ConnectionPoint firstDrawPoint;
		private string name;
		private List<Variable> variables;
		private MemoryPool memoryPool;

		public Schema(string name)
		{
			throw new System.NotImplementedException();
		}
	
		public Tool Tool
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public State State
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int ObjectSelectedCallback
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int ScrollCallback
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public string Name
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		private GraphicalObject TestIntersect(Point pos)
		{
			throw new System.NotImplementedException();
		}

		private ConnectionPoint FindParentPoint(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		private void CascadeDeleteConnectionPoint(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		private void onObjectSetSelected(GraphicalObject graphicalObject, bool selected)
		{
			throw new System.NotImplementedException();
		}

		private void FindBlockWithoutInputs(ObjectType objectType)
		{
			throw new System.NotImplementedException();
		}

		private void ResetCompileData()
		{
			throw new System.NotImplementedException();
		}

		private Block FindBlockWithProcessedInputs()
		{
			throw new System.NotImplementedException();
		}

		private OutputCommandBlock FindFirstOutputCommandBlock()
		{
			throw new System.NotImplementedException();
		}

		private void GetEndBlocksRecursive(ConnectionPoint connectionPoint, List<Block> endBlocks)
		{
			throw new System.NotImplementedException();
		}

		private Block GetOldestBlockOrderRecursive(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		private Variable AddVariable(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		private int AllocateMemoryForVariable(int varSize)
		{
			throw new System.NotImplementedException();
		}

		private void GarbageCollect(int lifeTime)
		{
			throw new System.NotImplementedException();
		}

		public void Validate()
		{
			throw new System.NotImplementedException();
		}

		public void Compile(RomData romData, MemoryPool memoryPool, int pos)
		{
			throw new System.NotImplementedException();
		}

		public string GetCommandMask()
		{
			throw new System.NotImplementedException();
		}

		public void Draw()
		{
			throw new System.NotImplementedException();
		}

		public void OnMouseMove(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public void OnLBDown(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public void OnLBUp(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public void OnRBDown(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public void OnRBUp(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public void OnDelKeyPressed()
		{
			throw new System.NotImplementedException();
		}

		public void OnKeyPressed(char key)
		{
			throw new System.NotImplementedException();
		}

		public void Save(int schemaXML)
		{
			throw new System.NotImplementedException();
		}

		public void Load(int schemaXML)
		{
			throw new System.NotImplementedException();
		}
	}
}
