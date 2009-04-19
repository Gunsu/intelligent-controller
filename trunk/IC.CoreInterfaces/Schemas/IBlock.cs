using IC.Utils.Common;

namespace IC.CoreInterfaces.Schemas
{
	/// <summary>
	/// Блок команды, выполняющий определённое действие, например, проверку на ноль, и, в общем случае, имеющий точки входа и выхода.
	/// </summary>
	public interface IBlock
	{
		/// <summary>
		/// Координаты блока.
		/// </summary>
		Coordinates Coordinates { get; }
	}
}
