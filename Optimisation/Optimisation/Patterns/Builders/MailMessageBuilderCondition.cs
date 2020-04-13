/*
 * Порождающий паттерн - builder (строитель)
 * Позволяет гибко конфигурировать конечный обьект
 * Вариант строителя с использованием состояний
 */

using System.Net.Mail;
using System.Text;

namespace Optimisation.Patterns.Builders
{
	public class MainClassCondition
	{
		//пример использования
		void Test()
		{
			//чтобы вызвать Build, нужен тип-состояние FinalMailMessageBuilder
			//тип-состояние FinalMailMessageBuilder получается только через обязательный метод To()
			FinalMailMessageBuilder mail = new MailMessageBuilderCondition()
					.Subject("Пример использования билдера")
					.Body("Построение почтового сообщения", Encoding.UTF8)
					.To("krop@lol.com")
				;
			MailMessage mailMessage = mail.Build();
			new SmtpClient().Send(mailMessage);
		}
	}

	public class MailMessageBuilderCondition: IMailMessageBuilderCondition
	{
		private readonly MailMessage _mailMessage;

		public MailMessageBuilderCondition()
		{
			_mailMessage = new MailMessage();
		}

		//Обязательный шаг реализован через состояние FinalMailMessageBuilder
		public FinalMailMessageBuilder To(string address)
		{
			_mailMessage.To(address);
			return new FinalMailMessageBuilder(_mailMessage);
		}

		public MailMessageBuilderCondition Subject(string subject)
		{
			_mailMessage.Subject = subject;
			return this;
		}

		public MailMessageBuilderCondition Body(string body, Encoding encoding)
		{
			_mailMessage.Body = body;
			_mailMessage.BodyEncoding = encoding;
			return this;
		}
	}

	public interface IMailMessageBuilderCondition
	{
		FinalMailMessageBuilder To(string address);
		MailMessageBuilderCondition Subject(string subject);
		MailMessageBuilderCondition Body(string body, Encoding encoding);
	}

	public class FinalMailMessageBuilder
	{
		private readonly MailMessage _mailMessage;

		public FinalMailMessageBuilder(MailMessage mailMessage)
		{
			_mailMessage = mailMessage;
		}

		public MailMessage Build()
		{
			return _mailMessage;
		}
	}
}
