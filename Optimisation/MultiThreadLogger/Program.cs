using System;
using System.Diagnostics;
using System.Threading;

/*
 * Использование логера из нескольких потоков одновременно.
 * Пример реализаций.
 * https://stackoverflow.com/questions/2954900/simple-multithread-safe-log-class
 */
namespace MultiThreadLogger
{
	class Program
	{
		static MtLogger _logger = new MtLogger();

		static void Main(string[] args)
		{
			int t = 2;
			Thread[] ts = new Thread[t];
			for (int i = 0; i < t; i++)
			{
				ts[i] = new Thread(() =>
					{
						int temp = i;
						CallService(temp);
					});
				ts[i].Start();
			}

			for (int i = 0; i < t; i++)
			{
				ts[i].Join();
			}

			Debug.WriteLine("# Hello World!");
			Console.Read();
		}

		public static void CallService(int i)
		{
			string msg = "# ServiceCall::" + i;
			Debug.WriteLine(msg);
			_logger.WriteLine(msg);
			ReadRooms(i);
		}

		public static void ReadRooms(int i)
		{
			string msg = "# Reading Room::" + i;
			Debug.WriteLine(msg);
			_logger.WriteLine(msg);

			Thread.Sleep(2000);
		}
	}
}
