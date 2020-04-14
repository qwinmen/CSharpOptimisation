/*
 * Пример реализации паттерна Итератор (iterator)
 * Отлично подходит для чтения данных из некоторого источника.
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Optimisation.Patterns.TemplateMethods;

namespace Optimisation.Patterns.Iterators
{
	class LogFileSource: IEnumerable<LogEntry>
	{
		private readonly string _logFileName;

		public LogFileSource(string logFileName)
		{
			_logFileName = logFileName;
		}

		public IEnumerator<LogEntry> GetEnumerator()
		{
			foreach (string line in File.ReadAllLines(_logFileName))
			{
				yield return LogEntry.Parse(line);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	static class CustomEnumerator
	{
		//Пример итератора массива:
		public static IEnumerator<int> CustomArrayEnumerator(this int[] array)
		{
			foreach (int i in array)
			{
				yield return i;
			}
		}
	}
}
