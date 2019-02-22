namespace TestLock.SingletonLocs
{
	/// <summary>
	///     Более простая версия обьекта без блокировки и потокобезопасная
	/// </summary>
	internal class Singleton
	{
		private static readonly Singleton s_value = new Singleton();

		private Singleton()
		{
		}

		public static Singleton GetSingleton()
		{
			return s_value;
		}
	}
}