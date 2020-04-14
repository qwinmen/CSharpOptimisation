
using System;
using System.Collections;

namespace Optimisation.Patterns.Iterators
{
	class ForeachExample
	{
		/*
		 * Конструкцию
		 * foreach(var i in sequence){ Console.WriteLine(i); }
		 * можно расписать так:
		 */
		public static void ForeachIEnumerable(IEnumerable sequence)
		{
			IEnumerator enumerator = sequence.GetEnumerator();
			object current = null;
			try
			{
				while (enumerator.MoveNext())
				{
					current = enumerator.Current;
					Console.WriteLine(current);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
