using System;
using System.Threading;

namespace TestLock.SingletonLocs
{
	/// <summary>
	///     Блокировка с двойной проверкой для одноэлементного обьекта (singleton)
	/// </summary>
	internal sealed class SingletonDoubleLoc
	{
		//Обьект s_lock требуеться для обеспечения безопасности в многопоточной среде,
		//Наличие этого обьекта предполагает, что для создания одноэлементного обьекта требуеться больше ресурсов,
		//чем для обьекта System.Object и что эта процедура может вовсе не понадобиться.
		//В противном случае проще и эффективнее получить одноэлементный обьект в конструкторе класса
		private static readonly Object s_lock = new Object();

		//Это поле ссылаеться на один обьект Singleton
		private static SingletonDoubleLoc s_value = null;

		//Закрытый конструктор не дает внешнему коду создавать экземпляры
		private SingletonDoubleLoc()
		{
		}

		//Открытый статический метод, возвращающий обьект Singleton (создавая его при необходимости)
		public static SingletonDoubleLoc GetSingletonDoubleLoc()
		{
			//если обьект уже создан, возвращаем его
			if (s_value != null)
				return s_value;

			//Если обьект еще не создан, разрешаем одному потоку создание
			Monitor.Enter(s_lock);
			if (s_value == null)
			{
				//если обьекта все еще нет, создаем его
				SingletonDoubleLoc temp = new SingletonDoubleLoc();
				//Сохраняем ссылку в переменной s_value
				Volatile.Write(ref s_value, temp);
			}

			Monitor.Exit(s_lock);

			//Возвращаем ссылку на обьект Singleton
			return s_value;
		}
	}
}