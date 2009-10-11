using System;
using System.Collections.Generic;
using ValidationAspects.PostSharp;

namespace IC.Core.Entities
{
	[Validate]
	[Serializable]
	public class Block
	{
		#region Fields and properties

		public BlockType BlockType { get; set; }
		public List<BlockConnectionPoint> InputPoints { get; set; }
		public List<BlockConnectionPoint> OutputPoints { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		#endregion


		#region Constructors

		public Block()
		{}

		public Block(BlockType blockType)
		{
			BlockType = blockType;
			InputPoints = new List<BlockConnectionPoint>();
			OutputPoints = new List<BlockConnectionPoint>();
		}

		#endregion


		#region Methods

		/// <summary>
		/// Проверяет блок на висячие входы.
		/// </summary>
		/// <returns>Возвращает true, если блок имеет один или больше висячих входов.</returns>
		public bool BlockHasHangingInput()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
