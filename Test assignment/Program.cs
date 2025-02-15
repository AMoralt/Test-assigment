using Test_assignment.Services;

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

void AddMeeting()
{
    
}

void EditMeeting()
{
    
}

void DeleteMeeting()
{
    
}

void ViewMeetings()
{
    
}

void ExportMeetings()
{
    
}