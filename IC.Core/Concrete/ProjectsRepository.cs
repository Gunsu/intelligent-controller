using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using IC.Core.Abstract;
using IC.Core.Entities.UI;
using ValidationAspects;
using ValidationAspects.PostSharp;
using System.Runtime.Serialization.Formatters.Binary;

namespace IC.Core.Concrete
{
	[Validate]
	public class ProjectsRepository : IProjectsRepository
	{
		private readonly string _folderPath;

		public Project Load([NotNullOrEmpty] string fileName)
		{
			using (var stream = new FileStream(fileName, FileMode.Open))
			{
				var serializer = new BinaryFormatter();
				//var serializer = new XmlSerializer(typeof (Project));
				var project = (Project) serializer.Deserialize(stream);

				return project;
			}
		}
		
		public Project Create([NotNullOrEmpty] string name)
		{
			var project = new Project();
			project.Name = name;
			project.IsSaved = true;
            project.AddSchema("Schema1");

			Update(project);

			return project;
		}
		
		public void Update([NotNull] Project project)
		{
			project.IsSaved = true;
			foreach(var schema in project.Schemas)
			{
				schema.Save(schema.CurrentUISchema);
			}

			using (var stream = new FileStream(Path.Combine(_folderPath, project.FileName), FileMode.Create))
			{
				var serializer = new BinaryFormatter();
				//var serializer = new XmlSerializer(typeof(Project));
				serializer.Serialize(stream, project);
			}
		}

		public ProjectsRepository([NotNullOrEmpty] string folderPath)
		{
			_folderPath = Path.GetFullPath(folderPath);
		}
	}
}
