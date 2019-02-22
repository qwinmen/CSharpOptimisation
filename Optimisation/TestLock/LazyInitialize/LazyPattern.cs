using System;
using System.Diagnostics;
using System.Threading;

namespace TestLock.LazyInitialize
{
	public class LazyPattern
	{
		public static void LocalMain()
		{
			Lazy<String> s = new Lazy<string>(() => DateTime.Now.ToLongTimeString(),
				LazyThreadSafetyMode.PublicationOnly);
			Debug.WriteLine("Обьект в lazy инициализирован? " + s.IsValueCreated);
			Debug.WriteLine("Инициализируем (запрашиваем) обьект " + s.Value);
			Debug.WriteLine("А теперь обьект в lazy инициализирован? " + s.IsValueCreated);

			Thread.Sleep(10000);
			Debug.WriteLine("Прошло 10 секунд.");
			Debug.WriteLine("Значение обьекта (времени) осталось равным первой инициализации (обращению) " + s.Value);
		}

		/*
		 * LazyThreadSafeMode перечисление:
		 * None - безопасность в отношении потоков не поддерживаеться (хорошо для GUI приложений)
		 * ExecutionAndPublication - используеться блокировка с двойной проверкой (подобие SingletonDoubleLoc)
		 * PublicationOnly - используеться метод режима пользователя Interlocked.CampareExchange
		 */

		public static void LocalMainEnsure()
		{
			String name = null;
			//Т.к. имя равно null, запускаеться делегат и инициализирует поле имени
			LazyInitializer.EnsureInitialized(ref name, () => "Jeffery");
			Debug.WriteLine(name); //Выводит "Jeffery"

			//Т.к. имя отлично от null, делегат не выполниться и имя не измениться
			LazyInitializer.EnsureInitialized(ref name, () => "Rither");
			Debug.WriteLine(name); //Выводит "Jeffery"
		}
	}
}