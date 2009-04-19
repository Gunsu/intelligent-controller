using IC.CoreInterfaces.Schemas;

namespace IC.Core.Schemas
{
	/// <summary>
	/// Маска команды.
	/// </summary>
	public class CommandMask : ICommandMask
	{
		private string _value;

		/// <summary>
		/// Размер маски в байтах.
		/// </summary>
		public uint Size { get; private set; }

		/// <summary>
		/// Значение маски.
		/// </summary>
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				Size = (uint)_value.Length;
				_value = value;
			}
		}
	}
}
