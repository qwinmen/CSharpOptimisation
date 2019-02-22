using System;
using TestLock.LazyInitialize;
using TestLock.Volatiles;

namespace TestLock
{
	public class Program
	{
		public static void Main(string[] args)
		{
			StrangeBehavior.LocalMain();
			LocSpeedTest.LocalMain();
			LazyPattern.LocalMain();
			LazyPattern.LocalMainEnsure();

			Console.ReadLine();
		}
	}
}