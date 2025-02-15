namespace Test_assignment.Models;

public class Meeting
{
    private static int _idCounter = 1;
    public int Id { get; private set; }
    public string Title { get; set; }
    /// <summary>
    /// Дата и время начала встречи
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// Дата и время окончания встречи
    /// </summary>
    public DateTime EndTime { get; set; }
    /// <summary>
    /// Время напоминания (относительно начала встречи)
    /// </summary>
    public TimeSpan ReminderBefore { get; set; }
    /// <summary>
    /// Флаг, указывающий, было ли уже выведено напоминание
    /// </summary>
    public bool IsNotified { get; set; }

    public Meeting(string title, DateTime startTime, DateTime endTime, TimeSpan reminderBefore)
    {
        Id = _idCounter++;
        Title = title;
        StartTime = startTime;
        EndTime = endTime;
        ReminderBefore = reminderBefore;
        IsNotified = false;
    }

    /// <summary>
    /// Возвращает время, когда должно сработать напоминание
    /// </summary>
    public DateTime ReminderTime => StartTime - ReminderBefore;
}