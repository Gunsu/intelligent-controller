using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
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

		public static IProjectProcesses ProjectProcesses
		{
			get { return new Mock<IProjectProcesses>().Object; }
		}

		public static ISchemaProcesses SchemaProcesses
		{
			get { return new Mock<ISchemaProcesses>().Object; }
		}

		#endregion

		#region Objects

		public static ISchema Schema
		{
			get { return new Mock<ISchema>().Object; }
		}

		public static IProject Project
		{
			get { return new Mock<IProject>().Object; }
		}

		#endregion
	}
}
