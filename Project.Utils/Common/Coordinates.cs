namespace Project.Utils.Common
{
	/// <summary>
	/// Координаты.
	/// </summary>
	public struct Coordinates
	{
		//private double _x;
		//private double _y;

		/// <summary>
		/// Координата по X.
		/// </summary>
		public double X;
		//{
		//    get { return _x;}
		//    set { _x = value; }
		//}

		public double GetX { get { return X; } set { X = value; } }

		/// <summary>
		/// Координата по Y.
		/// </summary>
		public double Y;

		public double GetY { get { return Y; } set { Y = value; } }
		//{
		//    get { return _y; }
		//    set { _y = value; }
		//}

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
