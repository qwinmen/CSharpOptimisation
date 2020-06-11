/*
 * http://sergeyteplyakov.blogspot.com/2015/07/foreachasync.html
 * Идеома ForEachAsync
 * Задача: обрабатывать некие данные разными параллельными задачами
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestLock.Partitioner
{
	public static class AsyncPartitions
	{
		public static IEnumerable<Task<TTask>> SelectAsync<TItem, TTask>(this IEnumerable<TItem> source, Func<TItem, Task<TTask>> selector,
			int degreeOfParallelism)
		{
			Contract.Requires(source != null);
			Contract.Requires(selector != null);
			//Материализуем последовательность
			var tasks = source.ToList();
			int completedTask = -1;

			//Массив будет хранить результаты всех операций
			var taskCompletions = new TaskCompletionSource<TTask>[tasks.Count];
			for (int i = 0; i < taskCompletions.Length; i++)
			{
				taskCompletions[i] = new TaskCompletionSource<TTask>();
			}

			//Partitioner ограничит число одновременных обработчиков (операций)
			//Partitioner.Create(tasks) возвращает обьект, который умеет делитьвходную последовательность для параллельной обработки.
			//Позволяет получить несколько итераторов, которые могут работать параллельно, каждый со своим куском входной последовательности
			foreach (IEnumerator<TItem> partition in System.Collections.Concurrent.Partitioner.Create(tasks).GetPartitions(degreeOfParallelism))
			{
				IEnumerator<TItem> p = partition;

				//Теряем контекст синхронизаций и запускаем обработку каждой партиции асинхронно
				Task.Run(async () =>
					{
						while (p.MoveNext())
						{
							Task<TTask> task = selector(p.Current);

							//Подавляем возможные исключения:
							await task.ContinueWith(_ =>
								{
								});
							//атомарность индекса через семафор:
							int finishedTaskIndex = Interlocked.Increment(ref completedTask);
							taskCompletions[finishedTaskIndex].SetResult(task.Result);//.FromTask(task);
						}
					});
			}

			return taskCompletions.Select(tcs => tcs.Task);
		}

		//При выполнении задачи может возникнуть ошибка, которая будет не видна, но может временно выключить партицию, которая работает с такой задачей.
		//Введя накопитель для ошибок, можем отслеживать состояния разделов. Только вот задачу и ошибку соотнести не выйдет
		public static async Task ForEachAsyncWithExceptions<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
		{
			ConcurrentQueue<Exception> exceptions = null;
			await Task.WhenAll(
				from partition in System.Collections.Concurrent.Partitioner.Create(source).GetPartitions(dop)
				select Task.Run(async delegate
					{
						using (partition)
						{
							while (partition.MoveNext())
							{
								try
								{
									await body(partition.Current);
								}
								catch (Exception e)
								{
									LazyInitializer.EnsureInitialized(ref exceptions).Enqueue(e);
								}
							}
						}
					})
			);

			if (exceptions != null)
			{
				throw new AggregateException(exceptions);
			}
		}
	}

	//Пример использования SelectAsync обработчика:
	public class Test
	{
		private Task<Weather> GetWeatherForAsync(string city)
		{
			//интернет сервис погоды WeatherService:
			return WeatherService.GetWeatherAsync(city);
		}

		public async Task SelectAsyncTest()
		{
			var cities = new List<string> { "Stakgolm", "Bankok" };

			var tasks = cities.SelectAsync(async c => new { City = c, Weather = await GetWeatherForAsync(c) }, 2);
			foreach (var task in tasks)
			{
				var taskResult = await task;
				ProcessWeather(taskResult.City, taskResult.Weather);
			}
		}

		private static void ProcessWeather(string city, Weather weather)
		{
			throw new NotImplementedException();
		}

		public class Weather
		{
			public string City { get; set; }
		}
	}
}
