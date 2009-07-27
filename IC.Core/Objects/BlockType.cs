using IC.CoreInterfaces.Objects;
using System.Collections.Generic;
using ValidationAspects;

namespace IC.Core.Objects
{
	/// <summary>
	/// Блок команды, выполняющий определённое действие, например, проверку на ноль, и, в общем случае, имеющий точки входа и выхода.
	/// </summary>
	public class BlockType : IBlockType
	{
		private BlockType()
		{
			InputPoints = new List<IBlockConnectionPoint>();
			OutputPoints = new List<IBlockConnectionPoint>();
		}

		/// <remarks>Минимум 2 установлен, потому что 0 и 1 зарезервированы для входного и выходного блоков.</remarks>
		public BlockType ([Minimum(2)] int id, [NotNullOrEmpty] string name)
			: this()
		{
			ID = id;
			Name = name;
		}

		#region IBlock Members

		/// <summary>
		/// Входные точки.
		/// </summary>
		public IList<IBlockConnectionPoint> InputPoints { get; private set; }

		/// <summary>
		/// Выходные точки.
		/// </summary>
		public IList<IBlockConnectionPoint> OutputPoints { get; private set; }

		public int ID { get; private set; }

		public string Name { get; private set; }

		#endregion
	}
}
