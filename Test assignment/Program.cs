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

        // Запрос заголовка встречи
        string title;
        while (true)
        {
            Console.Write("Введите заголовок встречи: ");
            title = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(title))
                break;
            Console.WriteLine("Заголовок не может быть пустым. Попробуйте снова.");
        }

        // Запрос даты и времени начала
        DateTime startTime;
        while (true)
        {
            Console.Write("Введите дату и время начала (например, 2025-03-15 14:00): ");
            string input = Console.ReadLine() ?? string.Empty;
            if (DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                break;
            Console.WriteLine("Неверный формат даты и времени. Попробуйте снова.");
        }

        // Запрос даты и времени окончания
        DateTime endTime;
        while (true)
        {
            Console.Write("Введите дату и время окончания (например, 2025-03-15 15:00): ");
            string input = Console.ReadLine() ?? string.Empty;
            if (DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                break;
            Console.WriteLine("Неверный формат даты и времени. Попробуйте снова.");
        }

        // Запрос времени напоминания (в минутах)
        int reminderMinutes;
        while (true)
        {
            Console.Write("Введите время напоминания (в минутах до начала встречи): ");
            string input = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(input, out reminderMinutes))
                break;
            Console.WriteLine("Неверный формат числа. Попробуйте снова.");
        }

        // Создаем и пытаемся добавить встречу
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

        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }

    private static void EditMeeting()
    {
        Console.Clear();
        Console.WriteLine("=== Изменение встречи ===");

        Console.Write("Введите идентификатор встречи: ");
        if (int.TryParse(Console.ReadLine(), out int meetingId))
        {
            if (_meetingManager != null)
            {
                var meeting = _meetingManager.GetMeetingById(meetingId);
                if (meeting == null)
                {
                    Console.WriteLine("Встреча не найдена.");
                }
                else
                {
                    try
                    {
                        Console.Write($"Заголовок ({meeting.Title}): ");
                        string title = Console.ReadLine() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(title))
                            meeting.Title = title;

                        Console.Write($"Дата и время начала ({meeting.StartTime:yyyy-MM-dd HH:mm}): ");
                        string startInput = Console.ReadLine() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(startInput))
                            meeting.StartTime = DateTime.ParseExact(startInput, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                        Console.Write($"Дата и время окончания ({meeting.EndTime:yyyy-MM-dd HH:mm}): ");
                        string endInput = Console.ReadLine() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(endInput))
                            meeting.EndTime = DateTime.ParseExact(endInput, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                        Console.Write($"Время напоминания в минутах ({meeting.ReminderBefore.TotalMinutes}): ");
                        string reminderInput = Console.ReadLine() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(reminderInput))
                            meeting.ReminderBefore = TimeSpan.FromMinutes(int.Parse(reminderInput));

                        // Сброс флага уведомления при изменении встречи
                        meeting.IsNotified = false;

                        if (_meetingManager.UpdateMeeting(meeting, out string error))
                        {
                            Console.WriteLine("Встреча успешно обновлена.");
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка: {error}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка ввода данных: {ex.Message}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Неверный идентификатор.");
        }

        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
        Console.ReadKey();      
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
        Console.Clear();
        Console.WriteLine("=== Экспорт встреч ===");
        Console.Write("Введите дату для экспорта (например, 2025-03-15): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            Console.Write("Введите путь и имя файла для экспорта: ");
            string filePath = Console.ReadLine() ?? string.Empty;

            if (_meetingManager != null)
            {
                if (_meetingManager.ExportMeetings(date, filePath, out string error))
                {
                    Console.WriteLine("Экспорт успешно выполнен.");
                }
                else
                {
                    Console.WriteLine($"Ошибка экспорта: {error}");
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
    // Метод, вызываемый таймером для проверки напоминаний по встречам.
    private static void CheckReminders(object? state)
    {
        Console.WriteLine("CheckReminders");
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