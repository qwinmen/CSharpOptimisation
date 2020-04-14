/*
 * Паттерн поведения - template method (шаблонный метод)
 * Позволяет создать небольшой каркас для решения определенной задачи, когда
 * базовый класс описывает основные шаги решения, заставляя наследников предоставить
 * недостающие куски кода.
 */

using System.Collections.Generic;
using System.Linq;

namespace Optimisation.Patterns.TemplateMethods
{
	public abstract class ReaderTemplate
	{
		private int _currentPosition;

		public IEnumerable<LogEntry> ReadLogEntry()
		{
			return ReadEntries(ref _currentPosition).Select(ParseLogEntry);
		}

		protected abstract IEnumerable<string> ReadEntries(ref int position);

		protected abstract LogEntry ParseLogEntry(string strEntry);
	}

	public class LogEntry
	{
		public static LogEntry Parse(string line)
		{
			throw new System.NotImplementedException();
		}
	}
}
