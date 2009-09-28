using System;

using IC.PresentationModels.Tests.Mocks;
using IC.PresentationModels.Tests.Mocks.Events;
using IC.UI.Infrastructure.Events;

using NUnit.Framework;

namespace IC.PresentationModels.Tests
{
	[TestFixture]
	public sealed class MenuPresentationModelTest
	{
		private MockEventAggregator _mockEventAggregator;
		private MenuPresentationModel _model;

		[SetUp]
		public void SetUpTests()
		{
			_mockEventAggregator = new MockEventAggregator();
			_mockEventAggregator.AddMapping(new ProjectOpenedEvent());
			_mockEventAggregator.AddMapping(new ProjectClosedEvent());
			_model = new MenuPresentationModel(_mockEventAggregator);
		}

		[Test]
		public void CreateProjectCommandShouldFireProjectCreatingEvent()
		{
			var projectCreatingEvent = new MockProjectCreatingEvent();
			_mockEventAggregator.AddMapping<ProjectCreatingEvent>(projectCreatingEvent);
			Assert.IsFalse(projectCreatingEvent.IsPublished);
			_model.CreateProjectCommand.Execute(EventArgs.Empty);
			Assert.IsTrue(projectCreatingEvent.IsPublished);
		}

		[Test]
		public void CreateSchemaCommandShouldFireSchemaCreatingEvent()
		{
			var schemaCreatingEvent = new MockSchemaCreatingEvent();
			_mockEventAggregator.AddMapping<SchemaCreatingEvent>(schemaCreatingEvent);
			Assert.IsFalse(schemaCreatingEvent.IsPublished);
			_model.CreateSchemaCommand.Execute(EventArgs.Empty);
			Assert.IsTrue(schemaCreatingEvent.IsPublished);
		}

		[Test]
		public void SaveProjectCommandShouldFireProjectSavingEvent()
		{
			var projectSavingEvent = new MockProjectSavingEvent();
			_mockEventAggregator.AddMapping<ProjectSavingEvent>(projectSavingEvent);
			Assert.IsFalse(projectSavingEvent.IsPublished);
			_model.SaveSchemaCommand.Execute(EventArgs.Empty);
			Assert.IsTrue(projectSavingEvent.IsPublished);
		}
	}
}
