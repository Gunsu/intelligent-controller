using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using ValidationAspects;

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
		[XmlIgnore]
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

		public Schema()
		{
			IsSaved = false;
			_lastStates = new LinkedList<XElement>();
		}

		/// <summary>
		/// Сохраняет схему на жёстком диске.
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
	}
}
