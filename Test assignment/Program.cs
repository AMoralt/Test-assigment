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
    
    }

    private static void EditMeeting()
    {
    
    }

    private static void DeleteMeeting()
    {
    
    }

    private static void ViewMeetings()
    {
    
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