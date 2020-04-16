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
using System.Reactive.Subjects;

namespace Optimisation.Patterns.Observers
{
	class LogFileReaderRxObservable: IDisposable
	{
		private readonly string _logFileName;
		private readonly Subject<string> _logEntriesSubject = new Subject<string>();

		public LogFileReaderRxObservable(string logFileName)
		{
			_logFileName = logFileName;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
