using System;
using System.Collections;
using System.Collections.Generic;
using Int32 = System.Int32;

namespace TestAlgPower
{
	class Program
	{
		static void Main(string[] args)
		{
			ValueTypePerfTest();
			ReferenceTypePerfTest();

			Console.ReadLine();
		}

		private static void ValueTypePerfTest()
		{
			const int count = 100000000;
			using (new OperationTimer("List<Int32>"))
			{
				List<Int32> l = new List<int>();
				for (int i = 0; i < count; i++)
				{
					l.Add(i);			//без упаковки
					int x = l[i];		//без распаковки
				}

				l = null;	//для удаления в процессе уборки мусора
			}

			using (new OperationTimer("ArrayList of Int32"))
			{
				ArrayList a = new ArrayList();
				for (int i = 0; i < count; i++)
				{
					a.Add(i);				//Упаковка
					int x = (Int32)a[i];	//Распаковка
				}

				a = null;	//Для удаления в процессе уборки мусора
			}
		}

		private static void ReferenceTypePerfTest()
		{
			const Int32 count = 100000000;
			using (new OperationTimer("List<String>"))
			{
				List<String> l = new List<string>();
				for (int i = 0; i < count; i++)
				{
					l.Add("X");			//Копирование ссылки
					string x = l[i];	//Копирование ссылки
				}

				l = null;	//Для удаления в процессе уборки мусора
			}

			using (new OperationTimer("ArrayList of String"))
			{
				ArrayList a = new ArrayList();
				for (int i = 0; i < count; i++)
				{
					a.Add("X");					//Копирование ссылки
					String x = (String)a[i];	//Проверка преобразования и копирование ссылки
				}

				a = null;	//Для удаления в процессе уборки мусора
			}
		}
	}
}
