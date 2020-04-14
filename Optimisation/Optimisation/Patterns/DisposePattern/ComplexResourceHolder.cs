/*
 * Пример реализации паттерна Освобождение ресурсов
 * Идея Dispose паттерна состоит в следующем:
 * давайте мы всю логику освобождения ресурсов поместим в отдельный метод,
 * и будем вызывать его и из метода Dispose(), и из ~финализатора(),
 * при этом, давайте добавим флажок, который бы говорил нам о том, кто вызвал этот метод.
 * bit.ly/DisposePatternDotNet
 */

using System;
using Microsoft.Win32.SafeHandles;

namespace Optimisation.Patterns.DisposePattern
{
	public class ComplexResourceHolder: IDisposable
	{
		//Неуправляемый ресурс
		private IntPtr _buffer;
		//Управляемый ресурс
		private SafeWaitHandle _handle;

		public ComplexResourceHolder()
		{
			//Захват ресурса
			_buffer = AllocateBuffer();
			_handle = new SafeWaitHandle(IntPtr.Zero, true);
		}

		private IntPtr AllocateBuffer()
		{
			throw new NotImplementedException();
		}

		// Для не-sealed классов // Для sealed классов это private void Dispose(bool disposing) {} 
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Освобождаем только управляемые ресурсы
				if (_handle != null)
				{
					_handle.Dispose();
				}
			}
			//Освобождаем неуправляемые ресурсы:
			ReleaseBuffer(_buffer);
		}

		private void ReleaseBuffer(IntPtr buffer)
		{
			throw new NotImplementedException();
		}

		//Финализатор во время сборки мусора
		~ComplexResourceHolder()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); // предотвращает вызов ~финализатора()
		}
	}
}
