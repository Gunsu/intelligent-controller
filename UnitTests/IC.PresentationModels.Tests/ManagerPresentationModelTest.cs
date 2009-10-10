﻿using System;
using System.Collections.Generic;
using IC.UI.Infrastructure.Interfaces.Windows;
using Microsoft.Practices.Unity;

using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using IC.PresentationModels.Tests.Mocks;
using IC.PresentationModels.Tests.Mocks.Events;
using IC.UI.Infrastructure.Events;
using Project.Utils.Common;

using Moq;
using NUnit.Framework;

namespace IC.PresentationModels.Tests
{
	[TestFixture]
	public sealed class ManagerPresentationModelTest
	{
		private MockEventAggregator _mockEventAggregator;
		private readonly IUnityContainer _mockContainer = new MockUnityContainer();
		
		[SetUp]
		public void SetUpTests()
		{
			_mockEventAggregator = new MockEventAggregator();
			_mockEventAggregator.AddMapping(new ProjectCreatingEvent());
			_mockEventAggregator.AddMapping(new ProjectSavingEvent());
			_mockEventAggregator.AddMapping(new SchemaCreatingEvent());
			_mockEventAggregator.AddMapping<SchemaSavedEvent>(new MockSchemaSavedEvent());
			_mockEventAggregator.AddMapping<ProjectSavedEvent>(new MockProjectSavedEvent());
			_mockEventAggregator.AddMapping<SchemaSavingEvent>(new MockSchemaSavingEvent());
			_mockEventAggregator.AddMapping<ProjectCreatedEvent>(new MockProjectCreatedEvent());
		}

		/// <summary>
		/// Проверяет, что событие ProjectSavingEvent вызовет метод SaveProject
		/// и в случае удачного сохранения опубликует событие ProjectSavedEvent.
		/// </summary>
		[Test]
		public void ProjectSavingEventShouldSaveProjectAndFireProjectSavedEvent()
		{
			//имитируем удачное сохранение проекта
			var mockProjectProcesses = new Mock<IProjectProcesses>();
			mockProjectProcesses.Setup(x => x.Save(It.IsAny<IProject>()))
				.Returns(new ProcessResult<List<ISchema>>() {NoErrors = true});
			
			//создаём нашу модель
			var model = new ManagerPresentationModel(_mockEventAggregator,
													 mockProjectProcesses.Object,
													 Stubs.CreateProjectWindow,
													 Stubs.CreateSchemaWindow);

			//удостоверяемся, что событие ProjectSavedEvent не опубликовано
			Assert.IsFalse(((MockProjectSavedEvent)_mockEventAggregator.GetEvent<ProjectSavedEvent>()).IsPublished);

			//публикуем событие ProjectSavingEvent
			_mockEventAggregator.GetEvent<ProjectSavingEvent>().Publish(EventArgs.Empty);

			//проверяем, что сохранение было вызвано и событие ProjectSavedEvent опубликовано
			mockProjectProcesses.Verify(x => x.Save(It.IsAny<IProject>()), Times.Once());
			Assert.IsTrue(((MockProjectSavedEvent)_mockEventAggregator.GetEvent<ProjectSavedEvent>()).IsPublished);
		}

		/// <summary>
		/// Проверяет, что событие ProjectSavingEvent вызовет метод SaveProject
		/// и в случае неудачного сохранения не опубликует событие ProjectSavedEvent.
		/// </summary>
		[Test]
		public void ProjectSavingEventShouldNotFireProjectSavedEventIfSaveFails()
		{
			//имитируем неудачное сохранение проекта
			var mockProjectProcesses = new Mock<IProjectProcesses>();
			mockProjectProcesses.Setup(x => x.Save(It.IsAny<IProject>()))
				.Returns(new ProcessResult<List<ISchema>>() { NoErrors = false });

			//создаём нашу модель
			var model = new ManagerPresentationModel(_mockEventAggregator,
													 mockProjectProcesses.Object,
													 Stubs.CreateProjectWindow,
													 Stubs.CreateSchemaWindow);

			//удостоверяемся, что событие ProjectSavedEvent не опубликовано
			Assert.IsFalse(((MockProjectSavedEvent) _mockEventAggregator.GetEvent<ProjectSavedEvent>()).IsPublished);

			//публикуем событие ProjectSavingEvent
			_mockEventAggregator.GetEvent<ProjectSavingEvent>().Publish(EventArgs.Empty);

			//проверяем, что сохранение было вызвано и событие ProjectSavingEvent по-прежнему не опубликовано
			mockProjectProcesses.Verify(x => x.Save(It.IsAny<IProject>()), Times.Once());
			Assert.IsFalse(((MockProjectSavedEvent)_mockEventAggregator.GetEvent<ProjectSavedEvent>()).IsPublished);
		}

