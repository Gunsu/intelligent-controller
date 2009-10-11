using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
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
			FileStream stream;
			stream = new FileStream(Path.Combine(_folderPath, name + ".prj"), FileMode.Open);
			var serializer = new XmlSerializer(typeof (Project));
			var project = (Project) serializer.Deserialize(stream);
			stream.Dispose();
			
			return project;
		}
		
		public Project Create(string name)
		{
			var project = new Project();
			project.Name = name;
			project.FilePath = Path.Combine(_folderPath, name + ".prj");

			project.AddSchema("Schema1");
			project.IsSaved = true;

			FileStream stream;
			stream = new FileStream(project.FilePath, FileMode.Create);
			//var serializer = new BinaryFormatter();
			var serializer = new XmlSerializer(typeof (Project));
            serializer.Serialize(stream, project);
			stream.Dispose();

			return project;
		}
		
		public void Update(Project project)
		{
			project.IsSaved = true;

			FileStream stream;
			stream = new FileStream(Path.Combine(_folderPath,project.Name + ".prj"), FileMode.Create);
			var serializer = new XmlSerializer(typeof(Project));
			serializer.Serialize(stream, project);
			stream.Dispose();

			////Сериализуем объект и сохраняем в файл
			//FileStream stream;
			//try
			//{
			//    stream = new FileStream(project.FilePath, FileMode.Create);
			//}
			//catch (Exception ex)
			//{
			//    throw new InvalidOperationException("Невозможно получить доступ к файлу проекта.", ex);
			//}

			//try
			//{
			//    var serializer = new BinaryFormatter();
			//    serializer.Serialize(stream, this);
			//}
			//catch (Exception ex)
			//{
			//    throw new InvalidOperationException(
			//        string.Format("Не удалось сериализовать или сохранить сериализованный объект в файл.\r\n" +
			//                      "Детали: {0}, StackTrace: {1}",
			//                      ex.Message, ex.StackTrace));
			//}
			//finally
			//{
			//    stream.Dispose();
			//}
		}

		public ProjectsRepository([NotNullOrEmpty] string folderPath)
		{
			_folderPath = Path.GetFullPath(folderPath);
		}
	}
}
