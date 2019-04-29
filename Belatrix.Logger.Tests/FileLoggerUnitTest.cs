using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Belatrix.Logger;
using System.Configuration;
using Belatrix.Logger.Helper;
using Belatrix.Logger.Enums;

namespace Belatrix.Logger.Tests
{
    [TestClass]
    public class FileLoggerUnitTest
    {
        private string logDirectory = ConfigurationManager.AppSettings["LogFileDirectory"];
        private FileLogger fileLogger = new FileLogger();

        [TestMethod]
        public void ShouldLogMessage()
        {
            //Arrange
            var logToFile = true;
            var logToConsole = false;
            var logToDatabase = false;
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedMessage = $"\n{today}_Log: {Constants.TestingMessageLog}";
            var jobLogger = new JobLogger(logToFile, logToConsole, logToDatabase);

            //Act
            jobLogger.LogMessage(Constants.TestingMessageLog, LogType.Message);
            var message = fileLogger.ReadFile(logDirectory, today);

            //Assert
            Assert.AreEqual(expectedMessage, message);
        }

        [TestMethod]
        public void ShouldLogWarning()
        {
            //Arrange
            var logToFile = true;
            var logToConsole = false;
            var logToDatabase = false;
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedMessage = $"\n{today}_Log: {Constants.TestingWarningLog}";
            var jobLogger = new JobLogger(logToFile, logToConsole, logToDatabase);
            

            //Act
            jobLogger.LogMessage(Constants.TestingWarningLog, LogType.Warning);
            var message = fileLogger.ReadFile(logDirectory, today);

            //Assert
            Assert.AreEqual(expectedMessage, message);
        }

        [TestMethod]
        public void ShouldLogError()
        {
            //Arrange
            var logToFile = true;
            var logToConsole = false;
            var logToDatabase = false;
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedMessage = $"\n{today}_Log: {Constants.TestingErrorLog}";
            var jobLogger = new JobLogger(logToFile, logToConsole, logToDatabase);
            
            //Act
            jobLogger.LogMessage(Constants.TestingErrorLog, LogType.Error);
            var message = fileLogger.ReadFile(logDirectory, today);

            //Assert
            Assert.AreEqual(expectedMessage, message);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            fileLogger.CleanFile(logDirectory, today);
        }
    }
}
