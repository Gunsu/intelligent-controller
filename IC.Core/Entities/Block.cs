using System;
using System.Collections.Generic;
using IC.Core.Enums;
using ValidationAspects.PostSharp;

namespace IC.Core.Entities
{
	[Validate]
	public class Block
	{
		#region Fields and properties

		public BlockType BlockType { get; private set; }
		public IList<BlockConnectionPoint> InputPoints { get; private set; }
		public IList<BlockConnectionPoint> OutputPoints { get; private set; }
		public int X { get; set; }
		public int Y { get; set; }

		#endregion


		#region Constructors

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
