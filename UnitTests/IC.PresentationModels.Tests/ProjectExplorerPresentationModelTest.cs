using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;
using IC.PresentationModels.Tests.Mocks;
using IC.PresentationModels.Tests.Mocks.Events;
using IC.UI.Infrastructure.Events;
using NUnit.Framework;

namespace IC.PresentationModels.Tests
{
	[TestFixture]
	public sealed class ProjectExplorerPresentationModelTest
	{
		private MockEventAggregator _mockEventAggregator;

		[SetUp]
		public void SetUpTests()
		{
			_mockEventAggregator = new MockEventAggregator();
			_mockEventAggregator.AddMapping<CurrentSchemaChangedEvent>(new MockCurrentSchemaChangedEvent());
			_mockEventAggregator.AddMapping(new SchemaCreatedEvent());
		}

		/// <summary>
		/// Проверяет, что SchemaCreatedEvent должен изменить текущую схему на созданную.
		/// </summary>
		[Test]
		public void SchemaCreatedEventShouldChangeCurrentSchema()
		{
			//создаём модель
			var model = new ProjectExplorerPresentationModel(_mockEventAggregator);
			model.SchemasListItems = new ObservableCollection<ISchema>();

			//убеждаемся, что текущая схема не та же самая, что и нужная нам
			var schemaStub = Stubs.Schema;
			Assert.AreNotEqual(schemaStub, model.CurrentSchemaItem);

			//публикуем SchemaCreatedEvent
			_mockEventAggregator.GetEvent<SchemaCreatedEvent>().Publish(schemaStub);

			//убеждаемся, что текущая схема сменилась на нужную нам
			Assert.AreEqual(schemaStub, model.CurrentSchemaItem);
		}

		/// <summary>
		/// Проверяет, что SchemaCreatedEvent должен добавить созданную схему к списку всех схем.
		/// </summary>
		[Test]
		public void SchemaCreatedEventShouldAddCreatedSchemaToSchemasListItems()
		{
			//создаём модель
			var model = new ProjectExplorerPresentationModel(_mockEventAggregator);
			model.SchemasListItems = new ObservableCollection<ISchema>();

			//убеждаемся, что схема не добавлена
			var schemaStub = Stubs.Schema;
			Assert.IsFalse(model.SchemasListItems.Contains(schemaStub));

			//публикуем SchemaCreatedEvent
			_mockEventAggregator.GetEvent<SchemaCreatedEvent>().Publish(schemaStub);

			//проверяем, что схема была добавлена
			Assert.IsTrue(model.SchemasListItems.Contains(schemaStub));
		}

		/// <summary>
		/// Проверяет, что при смене текущей схемы должно публиковаться событие CurrentSchemaChangedEvent с правильными параметрами.
		/// </summary>
		[Test]
		public void CurrentSchemaChangingShouldPublishCurrentSchemaChangedEvent()
		{
			//создаём модель
			var model = new ProjectExplorerPresentationModel(_mockEventAggregator);

			//убеждаемся, что событие CurrentSchemaChangedEvent не опубликовано
			Assert.IsFalse(((MockCurrentSchemaChangedEvent)_mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>()).IsPublished);
			
			//меняем текущую схему
			var schemaStub = Stubs.Schema;
			model.CurrentSchemaItem = schemaStub;

			//убеждаемся, что событие CurrentSchemaChangedEvent верно опубликовано
			Assert.IsTrue(((MockCurrentSchemaChangedEvent)_mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>()).IsPublished);
			var publishedPayload =
				((MockCurrentSchemaChangedEvent) _mockEventAggregator.GetEvent<CurrentSchemaChangedEvent>()).PayloadPublished;
			Assert.AreSame(schemaStub, publishedPayload);
		}
	}
}
