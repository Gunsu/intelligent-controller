using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IC.Core.Enums;
using ValidationAspects.PostSharp;
using ValidationAspects;

namespace IC.Core.Entities
{
	[Validate]
	public class Block
	{
		#region Fields and properties

		internal int Order { get; set; }
		internal bool Processed { get; set; }

		public BlockType BlockType { get; set; }
		public List<BlockConnectionPoint> InputPoints{ get; set; }
		public List<BlockConnectionPoint> OutputPoints { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
        public ObjectType ObjectType { get; set; }
		
		#endregion


		#region Constructors

		public Block()
		{
			Order = -1;
			Processed = false;
			ObjectType = IC.Core.Enums.ObjectType.Block;
			InputPoints = new List<BlockConnectionPoint>();
			OutputPoints = new List<BlockConnectionPoint>();
		}

		public Block(BlockType blockType)
			: this()
		{
			BlockType = blockType;
		}

		#endregion


		#region Methods

        public BlockConnectionPoint AddInputPoint([NotNull] BlockConnectionPoint point)
        {
            InputPoints.Add(point);
            point.Block = this;
            return point;
        }

        public BlockConnectionPoint AddOutputPoint([NotNull] BlockConnectionPoint point)
        {
            OutputPoints.Add(point);
            point.Block = this;
            return point;
        }

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
