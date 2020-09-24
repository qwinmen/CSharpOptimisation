namespace MultiThreadLogger
{
	internal class Param
	{
		internal enum LogType
		{
			Info,
			Warning,
			Error,
			SimpleError
		};

		internal Param()
		{
			Ltype = LogType.Info;
			Msg = "";
		}

		internal Param(LogType logType, string logMsg)
		{
			Ltype = logType;
			Msg = logMsg;
		}

		internal Param(LogType logType, string logMsg, string logAction, string logObj)
		{
			Ltype = logType;
			Msg = logMsg;
			Action = logAction;
			Obj = logObj;
		}

		internal LogType Ltype { get; set; } // Type of log
		internal string Msg { get; set; } // Message
		internal string Action { get; set; } // Action when error or warning occurs (optional)
		internal string Obj { get; set; } // Object that was processed whend error or warning occurs (optional)
	}
}
