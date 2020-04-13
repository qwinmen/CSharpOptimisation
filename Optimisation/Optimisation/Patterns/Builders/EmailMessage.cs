/*
 * Порождающий паттерн - builder (строитель)
 * Позволяет гибко конфигурировать конечный обьект
 * Вариант типа с закрытыми полями только для чтения,
 * устанавливать значения можно через внутренний билдер типа
 */

namespace Optimisation.Patterns.Builders
{
	public class Test
	{
		//Пример использования
		private void MainTest()
		{
			EmailMessage mail = EmailMessage.With()
					.From("one@home.com")
					.To("support@one.com")
					.Subject("Пример построения через внутренний билдер")
					.Body("rofl")
					.Build()
				;
		}
	}

	public class EmailMessage
	{
		public EmailMessage()
		{
		}

		public static InnerMailBuilder With()
		{
			return new InnerMailBuilder(new EmailMessage());
		}

		public string To { get; private set; }
		public string From { get; private set; }
		public string Subject { get; private set; }
		public string Body { get; private set; }

		public class InnerMailBuilder
		{
			private readonly EmailMessage _emailMessage;

			public InnerMailBuilder(EmailMessage emailMessage)
			{
				_emailMessage = emailMessage;
			}

			public InnerMailBuilder To(string to)
			{
				_emailMessage.To = to;
				return this;
			}

			public InnerMailBuilder From(string from)
			{
				_emailMessage.From = from;
				return this;
			}

			public InnerMailBuilder Subject(string subject)
			{
				_emailMessage.Subject = subject;
				return this;
			}

			public InnerMailBuilder Body(string body)
			{
				_emailMessage.Body = body;
				return this;
			}

			public EmailMessage Build()
			{
				return _emailMessage;
			}
		}
	}
}
