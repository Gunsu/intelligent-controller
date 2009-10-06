using System;
using System.Xml.Linq;
using IC.CoreInterfaces.Objects;

namespace IC.Core.Tests.Mocks
{
	[Serializable]
	public sealed class MockSchema : ISchema
	{
		public IBlocks Blocks{ get; set; }

		public bool IsSaved { get; set; }

		public string Name { get; set; }

		[NonSerialized]
		private XElement _uiSchema;

		public XElement UISchema
		{
			get { return _uiSchema; }
			set { _uiSchema = value; }
		}

		public string FilePath { get; set; }
	}
}
