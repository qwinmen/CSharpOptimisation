/*
 * Паттерн Наблюдатель (Observer)
 * Наблюдатель уведомляет все заинтересованные стороны о произошедшем событии
 * или об изменении своего состояния.
 * Реализация возможна при помощи:
 * - делегатов (методоы обратного вызова)
 * - событий
 * - специализированных интерфейсов наблюдателей
 * - интерфейсов IObserver\IObservable
 */

using System;
using System.Collections.Generic;

namespace Optimisation.Patterns.Observers
{
	class Example
	{
		void LogForwarded(LogFileReaderEventObservable logFileReaderEventObservable)
		{
			logFileReaderEventObservable.OnNewLogEntry += HandleNewLogEntry;
		}

		private void HandleNewLogEntry(object sender, LogEntryEventArgs e)
		{
			string logEntry = e.LogEntry;
			string logFile = ((LogFileReaderEventObservable) sender).LogFileName;
		}
	}

	public class LogEntryEventArgs : EventArgs
	{
		public LogEntryEventArgs(string logEntry)
		{
			LogEntry = logEntry;
		}

		public string LogEntry { get; internal set; }
	}

	/// <summary>
	/// Пример с использованием событий.
	/// Позволит подписаться на событие получения новых записей из файла любому количеству подписчиков.
	/// При этом нет гарантий, что подписчики вообще будут\есть
	/// </summary>
	class LogFileReaderEventObservable: IDisposable
	{
		private readonly string _logFileName;

		public LogFileReaderEventObservable(string logFileName)
		{
			_logFileName = logFileName;
			LogFileName = logFileName;
		}

		public event EventHandler<LogEntryEventArgs> OnNewLogEntry;
		public string LogFileName { get; }

		private void CheckFile()
		{
			foreach (string logEntry in ReadNewLogEntries())
			{
				RaiseNewLogEntry(logEntry);
			}
		}

		private IEnumerable<string> ReadNewLogEntries()
		{
			throw new NotImplementedException();
		}

		private void RaiseNewLogEntry(string logEntry)
		{
			var handler = OnNewLogEntry;
			if (handler != null)
				handler(this, new LogEntryEventArgs(logEntry));
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
