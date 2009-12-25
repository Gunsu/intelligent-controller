using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ValidationAspects;
using System.Runtime.Serialization;

namespace IC.Core.Entities.UI
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	[Serializable]
	public class Schema
	{
		/// <summary>
		/// Имя схемы.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		public bool IsSaved { get; set; }

		/// <summary>
		/// Текущая сериализованная схема UI.
		/// </summary>
		[NonSerialized] private XElement _currentUISchema;
		public XElement CurrentUISchema
		{
			get { return _currentUISchema; }
			set
			{
                _currentUISchema = value;
				if (_lastStates.Count > 10)
					_lastStates.RemoveFirst();
				_lastStates.AddLast(value);
			}
		}

		/// <summary>
		/// Последняя сохранённая схема.
		/// </summary>
		[NonSerialized] private XElement _savedUISchema;

		/// <summary>
		/// Последние изменения в схеме.
		/// </summary>
		[NonSerialized] private LinkedList<XElement> _lastStates;

		private string _serializedUISchema;

		public Schema()
		{
			_lastStates = new LinkedList<XElement>();
		}

		/// <summary>
		/// Сохраняет схему.
		/// </summary>
		/// <param name="uiSchema">Сериализованный набор компонентов в дизайнере.</param>
		/// <returns>Возвращает true, если схема успешно сохранена.</returns>
		public bool Save([NotNull] XElement uiSchema)
		{
			CurrentUISchema = uiSchema;
			_savedUISchema = uiSchema;
			IsSaved = true;
			return true;
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			_serializedUISchema = _savedUISchema.ToString();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			_savedUISchema = XElement.Parse(_serializedUISchema);
			_lastStates = new LinkedList<XElement>();
			CurrentUISchema = _savedUISchema;
		}
	}
}
