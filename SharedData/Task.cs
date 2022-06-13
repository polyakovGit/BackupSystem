using System;


namespace SharedData
{
    public enum TaskStatus
    {
        New = 0,
        Working,
        Error_NoFile,
        Error_DbConnect,
        Error_Quota,
        Disabled
    }

    public enum TaskAction
    {
        Created = 0,
        Backup,
        Restore
    }

    [Serializable]
    public struct TaskHistory
    {
        public DateTime Date;
        public TaskAction Action;
    }

    [Serializable]
    public abstract class BackupTask
    {
        public int Id = -1; //ID задания
        public List<DateTime> BackupTimes = new List<DateTime>(); //Список времен текущих резервных копий
        public DateTime NextBackupTime = DateTime.MaxValue; //Время следующего резервирования
        public int TypeTimeBackup = 0; //Тип расписания
        public TaskStatus Status = TaskStatus.New; //Статус
        public int MaxCount = 1; //Количество хранимых резервных копий
        public List<TaskHistory> History = new List<TaskHistory>();

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
                TaskStatus.New => "Запланировано",
                TaskStatus.Working => "Выполняется",
                TaskStatus.Error_NoFile => "Файл не найден",
                TaskStatus.Error_DbConnect => "Ошибка подключения",
                TaskStatus.Error_Quota => "Превышение квоты",
                TaskStatus.Disabled => "Отключено",
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

        public void AddAction(TaskAction action) => History.Add(new TaskHistory { Date = DateTime.Now, Action = action });
    }

    [Serializable]
    public class FileBackupTask : BackupTask
    {
        public string FileName = "";
    }

    [Serializable]
    public class DbBackupTask : BackupTask
    {
        public string Server = "";
        public string Login = "";
        public string Password = "";
        public string DbName = "";
    }
}
