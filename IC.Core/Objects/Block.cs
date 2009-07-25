using IC.CoreInterfaces.Objects;
using System.Collections.Generic;
using ValidationAspects;

namespace IC.Core.Objects
{
	/// <summary>
	/// Блок команды, выполняющий определённое действие, например, проверку на ноль, и, в общем случае, имеющий точки входа и выхода.
	/// </summary>
	public class Block : IBlock
	{
		private Block()
		{
			InputPoints = new List<IBlockConnectionPoint>();
			OutputPoints = new List<IBlockConnectionPoint>();
		}

		/// <remarks>Минимум 2 установлен, потому что 0 и 1 зарезервированы для входного и выходного блоков.</remarks>
		public Block ([Minimum(2)] int id)
		{
			ID = id;
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

		#endregion
	}
}
