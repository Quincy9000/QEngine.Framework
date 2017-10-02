using System;

namespace QEngine.Exceptions
{
	/// <summary>
	/// Simple class to throw exceptions in engine
	/// </summary>
	public class QException : Exception
	{
		public QException() { }

		public QException(string message) : base(message)
		{
			QLog.File(Message);
		}

		public QException(string message, Exception innerException) : base(message, innerException)
		{
			QLog.File(Message);

		}
	}
}