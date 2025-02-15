using System.Globalization;
using Test_assignment.Services;

namespace Test_assignment;

class Program
{
    private const int ReminderCheckInterval = 1000;
    private static Timer? _reminderTimer;
    private static MeetingManager? _meetingManager;
    static void Main(string[] args)
    {
        _meetingManager = new MeetingManager();

        // Инициализируем таймер для проверки напоминаний
        _reminderTimer = new Timer(CheckReminders, null, 0, ReminderCheckInterval);
        
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Менеджер встреч ===");
            Console.WriteLine("1. Добавить встречу");
            Console.WriteLine("2. Изменить встречу");
            Console.WriteLine("3. Удалить встречу");
            Console.WriteLine("4. Просмотр встреч на день");
            Console.WriteLine("5. Экспорт встреч на день в текстовый файл");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите опцию: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddMeeting();
                    break;
                case "2":
                    EditMeeting();
                    break;
                case "3":
                    DeleteMeeting();
                    break;
                case "4":
                    ViewMeetings();
                    break;
                case "5":
                    ExportMeetings();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    private static void AddMeeting()
    {
        Console.Clear();
        Console.WriteLine("=== Добавление встречи ===");

        try
        {
            Console.Write("Введите заголовок встречи: ");
            string title = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите дату и время начала (например, 2025-03-15 14:00): ");
            DateTime startTime = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Введите дату и время окончания (например, 2025-03-15 15:00): ");
            DateTime endTime = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Введите время напоминания (в минутах до начала встречи): ");
            int reminderMinutes = int.Parse(Console.ReadLine() ?? string.Empty);

            if (_meetingManager != null)
            {
                var meeting = _meetingManager.CreateMeeting(title, startTime, endTime, TimeSpan.FromMinutes(reminderMinutes));

                if (_meetingManager.AddMeeting(meeting, out string error))
                {
                    Console.WriteLine("Встреча успешно добавлена.");
                }
                else
                {
                    Console.WriteLine($"Ошибка: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка ввода данных: {ex.Message}");
        }

        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }

    private static void EditMeeting()
    {
    
    }

    private static void DeleteMeeting()
    {
        Console.Clear();
        Console.WriteLine("=== Удаление встречи ===");

        Console.Write("Введите идентификатор встречи для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int meetingId))
        {
            if (_meetingManager != null && _meetingManager.DeleteMeeting(meetingId))
            {
                Console.WriteLine("Встреча успешно удалена.");
            }
            else
            {
                Console.WriteLine("Встреча не найдена.");
            }
        }
        else
        {
            Console.WriteLine("Неверный идентификатор.");
        }

        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }

    private static void ViewMeetings()
    {
        Console.Clear();
        Console.WriteLine("=== Просмотр встреч ===");
        Console.Write("Введите дату для просмотра (например, 2025-03-15): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            if (_meetingManager != null)
            {
                var meetings = _meetingManager.GetMeetingsByDate(date);
                if (meetings.Count == 0)
                {
                    Console.WriteLine("На выбранную дату встреч нет.");
                }
                else
                {
                    foreach (var meeting in meetings)
                    {
                        Console.WriteLine($"ID: {meeting.Id} | {meeting.Title} | Начало: {meeting.StartTime:yyyy-MM-dd HH:mm} | Окончание: {meeting.EndTime:yyyy-MM-dd HH:mm} | Напоминание за {meeting.ReminderBefore.TotalMinutes} мин.");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Неверный формат даты.");
        }
        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }

    private static void ExportMeetings()
    {
    
    }
    // Метод, вызываемый таймером для проверки напоминаний по встречам.
    private static void CheckReminders(object? state)
    {
        // Получаем список встреч, у которых наступило время напоминания
        if (_meetingManager != null)
        {
            var reminders = _meetingManager.GetDueReminders();
            foreach (var meeting in reminders)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[Напоминание] Встреча \"{meeting.Title}\" начнется в {meeting.StartTime.ToString("g")}");
                Console.ResetColor();
                // Отмечаем встречу как уведомленную, чтобы не выводить напоминание повторно
                meeting.IsNotified = true;
            }
        }
    }
}