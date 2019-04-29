using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Belatrix.Logger.DA
{
    public class SQLServerLogger
    {
        private readonly string _databaseConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public int WriteLogInDatabase(string commandText)
        {
            try
            {
                using (var connection = new SqlConnection(_databaseConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    var transaction = connection.BeginTransaction();

                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = commandText;
                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();

                        Console.WriteLine("Log was successfully registered in the database");
                        return rowsAffected;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Commit exception: {ex.Message}");

                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine($"Rollback exception: {ex2.Message}");
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }

        }
    }
}
