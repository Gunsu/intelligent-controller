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
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				Check.Require(ErrorMessages.Count == 1, "ErrorMessages содержит не одно сообщение об ошибке.");
                
				return ErrorMessages[0];
			}
			set
			{
				Check.Require(NoErrors == false,
					          "Невозможно указать сообщение об ошибке, если процесс завершён без ошибок.");

				ErrorMessages = new StringCollection();
				ErrorMessages.Add(value);
			}
		}

		/// <summary>
		/// Получает все сообщения об ошибках.
		/// </summary>
		public StringCollection ErrorMessages { get; set; }

		private bool _noErrors;

		/// <summary>
		/// Обозначает закончился процесс с ошибками или без ошибок.
		/// </summary>
		public bool NoErrors
		{
			get { return _noErrors; }
			set
			{
				if (value == true)
				{
					Check.Require((ErrorMessages == null) || (ErrorMessages.Count == 0),
					              "Невозможно обозначить, что процесс завершён без ошибок, если ошибки уже указаны в ErrorMessages");
				}
				_noErrors = value;
			}
		}

		/// <summary>
		/// Получает объект, в котором находится результат выполнения процесса.
		/// </summary>
		public T Result { get; set; }
	}


	/// <summary>
	/// Результат выполнения процесса, содержащий флаг безошибочного завершения процесса
	/// и ошибки, в случае их возникновения.
	/// </summary>
	public class ProcessResult
		: ProcessResult<bool>
	{
	}
}
