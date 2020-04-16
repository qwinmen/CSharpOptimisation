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
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Optimisation.Patterns.TemplateMethods;

namespace Optimisation.Patterns.Observers
{
	class ExampleUsing
	{
		//Пример использования
		void Using(LogFileReaderRxObservable logFileReaderRxObservable)
		{
			IObservable<LogEntry> messageObservable = logFileReaderRxObservable.NewMessages
					.Select(ParseLogMessages<LogEntry>)
					.Where(m => m.Severity == Severity.Critical)
				;
			messageObservable.Buffer(10).Subscribe(onNext =>
				{
					IList criticalMessages = null;
				});
		}

		private TResult ParseLogMessages<TResult>(string arg)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Пример реализаций на основе интерфейсов IObserver\IObservable
	/// </summary>
	class LogFileReaderRxObservable : IDisposable
	{
		private readonly string _logFileName;
		private readonly Subject<string> _logEntriesSubject = new Subject<string>();

		public LogFileReaderRxObservable(string logFileName)
		{
			_logFileName = logFileName;
		}

		public void Dispose()
		{
			CloseFile();
			//Уведомляем подписчиков, что событий больше не будет
			_logEntriesSubject.OnCompleted();
		}

		private void CloseFile()
		{
			throw new NotImplementedException();
		}

		public IObservable<string> NewMessages
		{
			get { return _logEntriesSubject; }
		}

		private void CheckFile()
		{
			foreach (string logEntry in ReadNewLogEntries())
			{
				//Уведомить о новом событии
				_logEntriesSubject.OnNext(logEntry);
			}
		}

		private IEnumerable<string> ReadNewLogEntries()
		{
			throw new NotImplementedException();
		}
	}
}
