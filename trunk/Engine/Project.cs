using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class Project
	{
		private string projectFilePath;
		private string blockFactoryFile;
		private bool isLoad;
		public event EventHandler ProjectClosedCallback;

		public event EventHandler ProjectOpenedCallback;

		public bool IsLoad
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public void Load(string projectFilePath, string componentsFolderPath)
		{
			throw new System.NotImplementedException();
		}

		public void Save()
		{
			throw new System.NotImplementedException();
		}

		public void SaveAs(string projectFilePath)
		{
			throw new System.NotImplementedException();
		}

		public void New(string projectFilePath, string blockFactoryFilePath, string blockFactoryFile)
		{
			throw new System.NotImplementedException();
		}
	}
}