		/// <summary>
		/// Проверяет, что событие ProjectSavingEvent сделает всё необходимое для повторного сохранения проекта,
		/// если сохранение не удалось и результат показал, что не все схемы сохранены.
		/// </summary>
		[Test]
		public void ProjectSavingEventShouldSaveSchemaAndTryToSaveProjectAgainIfSchemasNotSaved()
		{
			//имитируем неудачное сохранение проекта, показывающее, что не все схемы сохранены
			var mockProjectProcesses = new Mock<IProjectProcesses>();
			var mockSchema = new Mock<ISchema>();
			var notSavedSchemas = new List<ISchema>();
			notSavedSchemas.Add(mockSchema.Object);
			mockProjectProcesses.Setup(x => x.Save(It.IsAny<IProject>()))
				.Returns(new ProcessResult<List<ISchema>>() { NoErrors = false, Result = notSavedSchemas});

			//создаём нашу модель
			var model = new ManagerPresentationModel(_mockEventAggregator,
													 mockProjectProcesses.Object,
													 Stubs.CreateProjectWindow,
													 Stubs.CreateSchemaWindow);

			//удостоверяемся, что событие SchemaSavingEvent не опубликовано и нет подписки на SchemaSavedEvent
			Assert.IsFalse(((MockSchemaSavingEvent)_mockEventAggregator.GetEvent<SchemaSavingEvent>()).IsPublished);
			Assert.IsFalse(((MockSchemaSavedEvent)_mockEventAggregator.GetEvent<SchemaSavedEvent>()).IsSubscribed);

			//публикуем событие ProjectSavingEvent
			_mockEventAggregator.GetEvent<ProjectSavingEvent>().Publish(EventArgs.Empty);

			//проверяем, что сохранение было вызвано, событие SchemaSavingEvent опубликовано
			//и произошла подписка на SchemaSavedEvent
			mockProjectProcesses.Verify(x => x.Save(It.IsAny<IProject>()), Times.Once());
			Assert.IsTrue(((MockSchemaSavingEvent)_mockEventAggregator.GetEvent<SchemaSavingEvent>()).IsPublished);
			Assert.IsTrue(((MockSchemaSavedEvent)_mockEventAggregator.GetEvent<SchemaSavedEvent>()).IsSubscribed);

			//публикеум событие SchemaSavedEvent
			_mockEventAggregator.GetEvent<SchemaSavedEvent>().Publish(EventArgs.Empty);

			//проверяем, что был вызван SaveProject ещё раз
			mockProjectProcesses.Verify(x => x.Save(It.IsAny<IProject>()), Times.Exactly(2));
		}

		/// <summary>
		/// Проверяет, что при событии ProjectCreatedEvent сменится CurrentProject.
		/// </summary>
		[Test]
		public void ProjectCreatedEventShouldChangeCurrentProject()
		{
			//создаём модель
			var model = new ManagerPresentationModel(_mockEventAggregator,
													 Stubs.ProjectProcesses,
													 Stubs.CreateProjectWindow,
													 Stubs.CreateSchemaWindow);
			//проверяем что текущий проект пуст
			Assert.IsNull(model.CurrentProject);

			//публикуем событие ProjectCreatedEvent
			var mockProject = new Mock<IProject>();
			_mockEventAggregator.GetEvent<ProjectCreatedEvent>().Publish(mockProject.Object);

			Assert.IsNotNull(model.CurrentProject);
		}

        /// <summary>
		/// Проверяет, что при событии SchemaCreatingEvent будет показано окно CreateSchemaWindow.
		/// </summary>
		[Test]
		public void SchemaCreatingEventShouldShowCreateSchemaWindow()
		{
        	//создаём модель
			var mockCreateSchemaWindow = new Mock<ICreateSchemaWindow>();
        	var model = new ManagerPresentationModel(_mockEventAggregator,
        	                                         Stubs.ProjectProcesses,
        	                                         Stubs.CreateProjectWindow,
        	                                         mockCreateSchemaWindow.Object);
        	var mockProject = new Mock<IProject>();
        	model.CurrentProject = mockProject.Object;

			//Публикуем событие SchemaCreatingEvent
			_mockEventAggregator.GetEvent<SchemaCreatingEvent>().Publish(EventArgs.Empty);

			//Проверяем, что вызвался метод ShowDialog()
			mockCreateSchemaWindow.Verify(x => x.ShowDialog(mockProject.Object), Times.Once());
		}
	}
}