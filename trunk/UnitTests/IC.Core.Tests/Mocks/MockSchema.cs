using IC.CoreInterfaces.Objects;
namespace IC.Core.Tests.Mocks
{
	public sealed class MockSchema : ISchema
	{
		#region ISchema members

		public IBlocks Blocks
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsSaved
		{
			get { throw new System.NotImplementedException(); }
		}

		public string Name
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		#endregion
	}
}
