using System;
using System.IO;

namespace rail.Models
{
    public class LogFileManager
    {
        private readonly string logFilePath;
        private readonly int cleanUpIntervalDays = 20;

        public LogFileManager()
        {
            // Путь к файлу логов (в папке AppData)
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "MyAppLogs");
            Directory.CreateDirectory(appFolder); // Создаем папку, если ее нет

            logFilePath = Path.Combine(appFolder, "TransferLogs.txt");
            EnsureLogFileExists();
        }

        public string GetPathFolder()
        {
            return Path.GetDirectoryName(logFilePath);
        }

        // Проверка и создание файла, если его нет
        private void EnsureLogFileExists()
        {
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose(); // Создаем файл и закрываем поток
            }
        }

        // Добавление записи в лог
        public void AddLog(string message)
        {
            EnsureLogFileExists(); // Проверяем существование файла
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

        // Очистка файла, если прошло cleanUpIntervalDays дней
        public void CleanUpLogsIfNecessary()
        {
            EnsureLogFileExists();

            FileInfo logFileInfo = new FileInfo(logFilePath);
            if (logFileInfo.CreationTime.AddDays(cleanUpIntervalDays) <= DateTime.Now)
            {
                File.WriteAllText(logFilePath, string.Empty); // Очищаем файл
                File.SetCreationTime(logFilePath, DateTime.Now); // Обновляем дату создания
            }
        }
    }
}
