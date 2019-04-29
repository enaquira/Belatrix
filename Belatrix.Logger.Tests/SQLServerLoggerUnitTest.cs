using Belatrix.Logger.DA;
using Belatrix.Logger.Enums;
using Belatrix.Logger.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Belatrix.Logger.Tests
{
    [TestClass]
    public class SQLServerLoggerUnitTest
    {

        [TestMethod]
        public void ShouldNotInsertLogToDatabase()
        {
            //Arrange
            var logToFile = false;
            var logToConsole = false;
            var logToDatabase = true;
            var expectedRowsAffected = 0;

            //Act
            SQLServerLogger sqlServerLogger = new SQLServerLogger();
            var command = $"INSERT INTO LOG VALUES('{Constants.TestingMessageLog}', {LogType.Message})";
            var rowsAffected = sqlServerLogger.WriteLogInDatabase(command);

            //Assert
            Assert.AreEqual(expectedRowsAffected, rowsAffected);
        }
    }
}
