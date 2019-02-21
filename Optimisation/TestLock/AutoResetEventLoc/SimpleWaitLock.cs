using System;
using System.Threading;

namespace TestLock.AutoResetEventLoc
{
	internal sealed class SimpleWaitLock : IDisposable
	{
		private readonly AutoResetEvent m_available; //Событие с автосбросом

		public SimpleWaitLock()
		{
			m_available = new AutoResetEvent(true); //Изначально свободен
		}

		public void Dispose() => m_available.Dispose();

		/// <summary>
		///     Вызов метода заставляет выполнить переход из управляемого кода в ядро
		/// </summary>
		public void Enter()
		{
			//Блокирование на уровне ядра до освобождения ресурса
			m_available.WaitOne();
		}

		/// <summary>
		///     Вызов метода заставляет выполнить переход из ядра в управляемый код
		/// </summary>
		public void Leave()
		{
			//Позволяет другому потоку обратиться к ресурсу
			m_available.Set();
		}
	}
}