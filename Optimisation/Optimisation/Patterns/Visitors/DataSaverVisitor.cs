
using System;
/*
 * Когда количество конкретных типов иерархий наследования невелико, интерфейс посетителя можно
 * заменить списком делегатов. Для этого метод Accept можно переименовать в Match, который будет
 * принимать несколько делегатов для обработки конкретных типов иерархий.
 */
namespace Optimisation.Patterns.Visitors
{
	public class DataSaverVisitor
	{
		public void SaveLog(LogEntry logEntry)
		{
			logEntry.Match(ex => SaveException(ex), simple => SaveSimpleLog(simple));
		}

		private void SaveSimpleLog(SimpleLogEntry simple)
		{
			throw new NotImplementedException();
		}

		private void SaveException(ExceptionLogEntry exceptionLogEntry)
		{
			throw new NotImplementedException();
		}
	}

	//Функциональная версия паттерна Visitor
	public abstract class LogEntry
	{
		public void Match(
			Action<ExceptionLogEntry> exceptionLogEntryMatch,
			Action<SimpleLogEntry> simpleLogEntryMatch)
		{
			ExceptionLogEntry exceptionLogEntry = this as ExceptionLogEntry;
			if (exceptionLogEntry != null)
			{
				exceptionLogEntryMatch(exceptionLogEntry);
				return;
			}

			SimpleLogEntry simpleLogEntry = this as SimpleLogEntry;
			if (simpleLogEntry != null)
			{
				simpleLogEntryMatch(simpleLogEntry);
				return;
			}

			throw new InvalidOperationException("Unknow log entry type.");
		}
	}
}
