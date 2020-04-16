/*
 * Пример реализации паттерна Итератор (iterator)
 * Внутренний итератор - когда в итератор передается метод обратного вызова через который
 * возможно уведомление клиента о следующем элементе
 */

using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Optimisation.Patterns.Iterators
{
	class PushBasedIterator
	{
		void Example()
		{
			var list = new List<int>{ 1, 2, 3 };
			IObservable<int> observable = list.ToObservable();
			observable.Subscribe(
				onNext: n => Console.WriteLine("Processiong: {0}", n),
				onCompleted: () => Console.WriteLine("Sequence finished.")
			);
		}
	}
}
