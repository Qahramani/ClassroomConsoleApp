using ClassroomConsoleApp.Enums;
using ClassroomConsoleApp.Exceptions;
using ClassroomConsoleApp.Helpers;
using ClassroomConsoleApp.Models;
using ClassroomConsoleApp.Services;
using Newtonsoft.Json;

namespace ClassroomConsoleApp;

public class Program
{
    static void Main(string[] args)
    {
        string classroomPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Jsons", "classroom.json");
        ClassroomService classroomService = new ClassroomService(classroomPath);

    restartMainMenu:
        Console.Clear();
        Console.WriteLine("----- Menu -----");
        Console.Write("[1] Create Classroom\n" +
            "[2] Classrooms info\n" +
            "[3] Add Student\n" +
            "[4] Delete student\n" +
            "[5] Update Student\n" +
            "[6] Show all students\n" +
            "[7] Get student by Id\n" +
            "[0] Exit\n" +
            ">>> ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                classroomService.AddClassroom();
                Console.ReadLine();
                goto restartMainMenu;
            case "2":
                classroomService.ShowAllClassrooms();
                Console.ReadLine();
                goto restartMainMenu;
            case "3":
                classroomService.AddStudent();
                Console.ReadLine();
                goto restartMainMenu;
            case "4":
                classroomService.DeleteStudent();
                Console.ReadLine();
                goto restartMainMenu;
            case "5":
                classroomService.UpdateStudent();
                Console.ReadLine();
                goto restartMainMenu;
            case "6":
                classroomService.ShowAllStudents();
                Console.ReadLine();
                goto restartMainMenu;
            case "7":
                classroomService.GetStudentById();
                Console.ReadLine();
                goto restartMainMenu;
            case "0":
                Colored.WriteLine("GoodBye...", ConsoleColor.DarkYellow);
                return;
            default:
                Colored.WriteLine("Invalid input", ConsoleColor.DarkRed);
                Console.ReadLine();
                goto restartMainMenu;
        }


    }
}
