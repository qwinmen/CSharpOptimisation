using System;
using System.Diagnostics;
using System.Threading;

namespace TestLock.Volatiles
{
	/// <summary>
	/// При компиляции возможна оптимизация кода, в результате которой Worker выполнит while() бесконечно
	/// </summary>
	internal static class StrangeBehavior
	{
		private static volatile Boolean s_stopWorker = false;

		public static void LocalMain()
		{
			Debug.WriteLine("LocalMain: запустить поток метода Worker на 5 секунд.");
			Thread t = new Thread(Worker);
			t.Start();
			Thread.Sleep(5000);
			s_stopWorker = true;
			Debug.WriteLine("LocalMain: поток ожидает завершения метода Worker.");
			t.Join();
		}

		private static void Worker(Object o)
		{
			Int32 x = 0;
			while (!s_stopWorker)
			{
				x++;
			}

			Debug.WriteLine("Worker: остановлен при х={0}", x);
		}
	}
}
