/*
 * Пример реализации паттерна Посредник (mediator)
 * Позволяет ограничивать изменения в одной части, не давая им влиять на остальные.
 */

namespace Optimisation.Patterns.Mediators
{
	//1. Неявная реализация посредника
	class LogMediator
	{
		//посредник, знающий о том, кто занимается чтением
		//а кто - сохранением
		private FileRiader _fileRiader;
		private FileSaver _fileSaver;
	}

	//2. Явная реализация посредника
	class LogSoMediator
	{
		//посредник знает об обоих участниках
		private FileRiader _fileRiader;
		private FileSaver _fileSaver;
	}

	class FileRiader
	{
		//обьект для чтения
		//участник знающий о явном медиаторе
		private LogSoMediator _logSoMediator;
	}

	class FileSaver
	{
		//обьект для сохранения
		//участник знающий о явном медиаторе
		private LogSoMediator _logSoMediator;
	}
}
