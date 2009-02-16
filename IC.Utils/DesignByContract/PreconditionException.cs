using System;

namespace IC.Utils.DesignByContract
{
	public sealed class PreconditionException : DesignByContractException
	{
		/// <summary>
		/// Precondition Exception.
		/// </summary>
		public PreconditionException() { }
		/// <summary>
		/// Precondition Exception.
		/// </summary>
		public PreconditionException(string message) : base(message) { }
		/// <summary>
		/// Precondition Exception.
		/// </summary>
		public PreconditionException(string message, Exception inner) : base(message, inner) { }
	}
}
