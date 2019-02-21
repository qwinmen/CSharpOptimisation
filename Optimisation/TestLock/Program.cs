using System;
using TestLock.Volatiles;

namespace TestLock
{
	public class Program
	{
		public static void Main(string[] args)
		{
			StrangeBehavior.LocalMain();
			LocSpeedTest.LocalMain();
			Console.ReadLine();
		}
	}
}