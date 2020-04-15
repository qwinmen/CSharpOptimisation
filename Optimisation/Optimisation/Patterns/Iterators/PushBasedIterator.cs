/*
 * Пример реализации паттерна Итератор (iterator)
 * Внутренний итератор - когда в итератор передается метод обратного вызова через который
 * возможно уведомление клиента о следующем элементе
 */

using System;
using System.Collections.Generic;

namespace Optimisation.Patterns.Iterators
{
	class PushBasedIterator
	{
		void Example()
		{
			var list = new List<int>{ 1, 2, 3 };
			IObservable<int> observable = list.ToObservable();
			IObserver<int> observer = null;
			observable.Subscribe(observer);
		}

		
	}

	static class Help
	{
		public static IObservable<int> ToObservable(this List<int> list)
		{
			return null;
		}
	}
}
