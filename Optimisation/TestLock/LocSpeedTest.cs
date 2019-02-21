using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using TestLock.AutoResetEventLoc;

namespace TestLock
{
	/// <summary>
	/// Тест скорости выполнения инкремента для разных блокировок
	/// </summary>
	public static class LocSpeedTest
	{
		public static void LocalMain()
		{
			Int32 x = 0;
			const Int32 iterations = 10000000;
			//Сколько времени займет инкремент 10 млн раз?
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				x++;
			}

			Debug.WriteLine("1) Incrementing x: {0:N0}", sw.ElapsedMilliseconds);

			//Сколько времени займет инкремент 10 млн раз, если добавить вызов пустого метода:
			sw.Restart();
			for (int i = 0; i < iterations; i++)
			{
				M();
				x++;
				M();
			}

			Debug.WriteLine("2) Incrementing M(): {0:N0}", sw.ElapsedMilliseconds);

			//Сколько времени займет инкремент 10 млн раз если добавить вызов неконкурирующего обьекта SimpleSpinLock:
			SpinLock sl = new SpinLock(false);
			sw.Restart();
			for (int i = 0; i < iterations; i++)
			{
				Boolean taken = false;
				sl.Enter(ref taken);
				x++;
				sl.Exit();
			}

			Debug.WriteLine("3) Incrementing in SpinLock: {0:N0}", sw.ElapsedMilliseconds);

			//Сколько времени займет инкремент 10 млн раз если добавить вызов неконкурирующего обьекта SimpleWaitLock:
			using (SimpleWaitLock swl = new SimpleWaitLock())
			{
				sw.Restart();
				for (int i = 0; i < iterations; i++)
				{
					swl.Enter();
					x++;
					swl.Leave();
				}

				Debug.WriteLine("4) Incrementing in SimpleWaitLock: {0:N0}", sw.ElapsedMilliseconds);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void M()
		{
			//этот метод только возвращает управление
		}
	}
}