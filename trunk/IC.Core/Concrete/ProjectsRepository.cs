using System;
using System.Collections.Generic;
using System.IO;
using IC.Core.Abstract;
using IC.Core.Entities;
using ValidationAspects;
using ValidationAspects.PostSharp;
using System.Runtime.Serialization.Formatters.Binary;

namespace IC.Core.Concrete
{
	[Validate]
	public class ProjectsRepository : IProjectsRepository
	{
		private readonly string _folderPath;

		public Project Load(string name)
		{
			throw new System.NotImplementedException();
		}
		
		public Project Create(string name)
		{
			throw new System.NotImplementedException();
		}
		
		public void Update(Project project)
		{
			////Проверяем все ли схемы сохранены.
			//var notSavedSchemas = new List<Schema>();
			//foreach (Schema schema in project.Schemas)
			//{
			//    if (!schema.IsSaved)
			//    {
			//        notSavedSchemas.Add(schema);
			//    }
			//}
			////Если не все, то сообщаем об этом и завершаем метод
			//if (notSavedSchemas.Count != 0)
			//{
			//    return new ProcessResult<List<ISchema>>()
			//    {
			//        ErrorMessage = "Не все схемы сохранены в проекте.",
			//        NoErrors = false,
			//        Result = notSavedSchemas
			//    };
			//}
			//Сериализуем объект и сохраняем в файл
			FileStream stream;
			try
			{
				stream = new FileStream(project.Path, FileMode.Create);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Невозможно получить доступ к файлу проекта.", ex);
			}

			try
			{
				var serializer = new BinaryFormatter();
				serializer.Serialize(stream, this);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(
					string.Format("Не удалось сериализовать или сохранить сериализованный объект в файл.\r\n" +
					              "Детали: {0}, StackTrace: {1}",
					              ex.Message, ex.StackTrace));
			}
			finally
			{
				stream.Dispose();
			}
		}

		public ProjectsRepository([NotNullOrEmpty] string folderPath)
		{
			_folderPath = folderPath;
		}
	}
}
