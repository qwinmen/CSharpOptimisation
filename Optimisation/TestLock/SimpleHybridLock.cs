using System;
using System.Threading;

namespace TestLock
{
	/// <summary>
	/// Пример гибридной блокировки в рамках синхронизации потоков
	/// </summary>
	internal sealed class SimpleHybridLock : IDisposable
	{
		//Int32 используеться примитивными конструкциями пользовательского режима (Interlocked)
		private Int32 m_waiters = 0;

		//AutoResetEvent - примитивная конструкция режима ядра
		private AutoResetEvent m_waiterLock = new AutoResetEvent(false);

		public void Enter()
		{
			//Поток хочет получить блокировку
			if (Interlocked.Increment(ref m_waiters) == 1)
				return; //Блокировка свободна, конкуренций нет, возвращаем управление.

			//Блокировка захвачена другим потоком (конкуренция), приходиться ждать
			m_waiterLock.WaitOne(); //Значительное снижение производительности.
			//Когда WaitOne() возвращает управление, этот поток блокируеться.
		}

		public void Leave()
		{
			//Этот поток освобождает блокировку
			if (Interlocked.Decrement(ref m_waiters) == 0)
				return; //Другие потоки не заблокированы, возвращаем управление.

			//Другие потоки заблокированы, пробуждаем один из них
			m_waiterLock.Set(); //Значительное снижение производительности
		}

		public void Dispose() => m_waiterLock.Dispose();	//Значительное снижение производительности
	}
}
