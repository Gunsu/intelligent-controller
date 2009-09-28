using System;
using System.Collections.Generic;

using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using Project.Utils.Common;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Core.Processes
{
	/// <summary>
	/// Процессы для работы с проектом.
	/// </summary>
	[Validate]
	public sealed class ProjectProcesses : IProjectProcesses
	{
		public IProject CreateProject([NotNullOrEmpty] string name,
			                          [NotNullOrEmpty] string filePath)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Открывает проект по указанному пути.
		/// </summary>
		/// <param name="path">Путь к файлу проекта.</param>
		/// <returns>Возвращает результат выполнения процесса и открытый проект.</returns>
		public ProcessResult<IProject> Open([NotNullOrEmpty] string path)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Сохраняет проект.
		/// </summary>
		/// <param name="project">Проект.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		public ProcessResult Save([NotNull] IProject project)
		{
			throw new NotImplementedException();
		}
	}
}
