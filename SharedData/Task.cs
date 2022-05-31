using System;


namespace SharedData
{
    public enum TaskStatus
    {
        New = 0,
        Working,
        Error_NoFile
    }

    [Serializable]
    public class BackupTask
    {
        public string FileName = "";
        public DateTime LastBackupTime =  DateTime.MinValue;
        public DateTime NextBackupTime = DateTime.MaxValue;
        public int TypeTimeBackup = 0;
        public TaskStatus Status = TaskStatus.New;

        public void UpdateNextBackupTime()
        {
            NextBackupTime = TypeTimeBackup switch
            {
                0 => NextBackupTime.AddDays(1), // Ежедневно
                1 => NextBackupTime.AddDays(1), // Еженедельно
                2 => NextBackupTime.AddMonths(1), //Ежемесячно
                _ => DateTime.MaxValue //Ошибочный тип
            };
        }

        public string GetStatusString()
        {
            return Status switch
            {
                TaskStatus.New => "Новая",
                TaskStatus.Working => "Выполняется",
                TaskStatus.Error_NoFile => "Файл не найден",
                _ => "-"
            };
        }

        public string GetScheduleString()
        {
            return TypeTimeBackup switch
            {
                0 => "Ежедневно",
                1 => "Еженедельно",
                2 => "Ежемесячно",
                _ => "-"
            };
        }
    }
}
