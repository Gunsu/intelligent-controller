using System.Xml.Linq;

using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using IC.PresentationModels.Tests.Mocks;
using IC.PresentationModels.Tests.Mocks.Events;
using IC.UI.Infrastructure.Events;

using Moq;
using NUnit.Framework;

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
			_mockEventAggregator.AddMapping<SchemaSavedEvent>(new MockSchemaSavedEvent());
			_mockEventAggregator.AddMapping(new CurrentSchemaChangedEvent());
		}

		[Test]
		public void SchemaSavingEventShouldSaveCurrentSchema()
		{
			//Имитируем удачное сохранение схемы
			var mockSchemaProcesses = new Mock<ISchemaProcesses>();
			mockSchemaProcesses.Setup(x => x.Save(It.IsAny<ISchema>(), It.IsAny<XElement>())).Returns(true);
			
			//Создаём нашу модель
			var model = new SchemaPresentationModel(_mockEventAggregator, mockSchemaProcesses.Object);
			model.CurrentSchema = new Mock<ISchema>().Object;

			//Проверяем, что событие SchemaSavedEvent не опубликовано
			Assert.IsFalse(((MockSchemaSavedEvent)(_mockEventAggregator.GetEvent<SchemaSavedEvent>())).IsPublished);

			//Публикуем событие SchemaSavingEvent
			_mockEventAggregator.GetEvent<SchemaSavingEvent>().Publish(new XElement("root"));

			//Проверяем, что произошло сохранение схемы и событие SchemaSavedEvent опубликовано
			mockSchemaProcesses.Verify(x => x.Save(It.IsAny<ISchema>(), It.IsAny<XElement>()), Times.Once());
			Assert.IsTrue(((MockSchemaSavedEvent)(_mockEventAggregator.GetEvent<SchemaSavedEvent>())).IsPublished);
		}
	}
}
