using Belatrix.Logger.DA;
using Belatrix.Logger.Enums;
using Belatrix.Logger.Helper;
using System;
using System.Configuration;

namespace Belatrix.Logger
{
    public class JobLogger
    {
        private readonly bool _logToFile;
        private readonly bool _logToConsole;
        private readonly bool _logToDatabase;
        private SQLServerLogger sqlServerLogger = new SQLServerLogger();
        private FileLogger fileLogger = new FileLogger();

        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase)
        {
            _logToFile = logToFile;
            _logToConsole = logToConsole;
            _logToDatabase = logToDatabase;
        }

        public void LogMessage(string message, LogType logType)
        {
            message = message.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new Exception(Constants.NullOrEmptyMessageError);
            }

            if (!_logToConsole && !_logToFile && !_logToDatabase)
            {
                throw new Exception(Constants.InvalidConfigurationError);
            }

            switch (logType)
            {
                case LogType.Message:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            var today = DateTime.Now.ToString("yyyy-MM-dd");

            if (_logToFile)
            {
                LogToFile(message, today);
            }

            if (_logToConsole)
            {
                Console.WriteLine($"{today} Log: {message}");
            }

            if (_logToDatabase)
            {
                LogToDatabase(message, logType);
            }
        }

        private void LogToDatabase(string message, LogType logType)
        {
            var command = $"INSERT INTO LOG VALUES('{message}', {logType})";
            sqlServerLogger.WriteLogInDatabase(command);
        }

        private void LogToFile(string message, string today)
        {
            var logDirectory = ConfigurationManager.AppSettings["LogFileDirectory"];
            fileLogger.WriteLogInFile(logDirectory, today, message);
        }
    }
}
