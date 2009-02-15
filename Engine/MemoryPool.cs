using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class MemoryPool
	{
		private int size;
		private int maxUsedSize;
		private char[] memory;

		public MemoryPool(int memoryPoolSize)
		{
			throw new System.NotImplementedException();
		}

		public int Size
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int MaxUsedSize
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public void Free()
		{
			throw new System.NotImplementedException();
		}

		public void FreeByte(int index)
		{
			throw new System.NotImplementedException();
		}

		public void AllocateByte(int index)
		{
			throw new System.NotImplementedException();
		}

		public bool IsFreeByte(int index)
		{
			throw new System.NotImplementedException();
		}
	}
}
