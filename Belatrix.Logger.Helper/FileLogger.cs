using System;
using System.IO;

namespace Belatrix.Logger.Helper
{
    public class FileLogger
    {
        public void WriteLogInFile(string fileDirectory, string today, string message)
        {
            var text = string.Empty;
            var filePath = $"{fileDirectory}Log{today}.txt";

            if (!Directory.Exists(fileDirectory))
            {
                try
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                catch (IOException ex)
                {
                    throw new Exception("Error trying to create file directory.\nException: " + ex);
                }
            }

            if (File.Exists(filePath))
            {
                text = File.ReadAllText(filePath);
            }

            text = $"{text}\n{today}_Log: {message}";

            File.WriteAllText(filePath, text);
        }

        public string ReadFile(string fileDirectory, string today)
        {
            var filePath = $"{fileDirectory}Log{today}.txt";
            return File.ReadAllText(filePath);
        }

        public void CleanFile(string fileDirectory, string today)
        {
            var filePath = $"{fileDirectory}Log{today}.txt";
            File.WriteAllText(filePath, string.Empty);
        }
    }
}
