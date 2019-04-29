using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belatrix
{
    public class JobLogger
    {
        private static bool _logToFile;
        private static bool _logToConsole;

        //La campo no cumple con la regla sugerida por Microsoft de tener el prefijo para campos  "_". Se sugiere que sea _logToDataBase
        private static bool LogToDatabase;

        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;

        //El campo _initialized no es usado
        private bool _initialized;

        //Exceso de parámetros
        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            LogToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
        }

        //El nombre del parámetro está duplicado. Provocará error de compilación.
        //Los parámetros 'message', 'warning' y 'error' pueden ir como uno solo.
        public static void LogMessage(string message, bool message, bool warning, bool error)
        {
            //El trim reemplaza el mismo mensaje por si mismo, si es que no se establece a la misma variable: message = message.Trim();
            message.Trim();

            //Se recomienda para estos casos usar la función predeterminada de IsNullOrWhiteSpace
            if (message == null || message.Length == 0)
            {
                return;
            }

            if (!_logToConsole && !_logToFile && !LogToDatabase)
            {
                //Los 'magic strings' deben reemplazarse por Constantes o ser establecidos en archivos de configuración
                throw new Exception("Invalid configuration");
            }
            if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && !error))
            {
                //Los 'magic strings' deben reemplazarse por Constantes o ser establecidos en archivos de configuración
                throw new Exception("Error or Warning or Message must be specified");
            }

            //La lógica de conexión a datos debe ser separada en otra capa.
            System.Data.SqlClient.SqlConnection connection = new
            System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();

            //La variable no ha sido inicializada
            int t;

            //Las condicionales podrían convertirse en un switch y los booleanos podrían ir como un solo parámetro de tipo Enum
            if (message && _logMessage)
            {
                t = 1;
            }
            if (error && _logError)
            {
                t = 2;
            }
            if (warning && _logWarning)
            {
                t = 3;
            }

            //No se realiza control de excepciones.
            //Se debe separar la lógica de base de datos en otra capa.
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")");
            command.ExecuteNonQuery();

            //La variable no ha sido inicializada
            string l;

            //La validación es incorrecta, valida si es que el archivo no existe. No hay control de errores o excepciones
            //Se debe separar la lógica del Logger en archivos en otra capa.
            //Al momento de leer el archivo ocurrirá error por el formato de fecha "ShortDateString" ya que incluirá '/'.
            if (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                l = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
            }

            //Se realizan validaciones, sin embargo el resultado es el mismo para las 3 condicionales
            if (error && _logError)
            {
                l = l + DateTime.Now.ToShortDateString() + message;
            }
            if (warning && _logWarning)
            {
                l = l + DateTime.Now.ToShortDateString() + message;
            }
            if (message && _logMessage)
            {
                l = l + DateTime.Now.ToShortDateString() + message;
            }

            System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);
            //Se puede reemplazar por un switch-case y se hacen validaciones dobles innecesariamente.
            if (error && _logError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (warning && _logWarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (message && _logMessage)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            //Se puede mejorar el formato.
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }
    }
}
