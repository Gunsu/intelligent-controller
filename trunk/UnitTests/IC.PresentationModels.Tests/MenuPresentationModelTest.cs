﻿using System;

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
			_mockEventAggregator.AddMapping<SchemaSavingEvent>(new MockSchemaSavingEvent());
			_mockEventAggregator.AddMapping<ProjectSavingEvent>(new MockProjectSavingEvent());
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
			var projectSavingEvent = (MockProjectSavingEvent) _mockEventAggregator.GetEvent<ProjectSavingEvent>();
			Assert.IsFalse(projectSavingEvent.IsPublished);
			_model.SaveProjectCommand.Execute(EventArgs.Empty);
			Assert.IsTrue(projectSavingEvent.IsPublished);
		}

		[Test]
		public void SaveSchemaCommandShouldFireSchemaSavingEvent()
		{
			var schemaSavingEvent = (MockSchemaSavingEvent) _mockEventAggregator.GetEvent<SchemaSavingEvent>();
			Assert.IsFalse(schemaSavingEvent.IsPublished);
			_model.SaveSchemaCommand.Execute(EventArgs.Empty);
			Assert.IsTrue(schemaSavingEvent.IsPublished);
		}
	}
}
