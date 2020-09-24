using System;

namespace Optimisation.Patterns.DisposePattern
{
	/*
	 * Вариант освобождения ресурсов фабрики через try finally конструкцию
	 * https://stackoverflow.com/questions/20703500/try-finally-block-vs-calling-dispose
	 */
	class ReleaseExample
	{
		private readonly ILogger _logger;
		private readonly ILoggerFactory _loggerFactory;

		public ReleaseExample(
			ILogger logger,
			ILoggerFactory loggerFactory)
		{
			_logger = logger;
			_loggerFactory = loggerFactory;
		}

		private IRequestLogger DbLogger
		{
			get
			{
				IRequestLogger requestLogger = null;
				try
				{
					requestLogger = new RequestLogger(_loggerFactory.Create(_logger));
					return requestLogger;
				}
				finally
				{
					//Сравнение ссылок позволит избежать ситуаций, когда в блоке try произошла ошибка
					//и объект так и не был построен (следовательно и релизить нет необходимости)
					if (!ReferenceEquals(requestLogger, null))
						_loggerFactory.Release(_logger);
				}
			}
		}
	}

	internal class RequestLogger
		: IRequestLogger
	{
		public RequestLogger(ILogger logger)
		{
			throw new NotImplementedException();
		}
	}

	internal interface IRequestLogger
	{
	}

	interface ILoggerFactory
	{
		ILogger Create(ILogger logger);
		void Release(ILogger logger);
	}

	internal interface ILogger
	{
	}
}
