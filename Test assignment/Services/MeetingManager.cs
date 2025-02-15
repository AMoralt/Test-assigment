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
        
        // Проверка: встреча должна начинаться в будущем
        if (meeting.StartTime <= DateTime.Now)
        {
            error = "Встреча должна планироваться на будущее время.";
            return false;
        }

        if (meeting.EndTime <= meeting.StartTime)
        {
            error = "Время окончания должно быть позже времени начала.";
            return false;
        }

        if (HasOverlap(meeting))
        {
            error = "Встреча пересекается с уже запланированными встречами.";
            return false;
        }

        _meetings.Add(meeting);
        return true;
    }

    /// <summary>
    /// Обновляет встречу после изменения данных.
    /// </summary>
    public bool UpdateMeeting(Meeting updatedMeeting, out string error)
    {
        error = string.Empty;
        var existing = _meetings.FirstOrDefault(m => m.Id == updatedMeeting.Id);
        if (existing == null)
        {
            error = "Встреча не найдена.";
            return false;
        }

        // Проверка: встреча должна начинаться в будущем
        if (updatedMeeting.StartTime <= DateTime.Now)
        {
            error = "Встреча должна планироваться на будущее время.";
            return false;
        }

        if (updatedMeeting.EndTime <= updatedMeeting.StartTime)
        {
            error = "Время окончания должно быть позже времени начала.";
            return false;
        }

        // Исключаем саму встречу при проверке пересечений
        if (HasOverlap(updatedMeeting, updatedMeeting.Id))
        {
            error = "Изменения приводят к пересечению с другими встречами.";
            return false;
        }

        // Обновляем данные
        existing.Title = updatedMeeting.Title;
        existing.StartTime = updatedMeeting.StartTime;
        existing.EndTime = updatedMeeting.EndTime;
        existing.ReminderBefore = updatedMeeting.ReminderBefore;
        existing.IsNotified = false;

        return true;
    }

    /// <summary>
    /// Удаляет встречу по Id.
    /// </summary>
    public bool DeleteMeeting(int meetingId)
    {
        var meeting = _meetings.FirstOrDefault(m => m.Id == meetingId);
        if (meeting == null)
            return false;

        _meetings.Remove(meeting);
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
    /// <summary>
    /// Проверяет, пересекается ли указанная встреча с уже запланированными.
    /// Исключая встречу с id excludeMeetingId (используется при обновлении).
    /// </summary>
    private bool HasOverlap(Meeting meeting, int excludeMeetingId = 0)
    {
        return _meetings.Any(m =>
            m.Id != excludeMeetingId &&
            meeting.StartTime < m.EndTime &&
            meeting.EndTime > m.StartTime);
    }
}