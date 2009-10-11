namespace IC.Core.Entities
{
	/// <summary>
	/// Маска команды.
	/// </summary>
	public class CommandMask
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
