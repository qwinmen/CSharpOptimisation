using System.Threading;

namespace TestLock.SingletonLocs
{
	/// <summary>
	///     Потокобезопасный метод с блокировкой в режиме пользователя
	/// </summary>
	internal sealed class SingletonLightLoc
	{
		private static SingletonLightLoc s_value = null;

		private SingletonLightLoc()
		{
		}

		public static SingletonLightLoc GetSingletonLightLoc()
		{
			if (s_value != null)
				return s_value;

			SingletonLightLoc temp = new SingletonLightLoc();

			//Если s_value == null, то сделать s_value=temp:
			Interlocked.CompareExchange(ref s_value, temp, null);
			
			//При потере этого потока, обьект temp утилизируеться уборщиком мусора
			return s_value;
		}
	}
}