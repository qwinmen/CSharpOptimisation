using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskFuncLogger.Helpers;

namespace TaskFuncLogger
{
	class Program
	{
		static void Main(string[] args)
		{
			Task.WhenAll(new[] {TaskLoggerTest(), WithCancellationTest()});
			Console.ReadKey();
		}

		/// <summary>
		/// Пример использования метода расширения WithCancellation
		/// </summary>
		/// <returns></returns>
		public static async Task WithCancellationTest()
		{
			//Создание обьекта CancellationTokenSource, отменяющего себя через заданный промежуток времени
			var cts = new CancellationTokenSource(5000);
			var ct = cts.Token;

			try
			{
				//Task.Dealy для тестирования. Заменить другим методом для проверки
				await Task.Delay(10000).WithCancellation(ct);
				Console.WriteLine("Задание завершено усешно за 10 секунд");
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Задание было закрыто через 5 секунд после старта. Слишком долгое ожидание.");
				throw;
			}
		}

		/// <summary>
		/// Пример использования логгера
		/// </summary>
		/// <returns></returns>
		public static async Task TaskLoggerTest(){
#if DEBUG
			//Использование TaskLogger приводит к лишним затратам памяти
			//и снижению производительности. Включить для отладочной версии
			TaskLogger.LogLevel = TaskLogger.TaskLogLevel.Pending;
#endif
			//Запускаем 3 задачи. Для тестирования TaskLogger их продолжительность задана явно
			var tasks = new List<Task>
			{
				Task.Delay(2000).Log("2s op"),
				Task.Delay(5000).Log("5s op"),
				Task.Delay(6000).Log("6s op"),
			};

			try
			{
				//Ожидание всех задач с отменой через 3 секунды. Только одна задача должна завершиться в указанное время.
				await Task.WhenAll(tasks).WithCancellation(new CancellationTokenSource(3000).Token);
			}
			catch (OperationCanceledException)
			{ }

			//Запрос информации о незавершенных задачах и их сортировка по убыванию продолжительности ожидания
			foreach(var op in TaskLogger.GetLogEntries().OrderBy(tle => tle.LogTime))
				Debug.WriteLine(op);
		}
	}
}