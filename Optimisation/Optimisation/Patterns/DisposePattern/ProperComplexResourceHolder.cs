/*
 * Использование шаблонного метода для управления ресурсами
 * bit.ly/DisposePatternDotNet
 */

using System;
using Microsoft.Win32.SafeHandles;

namespace Optimisation.Patterns.DisposePattern
{
	public class ProperComplexResourceHolder: IDisposable
	{
		//Неуправляемый ресурс
		private IntPtr _buffer;
		//Управляемый ресурс
		private SafeWaitHandle _handle;

		public ProperComplexResourceHolder()
		{
			//Захват ресурса
			_buffer = AllocateBuffer();
			_handle = new SafeWaitHandle(IntPtr.Zero, true);
		}

		private IntPtr AllocateBuffer()
		{
			throw new NotImplementedException();
		}

		protected virtual void DisposeNativeResources()
		{
			ReleaseBuffer(_buffer);
		}

		private void ReleaseBuffer(IntPtr buffer)
		{
			throw new NotImplementedException();
		}

		protected virtual void DisposeManagedResources()
		{
			if(_handle != null)
				_handle.Dispose();
		}

		~ProperComplexResourceHolder()
		{
			DisposeNativeResources();
		}

		public void Dispose()
		{
			DisposeNativeResources();
			DisposeManagedResources();
			GC.SuppressFinalize(this);
		}
	}
}
