using System.Xml.Linq;
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

			// подписка в конструкторе
		    _mockEventAggregator.AddMapping<SchemaSavingEvent>(new MockSchemaSavingEvent());
            _mockEventAggregator.AddMapping(new CurrentSchemaChangedEvent());

			// публикуются в методах
			_mockEventAggregator.AddMapping(new CurrentSchemaChangingEvent());
			_mockEventAggregator.AddMapping<SchemaSavedEvent>(new MockSchemaSavedEvent());
		}

		/// <summary>
		/// Проверяет, что при CurrentSchemaChangedEvent меняется текущая схема не публикуется SchemaSavingEvent.
		/// </summary>
		[Test]
		public void CurrentSchemaChangedEvent_Change_CurrentSchema_And_Do_Not_Publish_SchemaSavingEvent()
		{
			var model = new SchemaPresentationModel(_mockEventAggregator);

			var schemaStub = Stubs.Schema;
			_mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>().Publish(schemaStub);
			
			Assert.AreSame(schemaStub, model.CurrentSchema);
			var mockSchemaSavingEvent = (MockSchemaSavingEvent)_mockEventAggregator.GetEvent<SchemaSavingEvent>();
			Assert.IsFalse(mockSchemaSavingEvent.IsPublished);
		}

		//[Test]
		//public void SchemaSavingEventShouldSaveCurrentSchema()
		//{
		//    //Имитируем удачное сохранение схемы
		//    var mockSchemaProcesses = new Mock<ISchemaProcesses>();
		//    mockSchemaProcesses.Setup(x => x.Save(It.IsAny<ISchema>(), It.IsAny<XElement>())).Returns(true);
			
		//    //Создаём нашу модель
		//    var model = new SchemaPresentationModel(_mockEventAggregator, mockSchemaProcesses.Object);
		//    model.CurrentSchema = new Mock<ISchema>().Object;

		//    //Проверяем, что событие SchemaSavedEvent не опубликовано
		//    Assert.IsFalse(((MockSchemaSavedEvent)(_mockEventAggregator.GetEvent<SchemaSavedEvent>())).IsPublished);

		//    //Публикуем событие SchemaSavingEvent
		//    _mockEventAggregator.GetEvent<SchemaSavingEvent>().Publish(new XElement("root"));

		//    //Проверяем, что произошло сохранение схемы и событие SchemaSavedEvent опубликовано
		//    mockSchemaProcesses.Verify(x => x.Save(It.IsAny<ISchema>(), It.IsAny<XElement>()), Times.Once());
		//    Assert.IsTrue(((MockSchemaSavedEvent)(_mockEventAggregator.GetEvent<SchemaSavedEvent>())).IsPublished);
		//}

		///// <summary>
		///// Проверяет, что при CurrentSchemaChangedEvent будет инициировано сохранение схемы.
		///// </summary>
		//[Test]
		//public void CurrentSchemaChangedEventShouldPublishSchemaSavingEvent()
		//{
		//    //создаём модель
		//    var model = new SchemaPresentationModel(_mockEventAggregator, Stubs.SchemaProcesses);

		//    //удостоверяемся, что событие SchemaSavingEvent не опубликовано
		//    Assert.IsFalse(((MockSchemaSavingEvent)(_mockEventAggregator.GetEvent<SchemaSavingEvent>())).IsPublished);

		//    //публикуем событие CurrentSchemaChangedEvent
		//    _mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>().Publish(Stubs.Schema);

		//    //удостоверяемся, что событие SchemaSavingEvent опубликовано
		//    Assert.IsTrue(((MockSchemaSavingEvent)(_mockEventAggregator.GetEvent<SchemaSavingEvent>())).IsPublished);
		//    Assert.IsNull(((MockSchemaSavingEvent)(_mockEventAggregator.GetEvent<SchemaSavingEvent>())).PublishedPayload);
		//}

		///// <summary>
		///// Проверяет, что при CurrentSchemaChangedEvent схема будет сменена.
		///// </summary>
		//[Test]
		//public void CurrentSchemaChangedEventShouldChangeCurrentSchema()
		//{
		//    //создаём модель
		//    var model = new SchemaPresentationModel(_mockEventAggregator, Stubs.SchemaProcesses);
		//    model.CurrentSchema = Stubs.Schema;

		//    //публикуем событие CurrentSchemaChangedEvent
		//    var stubNewSchema = Stubs.Schema;
		//    _mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>().Publish(stubNewSchema);

		//    //удостоверяемся, что текущая схема сменилась на верную
		//    Assert.AreSame(stubNewSchema, model.CurrentSchema);
		//}
	}
}
