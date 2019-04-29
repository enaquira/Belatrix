using Belatrix.Logger.Enums;
using Belatrix.Logger.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Belatrix.Logger.Tests
{
    [TestClass]
    public class ExceptionsUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldReturnExceptionWhenEmptyMessage()
        {
            //Arrange
            var logToFile = false;
            var logToConsole = true;
            var logToDatabase = false;
            var jobLogger = new JobLogger(logToFile, logToConsole, logToDatabase);

            //Act
            jobLogger.LogMessage(string.Empty, LogType.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldReturnExceptionWhenInvalidConfiguration()
        {
            //Arrange
            var logToFile = false;
            var logToConsole = false;
            var logToDatabase = false;
            var jobLogger = new JobLogger(logToFile, logToConsole, logToDatabase);

            //Act
            jobLogger.LogMessage(Constants.TestingMessageLog, LogType.Message);
        }
    }
}
