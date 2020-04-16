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
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;

namespace Optimisation.Patterns.Observers
{
	/// <summary>
	/// Пример с использованием делегата
	/// </summary>
	class LogReaderObserver : IDisposable
	{
		private readonly Action<string> _logEntrySubscriber;

		//Отношение 1:1 между наблюдателем и наблюдаемым
		public LogReaderObserver(string fileName, Action<string> logEntrySubscriber)
		{
			Contract.Requires(File.Exists(fileName));

			_logEntrySubscriber = logEntrySubscriber;
			TimerCallback tc = CheckFile;
			_timer = new Timer(tc, null, CheckFileInterval, CheckFileInterval);
		}

		private readonly Timer _timer;
		private readonly static TimeSpan CheckFileInterval = TimeSpan.FromSeconds(5);

		public void Dispose()
		{
			_timer.Dispose();
		}

		private void CheckFile(object state)
		{
			foreach (string logEntry in ReadNewLogEntries())
			{
				_logEntrySubscriber(logEntry);
			}
		}

		private IEnumerable<string> ReadNewLogEntries()
		{
			//Читаем из файла новые записи,
			//которые появились с момента последнего чтения
			throw new NotImplementedException();
		}
	}
}
