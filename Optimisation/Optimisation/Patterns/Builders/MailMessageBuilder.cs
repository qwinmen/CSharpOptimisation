/*
 * Порождающий паттерн - builder (строитель)
 * Позволяет гибко конфигурировать конечный обьект
 */

using System;
using System.Net.Mail;
using System.Text;

namespace Optimisation.Patterns.Builders
{
	public class MainClass
	{
		public MainClass()
		{
			//Пример использования билдера:
			var mail = new MailMessageBuilder("krop@lol.com")
					.From("lalka@lol.com")
					.Cc("mirro_bk@lol.com")
					.Subject("Пример использования билдера")
					.Body("Построение почтового сообщения", Encoding.UTF8)
					.Build()
				;
			new SmtpClient().Send(mail);
		}
	}

	public sealed class MailMessageBuilder: IMailMessageBuilder
	{
		public MailMessageBuilder()
		{
		}

		//конструктор с обязательными для заполнения полями
		public MailMessageBuilder(string address)
		{
			To(address);
		}

		private readonly MailMessage _mailMessage = new MailMessage();

		public MailMessageBuilder From(string address)
		{
			_mailMessage.From = new MailAddress(address);
			return this;
		}

		public MailMessageBuilder To(string address)
		{
			_mailMessage.To.Add(address);
			return this;
		}

		public MailMessageBuilder Cc(string address)
		{
			_mailMessage.CC.Add(address);
			return this;
		}

		public MailMessageBuilder Subject(string subject)
		{
			_mailMessage.Subject = subject;
			return this;
		}

		public MailMessageBuilder Body(string body, Encoding encoding)
		{
			_mailMessage.Body = body;
			_mailMessage.BodyEncoding = encoding;
			return this;
		}

		public MailMessage Build()
		{
			if (_mailMessage.To.Count == 0)
				throw new InvalidOperationException("Can not create mail message with empty To collection.");

			return _mailMessage;
		}
	}

	public interface IMailMessageBuilder
	{
		MailMessageBuilder From(string address);
		MailMessageBuilder To(string address);
		MailMessageBuilder Cc(string address);
		MailMessageBuilder Subject(string subject);
		MailMessageBuilder Body(string body, Encoding encoding);
		MailMessage Build();
	}
}
