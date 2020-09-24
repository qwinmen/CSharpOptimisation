using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MultiThreadLogger
{
	/*
	 * Here is a sample for a Log implemented with the Producer/Consumer pattern (with .Net 4) using a BlockingCollection.
	 * Он создает поток, который получает команды записи через очередь (BlockingCollection).
	 * Пользователь этого журнала должен просто вызвать Logger.WriteLine (), и журнал будет прозрачно записан асинхронно.
	 * https://stackoverflow.com/questions/2954900/simple-multithread-safe-log-class
	 */
	public class MtLogger: IMtLogger
	{
		BlockingCollection<Param> _bc = new BlockingCollection<Param>();

		//Конструктор создает поток, ожидающий работу с .GetConsumingEnumerable()
		public MtLogger()
		{
			Task.Factory.StartNew(
				() =>
					{
						foreach (Param p in _bc.GetConsumingEnumerable())
						{
							switch (p.Ltype)
							{
								case Param.LogType.Info:
									const string LINE_MSG = "[{0}] {1}";
									Console.WriteLine(String.Format(LINE_MSG, LogTimeStamp(), p.Msg));
									break;
								case Param.LogType.Warning:
									const string WARNING_MSG = "[{3}] * Warning {0} (Action {1} on {2})";
									Console.WriteLine(String.Format(WARNING_MSG, p.Msg, p.Action, p.Obj, LogTimeStamp()));
									break;
								case Param.LogType.Error:
									const string ERROR_MSG = "[{3}] *** Error {0} (Action {1} on {2})";
									Console.WriteLine(String.Format(ERROR_MSG, p.Msg, p.Action, p.Obj, LogTimeStamp()));
									break;
								case Param.LogType.SimpleError:
									const string ERROR_MSG_SIMPLE = "[{0}] *** Error {1}";
									Console.WriteLine(String.Format(ERROR_MSG_SIMPLE, LogTimeStamp(), p.Msg));
									break;
								default:
									Console.WriteLine(String.Format(LINE_MSG, LogTimeStamp(), p.Msg));
									break;
							}
						}
					}
				);
		}

		string LogTimeStamp()
		{
			DateTime now = DateTime.Now;
			return now.ToShortTimeString();
		}

		~MtLogger()
		{
			// Free the writing thread
			_bc.CompleteAdding();
		}

		/*
		 * Просто вызовите этот метод, чтобы что-то записать
		 * (он быстро вернется, потому что просто ставит работу в очередь с помощью bc.Add(p))
		 */
		public void WriteLine(string msg)
		{
			Param p = new Param(Param.LogType.Info, msg);
			_bc.Add(p);
		}

		public void WriteError(string errorMsg)
		{
			Param p = new Param(Param.LogType.SimpleError, errorMsg);
			_bc.Add(p);
		}

		public void WriteError(string errorObject, string errorAction, string errorMsg)
		{
			Param p = new Param(Param.LogType.Error, errorMsg, errorAction, errorObject);
			_bc.Add(p);
		}

		public void WriteWarning(string errorObject, string errorAction, string errorMsg)
		{
			Param p = new Param(Param.LogType.Warning, errorMsg, errorAction, errorObject);
			_bc.Add(p);
		}
	}
}
