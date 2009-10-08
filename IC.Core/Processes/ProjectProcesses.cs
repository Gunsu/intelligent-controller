using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
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
		public IProject Create([NotNullOrEmpty] string name,
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
		/// Сохраняет проект, если сохранены все схемы.
		/// </summary>
		/// <param name="project">Проект.</param>
		/// <returns>Если сохранены не все схемы, то возвращает список несохранённых схем.</returns>
		public ProcessResult<List<ISchema>> Save([NotNull] IProject project)
		{
			//Проверяем все ли схемы сохранены.
			var notSavedSchemas = new List<ISchema>();
			foreach (ISchema schema in project.Schemas)
			{
				if (!schema.IsSaved)
				{
					notSavedSchemas.Add(schema);
				}
			}
			//Если не все, то сообщаем об этом и завершаем метод
			if (notSavedSchemas.Count != 0)
			{
				return new ProcessResult<List<ISchema>>()
				       	{
				       		ErrorMessage = "Не все схемы сохранены в проекте.",
				       		NoErrors = false,
				       		Result = notSavedSchemas
				       	};
			}
			//Сериализуем объект и сохраняем в файл
			MemoryStream stream;
			try
			{
				stream = new MemoryStream();
			}
			catch (Exception)
			{
				return new ProcessResult<List<ISchema>>()
				       	{
				       		ErrorMessage = "Невозможно получить доступ к файлу проекта.",
				       		NoErrors = false
				       	};
			}

			try
			{
				var serializer = new BinaryFormatter();
				serializer.Serialize(stream, project);
			}
			catch (Exception ex)
			{
				return new ProcessResult<List<ISchema>>()
				       	{
				       		ErrorMessage = string.Format("Не удалось сериализовать или сохранить сериализованный объект в файл.\r\n" +
				       		                             "Детали: {0}, StackTrace: {1}",
				       		                             ex.Message, ex.StackTrace),
				       		NoErrors = false
				       	};
			}
			finally
			{
				stream.Dispose();
			}

			return new ProcessResult<List<ISchema>>() {NoErrors = true};
		}

		/// <summary>
		/// Добавляет схему к проекту.
		/// </summary>
		/// <param name="project">Проект.</param>
		/// <param name="schema">Добавляемая схема.</param>
		/// <returns>True, в случае успешного добавления.</returns>
		public bool AddSchema([NotNull] IProject project, [NotNull] ISchema schema)
		{
			throw new NotImplementedException();
		}
	}
}
