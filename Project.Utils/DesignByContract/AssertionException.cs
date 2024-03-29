﻿using System;

namespace Project.Utils.DesignByContract
{
	/// <summary>
	/// Exception raised when an assertion fails.
	/// </summary>
	public sealed class AssertionException : DesignByContractException
	{
		/// <summary>
		/// Assertion Exception.
		/// </summary>
		public AssertionException() { }
		/// <summary>
		/// Assertion Exception.
		/// </summary>
		public AssertionException(string message) : base(message) { }
		/// <summary>
		/// Assertion Exception.
		/// </summary>
		public AssertionException(string message, Exception inner) : base(message, inner) { }
	}
}
