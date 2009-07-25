namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Маска команды.
	/// </summary>
	public interface ICommandMask
	{
		/// <summary>
		/// Размер маски в байтах.
		/// </summary>
		uint Size { get; }

		/// <summary>
		/// Значение маски.
		/// </summary>
		string Value { get; set; }
	}
}
