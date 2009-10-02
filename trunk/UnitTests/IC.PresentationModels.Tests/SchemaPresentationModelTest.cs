﻿using System;

using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using IC.PresentationModels.Tests.Mocks;
using IC.PresentationModels.Tests.Mocks.Events;
using IC.UI.Infrastructure.Events;

using Moq;
using NUnit.Framework;
using Project.Utils.Common;

namespace IC.PresentationModels.Tests
{
	[TestFixture]
	public sealed class SchemaPresentationModelTest
	{
		private MockEventAggregator _mockEventAggregator;

		[SetUp]
		public void SetUpTests()
		{
			_mockEventAggregator = new MockEventAggregator();
			_mockEventAggregator.AddMapping<SchemaSavingEvent>(new MockSchemaSavingEvent());
			_mockEventAggregator.AddMapping(new CurrentSchemaChangedEvent());
		}

		[Test]
		public void SchemaSavingEventShouldSaveCurrentSchema()
		{
			//Имитируем удачное сохранение схемы
			var mockSchemaProcesses = new Mock<ISchemaProcesses>();
			mockSchemaProcesses.Setup(x => x.Save(It.IsAny<ISchema>())).Returns(new ProcessResult());
			
			//Создаём нашу модель
			var model = new SchemaPresentationModel(_mockEventAggregator, mockSchemaProcesses.Object);

			//Публикуем событие SchemaSavingEvent
			_mockEventAggregator.GetEvent<SchemaSavingEvent>().Publish(EventArgs.Empty);

			//Проверяем, что произошло сохранение схемы.
			mockSchemaProcesses.Verify(x => x.Save(It.IsAny<ISchema>()), Times.Once());
		}
	}
}