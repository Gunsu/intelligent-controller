using System;
using System.Collections.Generic;

using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;

namespace IC.Core.Processes
{
	/// <summary>
	/// Процессы для работы с проектом.
	/// </summary>
	public sealed class ProjectProcesses : IProjectProcesses
	{
		public bool Validate(out Dictionary<ISchema, string> errorsToSchemaMap)
		{
			throw new NotImplementedException();
		}

		public bool Compile(out Dictionary<ISchema, string> errorsToSchemaMap)
		{
			throw new NotImplementedException();
		}
	}
}
