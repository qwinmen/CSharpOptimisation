using System;
using System.Diagnostics;

namespace TestAlgPower
{
	/// <summary>
	/// Класс для оценки времени выполнения операций
	/// </summary>
	internal sealed class OperationTimer: IDisposable
	{
		private Stopwatch m_startTime;
		private String m_text;
		private Int32 m_collectionCount;

		public OperationTimer(string text)
		{
			PrepareForOperation();

			m_text = text;
			m_collectionCount = GC.CollectionCount(0);

			//Эта команда должна быть последней в этом методе
			//Для максимально точной оценки быстродействия
			m_startTime = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			m_startTime.Stop();
			Debug.WriteLine("► {0} (GCs={1}) {2} {3}", m_startTime.Elapsed, GC.CollectionCount(0), m_text, m_collectionCount);
		}

		private static void PrepareForOperation()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}
	
	/*
	//пример использования в коде:
	using(new OperationTimer("GetAll")){
		//тут код операций для измерений
	}
	*/
}
