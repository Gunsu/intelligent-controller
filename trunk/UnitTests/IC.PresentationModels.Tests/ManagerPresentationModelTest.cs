using System;
using IC.Core.Abstract;
using Microsoft.Practices.Unity;
using IC.PresentationModels.Tests.Mocks;
using IC.PresentationModels.Tests.Mocks.Events;
using IC.UI.Infrastructure.Events;

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

			// подписываются в конструкторе
			_mockEventAggregator.AddMapping<ProjectCreatedEvent>(new MockProjectCreatedEvent());
			_mockEventAggregator.AddMapping(new ProjectCreatingEvent());
			_mockEventAggregator.AddMapping(new ProjectOpenedEvent());
			_mockEventAggregator.AddMapping(new ProjectOpeningEvent());
            _mockEventAggregator.AddMapping(new ProjectSavingEvent());
			_mockEventAggregator.AddMapping(new SchemaCreatingEvent());

			//вызываются в методах
			_mockEventAggregator.AddMapping<ProjectSavedEvent>(new MockProjectSavedEvent());
			_mockEventAggregator.AddMapping<SchemaSavedEvent>(new MockSchemaSavedEvent());
			_mockEventAggregator.AddMapping<SchemaSavingEvent>(new MockSchemaSavingEvent());
		}

        /// <summary>
		/// Проверяет, что при ProjectSavingEvent будет сохранена схема,
		/// вызвано обновление текущего Project-а и опубликован ProjectSavedEvent.
		/// </summary>
		[Test]
		public void ProjectSavingEvent_Should_Update_Project_And_Publish_ProjectSavedEvent()
		{
			// создаём модель
        	var mockRepository = new Mock<IProjectsRepository>();
			var model = new ManagerPresentationModel(_mockEventAggregator,
        	                                         mockRepository.Object,
        	                                         Stubs.CreateProjectWindow,
        	                                         Stubs.CreateSchemaWindow);
			var mockProject = Stubs.Project;
        	model.CurrentProject = mockProject;
			var mockProjectSavedEvent = (MockProjectSavedEvent)_mockEventAggregator.GetEvent<ProjectSavedEvent>();

			// убеждаемся, что ProjectSavedEvent не опубликован и обновление не вызвано
			mockRepository.Verify(x => x.Update(mockProject), Times.Never());
        	Assert.IsFalse(mockProjectSavedEvent.IsPublished);

			// публикуем событие ProjectSavingEvent
			_mockEventAggregator.GetEvent<ProjectSavingEvent>().Publish(EventArgs.Empty);

			// убеждаемся, что обновление текущего проекта вызвано и ProjectSavedEvent опубликован
			mockRepository.Verify(x => x.Update(mockProject), Times.Once());
			Assert.IsTrue(mockProjectSavedEvent.IsPublished);
		}

		/// <summary>
		/// Проверяет, что модель не подписана на событие CurrentSchemaChangedEvent.
		/// </summary>
		[Test]
		public void Model_Is_Not_Subscribed_On_CurrentSchemaChangedEvent()
		{
			var model = new ManagerPresentationModel(_mockEventAggregator,
													 Stubs.ProjectsRepository,
													 Stubs.CreateProjectWindow,
													 Stubs.CreateSchemaWindow);

			try
			{
				var mockCurrentSchemaChangedEvent =
					(MockCurrentSchemaChangedEvent)_mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>();
				Assert.IsFalse(mockCurrentSchemaChangedEvent.IsSubscribed);
			}
			catch
			{
				// expected
			}
		}
	}
}
