namespace MultiThreadLogger
{
	public interface IMtLogger
	{
		void WriteLine(string msg);
		void WriteError(string errorMsg);
		void WriteError(string errorObject, string errorAction, string errorMsg);
		void WriteWarning(string errorObject, string errorAction, string errorMsg);
	}
}
