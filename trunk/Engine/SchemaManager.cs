using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class SchemaManager
	{
		private List<Schema> schemas;

		public Schema CurSchema
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int ObjectSelectedCallback
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int ScrollCallback
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public bool AddSchema(Schema schema)
		{
			throw new System.NotImplementedException();
		}

		public bool DelCurSchema()
		{
			throw new System.NotImplementedException();
		}

		public bool RenameCurSchema(string name)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public void SetCurSchemaToFirst()
		{
			throw new System.NotImplementedException();
		}

		public void SetCurSchemaToNext()
		{
			throw new System.NotImplementedException();
		}

		public void Validate()
		{
			throw new System.NotImplementedException();
		}

		public void Compile(string romFilePath)
		{
			throw new System.NotImplementedException();
		}

		public void Save(int xmlSchema)
		{
			throw new System.NotImplementedException();
		}

		public void Load(int xmlSchema)
		{
			throw new System.NotImplementedException();
		}
	}
}
