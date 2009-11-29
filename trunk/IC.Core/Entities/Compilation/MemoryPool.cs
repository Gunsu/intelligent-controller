namespace IC.Core.Entities
{
	/// <summary>
	/// Пул памяти, в котором находятся скомпилированные схемы.
	/// </summary>
	internal class MemoryPool
	{
		public int Size { get; private set; }
		public int MaxUsedSize { get; private set; }
		public byte[] Memory { get; private set; }

		public MemoryPool(int size)
		{
			Size = size;
			MaxUsedSize = 0;
			Memory = new byte[size];
			this.Free();
		}

		public void Free()
		{
			for(int i = 0; i < Memory.Length; ++i)
			{
				Memory[i] = 0;
			}
		}

		public bool IsFreeByte(int index)
		{
			return (Memory[index] == (char)0);
		}

		public void AllocateByte(int index)
		{
			if (MaxUsedSize <= index)
				MaxUsedSize = index + 1;

			Memory[index] = 1;
		}

		public void FreeByte(int index)
		{
			Memory[index] = 0;
		}
	}
}
