using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ValidationAspects.PostSharp;

namespace IC.Core.Entities.UI
{
	/// <summary>
	/// Точка блока для присоединения других блоков.
	/// </summary>
	[Serializable]
	[Validate]
	public class ConnectionPoint
	{
		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание о предназначении данной точки.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Блок, к которому подсоединена данная связь.
		/// </summary>
		public Block Block { get; set; }

		/// <summary>
		/// Размер данной связи. Если null, то не имеет значения.
		/// </summary>
		public int? Size { get; set; }

		private List<Block> _outputs;

		/// <summary>
		/// Блоки, к которым присоединена данная связь.
		/// </summary>
		public ReadOnlyCollection<Block> Outputs
		{
			get { return _outputs.AsReadOnly(); }
		}

		public void AddOutput(Block block)
		{
			_outputs.Add(block);
		}

		public void RemoveOutput(Block block)
		{
			_outputs.Remove(block);
		}

		public ConnectionPoint()
		{
			_outputs = new List<Block>();
		}
	}
}
