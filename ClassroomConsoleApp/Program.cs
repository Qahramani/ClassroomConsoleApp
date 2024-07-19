using ClassroomConsoleApp.Enums;
using ClassroomConsoleApp.Exceptions;
using ClassroomConsoleApp.Helpers;
using ClassroomConsoleApp.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClassroomConsoleApp;

public class Program
{
    static void Main(string[] args)
    {
        string classroomPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Jsons", "classroom.json");

        List<Classroom> classrooms = new();


    restartMainMenu:
        Console.WriteLine("----- Menu -----");
        Console.Write("[1] Create Classroom\n" +
            "[2] Classrooms info\n" +
            "[3] Add Student\n" +
            "[4] Delete student\n" +
            "[5] Show all students\n" +
            ">>> ");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
            restartClassroomCreation:
                Console.WriteLine("----- Classroom Creation -----");
                Console.Write("Group Name: ");
                string groupName = Console.ReadLine().Trim();
                if (!groupName.ClassroomNameChecker())
                {
                    goto restartClassroomCreation;
                }
                Console.Write("Class Type: ");
                Console.WriteLine("[1] Frontend\n" +
                    "[2] Backend");
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

                classrooms = Convertion(classroomPath, classrooms);

                goto restartMainMenu;

            case "2":
                Console.WriteLine("----- Classrooms List -----");
                foreach (var classroom in classrooms)
                {
                    Console.WriteLine(classroom);
                }
                goto restartMainMenu;
            case "3":
            restartStudentCreation:
                Console.WriteLine("----- Student Creation -----");

                Console.Write("Student name: ");
                string studentName = Console.ReadLine().Trim();
                if (!studentName.IsCapitalized() || !studentName.WordCount())
                {
                    goto restartStudentCreation;
                }

                string studentSurname = Console.ReadLine().Trim();
                if (!studentSurname.IsCapitalized() || !studentSurname.WordCount())
                {
                    goto restartStudentCreation;
                }

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
                            classroom.AddStudent(new(studentName, studentSurname));
                            Convertion(classroomPath, classrooms);
                            goto restartMainMenu;
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
                goto restartMainMenu;
            case "4":
                Console.WriteLine("----- Delete Student -----");
                foreach (var classroom in classrooms)
                {
                    Console.WriteLine(classroom);
                }
                ShowAllStudents(classrooms);
                Console.Write("Student Id: ");
                int idDelete;
                bool isCorrect = int.TryParse(Console.ReadLine(), out idDelete);
                if (!isCorrect)
                {
                    Colored.WriteLine("Invalid input for id", ConsoleColor.DarkRed);
                    goto restartMainMenu;
                }

                foreach (var classroom in classrooms)
                {
                    var deletedStudent = classroom.Students.FirstOrDefault(x => x.Id == idDelete);
                    if (deletedStudent is null)
                        continue;
                    classroom.Students.Remove(deletedStudent);
                    Convertion(classroomPath, classrooms);
                    goto restartMainMenu;
                }
                Colored.WriteLine("Student is not found", ConsoleColor.DarkRed);
                goto restartMainMenu;
            case "5":
                ShowAllStudents(classrooms);
                goto restartMainMenu;
            default:
                break;
        }


    }

    public static void ShowAllStudents(List<Classroom> classrooms)
    {
        foreach (var classroom in classrooms)
        {
            Console.WriteLine($"Group: {classroom.Name}");
            foreach (var student in classroom.Students)
            {
                Console.WriteLine(student);
            }
        }
    }

    private static List<Classroom> Convertion(string classroomPath, List<Classroom> classrooms)
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
        return classrooms;
    }
}
