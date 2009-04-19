using IC.CoreInterfaces.Schemas;
using IC.Utils.Common;

namespace IC.Core.Schemas
{
	/// <summary>
	/// Блок команды, выполняющий определённое действие, например, проверку на ноль, и, в общем случае, имеющий точки входа и выхода.
	/// </summary>
	public class Block : IBlock
	{
		/// <summary>
		/// Координаты блока.
		/// </summary>
		public Coordinates Coordinates { get; private set; }

		private Block()
		{
			Coordinates = new Coordinates();
		}
	}
}
