namespace Project.Utils.Common
{
	/// <summary>
	/// Координаты.
	/// </summary>
	public struct Coordinates
	{
		/// <summary>
		/// Координата по X.
		/// </summary>
		public double X;

		/// <summary>
		/// Координата по Y.
		/// </summary>
		public double Y;

		/// <summary>
		/// Инициализирует структуру.
		/// </summary>
		/// <param name="x">Координата по X.</param>
		/// <param name="y">Координата по Y.</param>
		public Coordinates(double x, double y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return string.Format("({0};{1})", X, Y);
		}
	}
}
