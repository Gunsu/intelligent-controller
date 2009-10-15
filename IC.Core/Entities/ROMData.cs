using System;

namespace IC.Core.Entities
{
	internal class ROMData
	{
		public int Size { get; private set; }
		public int UsedSize { get; private set; }
		public byte[] Data { get; private set; }
		public byte this[int index]
		{
			get { return Data[index]; }
			set { Data[index] = value; }
		}

		public ROMData(int size)
		{
			UsedSize = 0;
			Size = size;
			Data = new byte[size];
		}

		public void SaveToBin(string filePath)
		{
			throw new NotImplementedException();
		}
	}
}
