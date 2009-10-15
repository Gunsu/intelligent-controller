namespace IC.Core.Entities
{
	/// <summary>
	/// Структура, представляющая переменную в пуле памяти
	/// </summary>
	internal class MemoryPoolVariable
	{
		/// <summary>
		/// Алрес переменной в пуле памяти.
		/// </summary>
		public int Address { get; set; }

		/// <summary>
		/// Размер переменной в байтах.
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// Время жизни переменной.
		/// Фактически это порядковый номер блока, после обработки которого переменная становится не нужна.
		/// </summary>
		public int LifeTime { get; set; }
		public bool Active { get; set; }
	}
}
