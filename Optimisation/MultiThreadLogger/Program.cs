using System;

/*
 * Использование логера из нескольких потоков одновременно.
 * Пример реализаций.
 * https://stackoverflow.com/questions/2954900/simple-multithread-safe-log-class
 */
namespace MultiThreadLogger
{
	class Program
	{
		static void Main(string[] args)
		{
			var logger = new MtLogger();
			Console.WriteLine("Hello World!");
		}
	}
}
