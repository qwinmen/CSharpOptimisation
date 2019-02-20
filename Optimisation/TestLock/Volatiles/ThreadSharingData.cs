using System;
using System.Diagnostics;

namespace TestLock.Volatiles
{
	internal sealed class ThreadSharingData
	{
		private volatile Int32 m_flag = 0;
		private Int32 m_value = 0;

		//Этот метод исполняеться потоком А
		public void ThreadA()
		{
			//Значение 5 должно быть записано в m_value ПЕРЕД записью 1 в m_flag
			m_value = 5;
			m_flag = 1;
		}

		//Этот метод исполняеться потоком Б
		public void ThreadB()
		{
			//Поле m_value должно быть прочитано после m_flag
			if(m_flag == 1)
				Debug.WriteLine(m_value);
		}
	}
}
