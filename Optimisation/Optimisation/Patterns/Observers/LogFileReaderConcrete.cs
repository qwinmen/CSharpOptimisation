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

namespace Optimisation.Patterns.Observers
{
	public interface ILogReaderObserver
	{
		void NewLogEntry(string logentry);
		void FileWasRolled(string oldLogFile, string newLogFile);
	}

    /// <summary>
	/// Пример строготипизированного наблюдателя
	/// </summary>
	class LogFileReaderConcrete: IDisposable
	{
		private readonly string _logFileName;
		private readonly ILogReaderObserver _logReaderObserver;

		public LogFileReaderConcrete(string logFileName, ILogReaderObserver logReaderObserver)
		{
			_logFileName = logFileName;
			_logReaderObserver = logReaderObserver;
		}

		/*
		 * Добавлена дополнительная логика, которая определяет, что логгер перестал
		 * добавлять записи в файл и переключился на новый.
		 */
		private void DetectThatNewFileWasCreated()
		{
			//метод вызывается по таймеру
			if(NewLogFileWasCreated())
				_logReaderObserver.NewLogEntry(GetNewLogFileName(_logFileName));
		}

		private string GetNewLogFileName(string logFileName)
		{
			throw new NotImplementedException();
		}

		private bool NewLogFileWasCreated()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
