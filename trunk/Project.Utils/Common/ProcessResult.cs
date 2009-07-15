using System.Collections.Specialized;

using Project.Utils.DesignByContract;

namespace Project.Utils.Common
{
	/// <summary>
	/// Результат выполнения процесса, содержащий сам результат выполнения,
	/// флаг безошибочного завершения процесса и ошибки, в случае их возникновения.
	/// </summary>
	public class ProcessResult<T>
	{
		/// <summary>
		/// Получает сообщение об ошибке.
		/// Использовать только если сообщение об ошибке одно и только одно.
		/// Возвращает null, если нет ошибок.
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				Check.Require(ErrorMessages.Count == 1, "ErrorMessages содержит не одно сообщение об ошибке.");

				return ErrorMessages[0];
			}
		}

		/// <summary>
		/// Получает все сообщения об ошибках.
		/// Возвращает null, если нет ошибок.
		/// </summary>
		public StringCollection ErrorMessages { get; protected set; }

		/// <summary>
		/// Обозначает закончился процесс с ошибками или без ошибок.
		/// </summary>
		public bool NoErrors { get; protected set; }

		/// <summary>
		/// Получает объект, в котором находится результат выполнения процесса.
		/// Возвращает null, если были ошибки во время выполнения процесса.
		/// </summary>
		public T Result { get; protected set; }


		/// <summary>
		/// Инициализирует объект класса. Защищённый конструкор создан для использования его
		/// в non generic классе-потомке.
		/// </summary>
		protected ProcessResult()
		{
		}

		/// <summary>
		/// Инициализирует объект класса. Операция будет считаться успешной,
		/// если число сообщений об ошибках равно нулю.
		/// </summary>
		/// <param name="result">Объект, в котором находится результат выполнения процесса.</param>
		/// <param name="errorMessages">Сообщения об ошибках.</param>
		public ProcessResult(T result, StringCollection errorMessages)
		{
			Check.Require(errorMessages != null, "errorMessages не может быть равным null.");

			if (errorMessages.Count == 0)
			{
				Check.Require(result != null, "result не может быть равным null при отсутствии ошибок.");

				NoErrors = true;
				Result = result;
			}
			else
			{
				NoErrors = false;
				ErrorMessages = errorMessages;
			}
		}

		/// <summary>
		/// Инициализирует объект класса. Используется, если процесс завершён успешно и без ошибок
		/// и необходимо указать его результат.
		/// </summary>
		/// <param name="result">Объект, в котором находится результат выполнения процесса.</param>
		public ProcessResult(T result)
			: this(result, new StringCollection())
		{
		}

		/// <summary>
		/// Инициализирует объект класса. Используется, если процесс завершён с несколькими ошибками
		/// и необходимо указать сообщения об этих ошибках./>
		/// </summary>
		/// <param name="errorMessages">Сообщения об ошибках.</param>
		public ProcessResult(StringCollection errorMessages)
		{
			Check.Require(errorMessages != null, "errorMessages не может быть равным null.");

			NoErrors = false;
			ErrorMessages = errorMessages;
		}

		/// <summary>
		/// Инициализирует объект класса. Используется, если процесс завершён с одной ошибкой
		/// и необходимо указать сообщение об ошибке.
		/// </summary>
		/// <param name="errorMessage">Сообщение об ошибки.</param>
		public ProcessResult(string errorMessage)
		{
			Check.Require(!string.IsNullOrEmpty(errorMessage), "errorMessage не может быть пустым или равным null.");

			NoErrors = false;
			ErrorMessages = new StringCollection();
			ErrorMessages.Add(errorMessage);
		}
	}


	/// <summary>
	/// Результат выполнения процесса, содержащий флаг безошибочного завершения процесса
	/// и ошибки, в случае их возникновения.
	/// </summary>
	public class ProcessResult
		: ProcessResult<bool>
	{
		/// <summary>
		/// Инициализирует объект класса. Используется, если процесс завершён успешно и без ошибок
		/// и указывать результат нет необходимости.
		/// </summary>
		public ProcessResult()
			: base(true)
		{
		}

		/// <summary>
		/// Инициализирует объект класса. Операция будет считаться успешной,
		/// если флаг noErrors установлен в true.
		/// </summary>
		/// <param name="noErrors">Флаг, означающий отсутствие ошибок.</param>
		/// <param name="errorMessages">Сообщения об ошибках.</param>
		public ProcessResult(bool noErrors, StringCollection errorMessages)
		{
			if (noErrors == true)
			{
				NoErrors = true;
			}
			else
			{
				Check.Require(errorMessages != null, "errorMessages не может быть равным null.");

				NoErrors = false;
				ErrorMessages = errorMessages;
			}
		}

		/// <summary>
		/// Инициализирует объект класса. Используется, если процесс завершён с одной ошибкой
		/// и необходимо указать сообщение об ошибке.
		/// </summary>
		/// <param name="errorMessage">Сообщение об ошибки.</param>
		public ProcessResult(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Инициализирует объект класса. Используется, если процесс завершён с несколькими ошибками
		/// и необходимо указать сообщения об этих ошибках./>
		/// </summary>
		/// <param name="errorMessages">Сообщения об ошибках.</param>
		public ProcessResult(StringCollection errorMessages)
			: base(errorMessages)
		{
		}
	}
}
