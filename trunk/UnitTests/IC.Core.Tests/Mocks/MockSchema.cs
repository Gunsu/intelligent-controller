using System.Xml.Linq;
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

		public bool IsSaved { get; set; }

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

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		public XElement UISchema { get; set; }

		#endregion
	}
}
