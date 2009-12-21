using IC.Core.Abstract;
using IC.Core.Entities.UI;
using IC.UI.Infrastructure.Interfaces.Windows;
using Moq;

namespace IC.PresentationModels.Tests.Mocks
{
	/// <summary>
	/// Класс для создания простых stub-ов объектов для использования в тестах.
	/// </summary>
	public static class Stubs
	{
		#region Windows

		public static ICreateProjectWindow CreateProjectWindow
		{
			get { return new Mock<ICreateProjectWindow>().Object; }
		}

		public static ICreateSchemaWindow CreateSchemaWindow
		{
			get { return new Mock<ICreateSchemaWindow>().Object; }
		}

		#endregion

		#region Processes

		public static IBlockTypesRepository BlockTypesRepository
		{
			get { return new Mock<IBlockTypesRepository>().Object; }
		}

		public static IProjectsRepository ProjectsRepository
		{
			get { return new Mock<IProjectsRepository>().Object; }
		}

		#endregion

		#region Objects

		public static Schema Schema
		{
			get { return new Mock<Schema>().Object; }
		}

		public static Project Project
		{
			get { return new Mock<Project>().Object; }
		}

		#endregion
	}
}
