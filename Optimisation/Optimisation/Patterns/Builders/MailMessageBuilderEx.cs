/*
 * Порождающий паттерн - builder (строитель)
 * Позволяет гибко конфигурировать конечный обьект
 * Вместо использования обьекта MailMessageBuilder можно применить методы расширения
 * над существующим типом.
 */

using System.Net.Mail;
using System.Text;

namespace Optimisation.Patterns.Builders
{
	public class MainClassEx
	{
		public MainClassEx()
		{
			//Пример использования билдера:
			var mail = new MailMessage()
					.From("lalka@lol.com")
					.To("krop@lol.com")
					.Cc("mirro_bk@lol.com")
					.Subject("Пример использования билдера")
					.Body("Построение почтового сообщения", Encoding.UTF8)
				;
			new SmtpClient().Send(mail);
		}
	}

	public static class MailMessageBuilderEx
	{
		public static MailMessage From(this MailMessage mailMessage, string address)
		{
			mailMessage.From = new MailAddress(address);
			return mailMessage;
		}

		public static MailMessage To(this MailMessage mailMessage, string address)
		{
			mailMessage.To.Add(address);
			return mailMessage;
		}

		public static MailMessage Cc(this MailMessage mailMessage, string address)
		{
			mailMessage.CC.Add(address);
			return mailMessage;
		}

		public static MailMessage Subject(this MailMessage mailMessage, string subject)
		{
			mailMessage.Subject = subject;
			return mailMessage;
		}

		public static MailMessage Body(this MailMessage mailMessage, string body, Encoding encoding)
		{
			mailMessage.Body = body;
			mailMessage.BodyEncoding = encoding;
			return mailMessage;
		}
	}
}
