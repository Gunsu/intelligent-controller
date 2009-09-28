using System;
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
			_mockEventAggregator.AddMapping<ProjectSavedEvent>(new MockProjectSavedEvent());
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
			mockProjectProcesses.Setup(x => x.Save(It.IsAny<IProject>())).Returns(new ProcessResult());
			
			//создаём нашу модель
			var model = new ManagerPresentationModel(_mockEventAggregator,
                                                     _mockContainer,
													 mockProjectProcesses.Object);

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
			mockProjectProcesses.Setup(x => x.Save(It.IsAny<IProject>())).Returns(new ProcessResult("SomeErrorText"));

			//создаём нашу модель
			var model = new ManagerPresentationModel(_mockEventAggregator,
													 _mockContainer,
													 mockProjectProcesses.Object);

			//удостоверяемся, что событие ProjectSavedEvent не опубликовано
			Assert.IsFalse(((MockProjectSavedEvent) _mockEventAggregator.GetEvent<ProjectSavedEvent>()).IsPublished);

			//публикуем событие ProjectSavingEvent
			_mockEventAggregator.GetEvent<ProjectSavingEvent>().Publish(EventArgs.Empty);

			//проверяем, что сохранение было вызвано и событие ProjectSavingEvent по-прежнему не опубликовано
			mockProjectProcesses.Verify(x => x.Save(It.IsAny<IProject>()), Times.Once());
			Assert.IsFalse(((MockProjectSavedEvent)_mockEventAggregator.GetEvent<ProjectSavedEvent>()).IsPublished);
		}
	}
}
