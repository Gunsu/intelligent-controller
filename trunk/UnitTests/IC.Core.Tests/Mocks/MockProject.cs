using System;
using IC.CoreInterfaces.Objects;
using System.Collections.Generic;

namespace IC.Core.Tests.Mocks
{
	[Serializable]
	public sealed class MockProject : IProject
	{
		public IList<ISchema> Schemas { get; set; }

		public bool IsSaved { get; set; }

		public string Path { get; set; }
	}
}
