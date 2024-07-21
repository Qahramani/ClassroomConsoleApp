using ClassroomConsoleApp.Enums;
using ClassroomConsoleApp.Exceptions;
using ClassroomConsoleApp.Helpers;
using ClassroomConsoleApp.Models;
using Newtonsoft.Json;

namespace ClassroomConsoleApp;

public class Program
{
    static void Main(string[] args)
    {
        string classroomPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Jsons", "classroom.json");

        List<Classroom> classrooms = new();

        string result;
        using (StreamReader sr = new StreamReader(classroomPath))
        {
            result = sr.ReadToEnd();
        }
        if (result != "")
            classrooms = JsonConvert.DeserializeObject<List<Classroom>>(result);


        restartMainMenu:
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
                CreateClassroom(classroomPath, classrooms);
                goto restartMainMenu;
            case "2":
                Console.WriteLine("----- Classrooms List -----");
                foreach (var classroom in classrooms)
                {
                    Console.WriteLine(classroom);
                }
                goto restartMainMenu;
            case "3":
                CreateStudent(classroomPath, classrooms);
                goto restartMainMenu;
            case "4":
                DeleteStudent(classroomPath, classrooms);
                goto restartMainMenu;
            case "5":
                UpdateStudent(classroomPath, classrooms);
                goto restartMainMenu;
            case "6":
                ShowAllStudents(classrooms);
                goto restartMainMenu;
            case "7":
                Console.Write("Id: ");
                int id ;
                bool isCorrect = int.TryParse(Console.ReadLine(), out id);
                if (!isCorrect)
                {
                    Colored.WriteLine("Invalid input for id", ConsoleColor.DarkRed);
                    goto restartMainMenu;
                }
                foreach (var classroom in classrooms)
                {
                    var foudStudent = classroom.Students.FirstOrDefault(x => x.Id == id);
                    if (foudStudent is null)
                        continue;
                    Console.WriteLine($"{foudStudent}, Group: {classroom.Name}, Type: {classroom.ClassType}");
                    goto restartMainMenu;
                }
                Colored.WriteLine("Student not found", ConsoleColor.DarkRed);
                goto restartMainMenu;
            case "0":
                Colored.WriteLine("GoodBye...", ConsoleColor.DarkYellow);
                return;
            default:
                Colored.WriteLine("Invalid input", ConsoleColor.DarkRed);
                goto restartMainMenu;
        }


    }

    private static void UpdateStudent(string classroomPath, List<Classroom> classrooms)
    {
    restartStudentUpdate:
        Console.WriteLine("----- Update Student -----");

        Console.Write("New Name: ");
        string name = Console.ReadLine();
        if (!name.IsCapitalized() || !name.WordCount())
            goto restartStudentUpdate;

        Console.Write("New Surname: ");
        string surname = Console.ReadLine();
        if (!surname.IsCapitalized() || !surname.WordCount())
            goto restartStudentUpdate;

        ShowAllStudents(classrooms);
        Console.Write("Id of student that you want update: ");
        int idUpdate;
        bool isCorrect = int.TryParse(Console.ReadLine(), out idUpdate);
        if (!isCorrect)
        {
            Colored.WriteLine("Invalid input for Id", ConsoleColor.DarkRed);
            goto restartStudentUpdate;
        }

        foreach (var classroom in classrooms)
        {
            var updatedStudent = classroom.Students.FirstOrDefault(x => x.Id == idUpdate);
            if (updatedStudent is null)
                continue;

            classroom.UpdateStudent(idUpdate, name, surname);
            Convertion(classroomPath, classrooms);
            return;
        }
        Colored.WriteLine("Student is not found", ConsoleColor.DarkRed);
    }

    private static void DeleteStudent(string classroomPath, List<Classroom> classrooms)
    {
        Console.WriteLine("----- Delete Student -----");
        ShowAllStudents(classrooms);
        Console.Write("Student Id: ");
        int idDelete;
        bool isCorrect = int.TryParse(Console.ReadLine(), out idDelete);
        if (!isCorrect)
        {
            Colored.WriteLine("Invalid input for id", ConsoleColor.DarkRed);
            return;
        }

        foreach (var classroom in classrooms)
        {
            var deletedStudent = classroom.Students.FirstOrDefault(x => x.Id == idDelete);
            if (deletedStudent is null)
                continue;
            classroom.Students.Remove(deletedStudent);
            Convertion(classroomPath, classrooms);
            return;
        }
        Colored.WriteLine("Student is not found", ConsoleColor.DarkRed);
    }

    private static void CreateStudent(string classroomPath, List<Classroom> classrooms)
    {
        if (classrooms.Count == 0)
        {
            Colored.WriteLine("There is no classroom in the Course. Please create a classroom at first", ConsoleColor.DarkRed);
            return;
        }
    restartStudentCreation:
        Console.WriteLine("----- Student Creation -----");

        Console.Write("Student name: ");
        string name = Console.ReadLine().Trim();
        if (!name.IsCapitalized() || !name.WordCount())
            goto restartStudentCreation;


        Console.Write("Student Surname: ");
        string surname = Console.ReadLine().Trim();
        if (!surname.IsCapitalized() || !surname.WordCount())
            goto restartStudentCreation;

        foreach (var classroom in classrooms)
        {
            Console.WriteLine(classroom);
        }
        try
        {
            Console.Write("Classroom Id: ");
            int classroomId = int.Parse(Console.ReadLine());
            foreach (var classroom in classrooms)
            {
                if (classroom.Id == classroomId)
                {
                    classroom.AddStudent(new(name, surname));
                    Convertion(classroomPath, classrooms);
                    return;
                }
            }

            throw new ClassroomNotFoundException("Classroom not found");
        }
        catch (CapacityLimitException ex)
        {
            Colored.WriteLine(ex.Message, ConsoleColor.DarkRed);
        }
        catch (ClassroomNotFoundException ex)
        {
            Colored.WriteLine(ex.Message, ConsoleColor.DarkRed);
        }
        catch (Exception ex)
        {
            Colored.WriteLine(ex.Message, ConsoleColor.DarkRed);
        }
    }

    private static void CreateClassroom(string classroomPath, List<Classroom> classrooms)
    {
        string option;
    restartClassroomCreation:
        Console.WriteLine("----- Classroom Creation -----");
        Console.Write("Group Name: ");
        string groupName = Console.ReadLine().Trim();
        if (!groupName.ClassroomNameChecker())
        {
            goto restartClassroomCreation;
        }
        Console.WriteLine("[1] Frontend\n" +
            "[2] Backend");
        Console.Write("Class Type: ");
        option = Console.ReadLine();
        ClassType type;
        switch (option)
        {
            case "1":
                type = ClassType.Frontend;
                break;
            case "2":
                type = ClassType.Backend;
                break;
            default:
                Colored.WriteLine("Invalid input", ConsoleColor.DarkRed);
                goto restartClassroomCreation;
        }

        Classroom classRoom = new Classroom(groupName, type);
        classrooms.Add(classRoom);

        Convertion(classroomPath, classrooms);
        Colored.WriteLine($"Classroom \"{classRoom.Name}\" created successfully", ConsoleColor.DarkGreen);
    }

    public static void ShowAllStudents(List<Classroom> classrooms)
    {
        foreach (var classroom in classrooms)
        {
            foreach (var student in classroom.Students)
            {
                Colored.WriteLine($"{student}, Group: {classroom.Name}, Type: {classroom.ClassType}", ConsoleColor.DarkCyan);
            }
        }
    }

    public static void Convertion(string classroomPath, List<Classroom> classrooms)
    {
        var json = JsonConvert.SerializeObject(classrooms);
        using (StreamWriter sr = new StreamWriter(classroomPath))
        {
            sr.WriteLine(json);
        }
        string result;
        using (StreamReader sr = new StreamReader(classroomPath))
        {
            result = sr.ReadToEnd();
        }

        classrooms = JsonConvert.DeserializeObject<List<Classroom>>(result);
    }
}
