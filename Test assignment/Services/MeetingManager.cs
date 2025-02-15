using Test_assignment.Models;

namespace Test_assignment.Services;

public class MeetingManager
{
    private readonly List<Meeting> _meetings = new List<Meeting>();

    /// <summary>
    /// Фабричный метод для создания встречи.
    /// </summary>
    public Meeting CreateMeeting(string title, DateTime startTime, DateTime endTime, TimeSpan reminderBefore)
    {
        return new Meeting(title, startTime, endTime, reminderBefore);
    }

    /// <summary>
    /// Добавляет встречу, если соблюдены все условия 
    /// </summary>
    public bool AddMeeting(Meeting meeting, out string error)
    {
        error = string.Empty;
        
        return true;
    }

    /// <summary>
    /// Обновляет встречу после изменения данных.
    /// </summary>
    public bool UpdateMeeting(Meeting updatedMeeting, out string error)
    {
        error = string.Empty;

        return true;
    }

    /// <summary>
    /// Удаляет встречу по Id.
    /// </summary>
    public bool DeleteMeeting(int meetingId)
    {
        return true;
    }

    /// <summary>
    /// Возвращает встречу по Id.
    /// </summary>
    public Meeting GetMeetingById(int meetingId)
    {
        return _meetings.FirstOrDefault(m => m.Id == meetingId);
    }

    /// <summary>
    /// Возвращает список встреч для выбранного дня.
    /// </summary>
    public List<Meeting> GetMeetingsByDate(DateTime date)
    {
        return _meetings
            .Where(m => m.StartTime.Date == date.Date)
            .OrderBy(m => m.StartTime)
            .ToList();
    }
    
    /// <summary>
    /// Возвращает список встреч, для которых наступило время напоминания и напоминание ещё не было выведено.
    /// </summary>
    public List<Meeting> GetDueReminders()
    {
        DateTime now = DateTime.Now;
        return _meetings
            .Where(m => !m.IsNotified && now >= m.ReminderTime && now < m.StartTime)
            .ToList();
    }
}