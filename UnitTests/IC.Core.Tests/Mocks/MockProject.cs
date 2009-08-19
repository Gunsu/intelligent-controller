using IC.CoreInterfaces.Objects;
using System.Collections.Generic;

namespace IC.Core.Tests.Mocks
{
	public sealed class MockProject : IProject
	{
		#region IProject members

		public IList<ISchema> Schemas
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsSaved
		{
			get { throw new System.NotImplementedException(); }
		}

		#endregion
	}
}
