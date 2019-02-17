﻿using System.Threading;
using System.Threading.Tasks;

namespace TaskFuncLogger.Helpers
{
	public static class TaskHelpers
	{
		/// <summary>
		/// Метод расширения задания, позволяющий выполнить отмену затянувшейся операции.
		/// </summary>
		/// <param name="originalTask">Задание для проверки</param>
		/// <param name="ct">Ключ отмены задания</param>
		/// <returns>Вернуть исключение OperationCancelledException для завершенной задачи или проигнорировать в случае уже выполненого задания.</returns>
		public static async Task WithCancellation(this Task originalTask, CancellationToken ct)
		{
			//Создание обьекта Task, завершаемого при отмене CancellationToken
			var cancelTask = new TaskCompletionSource<Void>();

			//При отмене CancellationToken завершить Task
			using (ct.Register(t => ((TaskCompletionSource<Void>) t).TrySetResult(new Void()), cancelTask))
			{
				//Создание обьекта Task, завершаемого при отмене исходного обьекта Task или обьекта Task от CancellationToken
				var any = await Task.WhenAny(originalTask, cancelTask.Task);

				//Если какой-либо обьект Task завершаеться из-за CancellationToken, инициировать OperationCancelledException
				if (any == cancelTask.Task)
					ct.ThrowIfCancellationRequested();
			}
		}

		/// <summary>
		/// Метод расширения задания, позволяющий выполнить отмену затянувшейся операции.
		/// </summary>
		/// <typeparam name="TResult">Результирующие данные задания</typeparam>
		/// <param name="originalTask">Задание для проверки</param>
		/// <param name="ct">Ключ отмены задания</param>
		/// <returns>Вернуть исключение OperationCancelledException для завершенной задачи или в случае уже выполненого задания вернуть результат.</returns>
		public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> originalTask, CancellationToken ct)
		{
			//Создание обьекта Task, завершаемого при отмене CancellationToken
			var cancelTask = new TaskCompletionSource<Void>();

			//При отмене CancellationToken завершить Task
			using (ct.Register(t => ((TaskCompletionSource<Void>) t).TrySetResult(new Void()), cancelTask))
			{
				//Создание обьекта Task, завершаемого при отмене исходного обьекта Task или обьекта Task от CancellationToken
				var any = await Task.WhenAny(originalTask, cancelTask.Task);

				//Если какой-либо обьект Task завершаеться из-за CancellationToken, инициировать OperationCancelledException
				if (any == cancelTask.Task)
					ct.ThrowIfCancellationRequested();
			}

			//Выполнить await для исходного задания (синхронно). Если произойдет ошибка, выдать первое внутренее исключение вместо AggregateException
			return await originalTask;
		}

		//Из-за отсутствия необобщенного класса TaskCompletionSource
		private struct Void
		{
		}
	}
}