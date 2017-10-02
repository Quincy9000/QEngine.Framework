using System;
using System.Globalization;
using System.IO;

namespace QEngine
{
	public enum QLoggingLevel
	{
		None,
		WarningsOnly,
		ThrowOnWarnings,
	}

	/// <summary>
	/// Set of functions to log onto a file if the game crashes or unexpected things happen
	/// </summary>
	public static class QLog
	{
		public const string DebugLogDestinationPath = "LogInfo/";

		public static QLoggingLevel Level { get; set; } = QLoggingLevel.None;

		/// <summary>
		/// Literally console.writeline
		/// </summary>
		public static void WriteLine<T>(T msg)
		{
			Console.WriteLine(msg);
		}

		/// <summary>
		/// Writes message file to log destination
		/// </summary>
		/// <param name="msg"></param>
		/// <typeparam name="T"></typeparam>
		public static void File<T>(T msg)
		{
			var time = DateTime.Now;
			try
			{
				if(!Directory.Exists(DebugLogDestinationPath))
					Directory.CreateDirectory(DebugLogDestinationPath);
				using(var textWriter =
					System.IO.File.CreateText(DebugLogDestinationPath + "Log" + "_" + time.ToString("yyyy-MM-dd") + "_" + time.ToString("H-mm") + ".txt"))
				{
					textWriter.Write(time.ToString(CultureInfo.InvariantCulture) + Environment.NewLine + msg);
				}
			}
			catch(Exception e)
			{
				WriteLine(e);
			}
		}
	}
}