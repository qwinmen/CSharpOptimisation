

//https://www.codeproject.com/Tips/737117/Factory-Pattern-Example-in-Csharp
/*
 * Фабрика
 * - шаблон создающего типа (создающие, поведения, структурные).
 * - проблема создания объектов (продуктов) без указания точного класса объекта, который будет создан.
 * - суть шаблона - определить интерфейс для создания объекта, но пусть классы, реализующие интерфейс, решают и уточняют, какой класс создавать.
 * - позволяет классу откладывать создание экземпляров для подклассов.
 */
namespace Optimisation.Patterns
{
	/// <summary>
	///     Обобщеный интерфейс настроек
	/// </summary>
	public interface ISmtpConfiguration
	{
		string PickupDirectory(string name);
	}

	//Конфигурируем  почтовую доставку
	class ApmEmailConfiguration : ISmtpConfiguration
	{
		private readonly string _apmMimeType = "png";

		public string PickupDirectory(string name)
		{
			return name + _apmMimeType;
		}
	}

	//Конфигурируем смс параметры
	class ApmSmsConfiguration : ISmtpConfiguration
	{
		private const string PatchName = "./sms/";

		public string PickupDirectory(string name)
		{
			return name.Contains(PatchName) ? name : PatchName;
		}
	}

	/// <summary>
	///     Фабрика
	/// </summary>
	public class Factory
	{
		public enum SmtpProviderType
		{
			Local = 0,
			Network = 1,
		}

		//Создаем обьект в зависимости от надобности
		public ISmtpConfiguration CreateSmtpConfiguration(SmtpProviderType smtpProviderType)
		{
			switch (smtpProviderType)
			{
				case SmtpProviderType.Local:
					return new ApmEmailConfiguration();
				case SmtpProviderType.Network:
					return new ApmSmsConfiguration();
			}

			return null;
		}
	}

	//Пример использования:
	//ISmtpConfiguration fctry = Factory.CreateSmtpConfiguration(configType);
	//fctry.PickupDirectory("sms")
}