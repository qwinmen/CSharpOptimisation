using System;

/*
 * Visitor (Посетитель) - паттерн поведения.
 * Предназначен для добавления новых операций над иерархией типов без ее изменения.
 *
 */
namespace Optimisation.Patterns.Visitors
{
	public class DataBaseLogSaver
	{
		/// <summary>
		/// Пример реализаций метода, которую можно переписать на Visitor.
		/// </summary>
		/// <param name="logEntry"></param>
		public void SaveLogEntry(BaseLogEntry logEntry)
		{
			ExceptionLogEntry exception = logEntry as ExceptionLogEntry;
			if (exception != null)
			{
				SaveException(exception);
			}
			else
			{
				SimpleLogEntry simpleLog = logEntry as SimpleLogEntry;
				if (simpleLog != null)
					SaveSimpleLogEntry(simpleLog);

				throw new InvalidOperationException("Unknow log entry type.");
			}
		}

		private void SaveSimpleLogEntry(SimpleLogEntry simpleLog)
		{
			throw new NotImplementedException();
		}

		private void SaveException(ExceptionLogEntry exception)
		{
			throw new NotImplementedException();
		}
	}

	//DataBaseLogSaver перепишем с использованием Visitor:
	public class DataBaseLogSaverEx: ILogEntryVisitor
	{
		public void SaveLogEntry(BaseLogEntry logEntry)
		{
			logEntry.Accept(this);
		}

		void ILogEntryVisitor.Visit(ExceptionLogEntry exceptionLogEntry)
		{
			SaveException(exceptionLogEntry);
		}

		void ILogEntryVisitor.Visit(SimpleLogEntry simpleLogEntry)
		{
			SaveSimpleLogEntry(simpleLogEntry);
		}

		private void SaveSimpleLogEntry(SimpleLogEntry simpleLog)
		{
			throw new NotImplementedException();
		}

		private void SaveException(ExceptionLogEntry exception)
		{
			throw new NotImplementedException();
		}
	}

	//Базовый класс определяет операции:
	public abstract class BaseLogEntry
	{
		//Добавляем абстрактный метод, который принимает ILogEntryVisitor
		public abstract void Accept(ILogEntryVisitor logEntryVisitor);
	}

	public interface ILogEntryVisitor
	{
		void Visit(ExceptionLogEntry exceptionLogEntry);
		void Visit(SimpleLogEntry simpleLogEntry);
	}

	//Поведение\реализация операций базового класса BaseLogEntry определяется\реализуется наследниками:
	public class SimpleLogEntry: BaseLogEntry
	{
		public override void Accept(ILogEntryVisitor logEntryVisitor)
		{
			//Благодаря перегрузке методов в интерфейсе, будет выбран Visit(SimpleLogEntry simpleLogEntry)
			logEntryVisitor.Visit(this);
		}
	}

	//Поведение\реализация операций базового класса BaseLogEntry определяется\реализуется наследниками:
	public class ExceptionLogEntry: BaseLogEntry
	{
		public override void Accept(ILogEntryVisitor logEntryVisitor)
		{
			logEntryVisitor.Visit(this);
		}
	}
}
