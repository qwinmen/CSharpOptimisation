using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TaskFuncLogger
{
	/// <summary>
	/// Может использоваться для вывода информации о незавершенных асинхронных операциях.
	/// Такая информация полезна в ходе отладки, если приложение виснет из-за некорректного запроса
	/// или отсутствия реакции сервера.
	/// </summary>
	public static class TaskLogger
	{
		public enum TaskLogLevel
		{
			None,
			Pending,
		}

		public static TaskLogLevel LogLevel { get; set; }

		public sealed class TaskLogEntry
		{
			public Task Task { get; internal set; }

			/// <summary>
			/// Описание задания
			/// </summary>
			public String Tag { get; internal set; }

			/// <summary>
			/// Время запуска задачи
			/// </summary>
			public DateTime LogTime { get; internal set; }

			/// <summary>
			/// Метод, запустивший задание
			/// </summary>
			public string CallerMemberName { get; internal set; }

			/// <summary>
			/// Путь к файлу с методом
			/// </summary>
			public string CallerFilePath { get; internal set; }

			/// <summary>
			/// Точка вызова метода логгирования
			/// </summary>
			public Int32 CallerLineNumber { get; internal set; }

			public override string ToString() =>
				$"► LogTime={LogTime}, Tag={Tag ?? "(none)"}, Member={CallerMemberName}, File={CallerFilePath} ({CallerLineNumber})";
		}

		private static readonly ConcurrentDictionary<Task, TaskLogEntry> s_log =
			new ConcurrentDictionary<Task, TaskLogEntry>();

		/// <summary>
		/// Список задач, не уложившихся в установленные временные ограничения
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<TaskLogEntry> GetLogEntries() => s_log.Values;

		public static Task<TResult> Log<TResult>(this Task<TResult> task, string tag = null,
			[CallerMemberName] string callerMemberName = null,
			[CallerFilePath] string callerFilePath = null,
			[CallerLineNumber] Int32 callerLineNumber = 1)
		{
			return (Task<TResult>) Log((Task) task, tag, callerMemberName, callerFilePath, callerLineNumber);
		}

		public static Task Log(this Task task, string tag = null,
			[CallerMemberName] string callerMemberName = null,
			[CallerFilePath] string callerFilePath = null,
			[CallerLineNumber] Int32 callerLineNumber = 1)
		{
			if (LogLevel == TaskLogLevel.None)
				return task;

			var logEntry = new TaskLogEntry
			{
				Task = task,
				LogTime = DateTime.Now,
				Tag = tag,
				CallerMemberName = callerMemberName,
				CallerFilePath = callerFilePath,
				CallerLineNumber = callerLineNumber,
			};
			s_log[task] = logEntry;
			task.ContinueWith(t =>
			{
				TaskLogEntry entry;
				s_log.TryRemove(t, out entry);
			}, TaskContinuationOptions.ExecuteSynchronously);

			return task;
		}
	}
}