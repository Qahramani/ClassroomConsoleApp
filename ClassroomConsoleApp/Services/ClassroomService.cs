using ClassroomConsoleApp.Enums;
using ClassroomConsoleApp.Exceptions;
using ClassroomConsoleApp.Helpers;
using ClassroomConsoleApp.Models;
using Newtonsoft.Json;

namespace ClassroomConsoleApp.Services;

public class ClassroomService
{
    public string classroomPath;
    private List<Classroom> classrooms;

    public ClassroomService(string classroomPath)
    {
        this.classroomPath = classroomPath;
        classrooms = new List<Classroom>();

        string result;
        using (StreamReader sr = new StreamReader(classroomPath))
        {
            result = sr.ReadToEnd();
        }
        if (result != "")
        {
            classrooms = JsonConvert.DeserializeObject<List<Classroom>>(result);
        }
    }

    public void AddStudent()
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
                    if (classroom.Students.Count == (int)classroom.ClassType)
                        throw new CapacityLimitException("Classroom is full");
                    int studentId = GetMaxStudentId() + 1;
                    classroom.Students.Add(new(name, surname, studentId));
                    
                    Convertion(classroomPath);
                    Colored.WriteLine($"Student added succesffully", ConsoleColor.DarkGreen);
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
    public void DeleteStudent()
    {
        Console.WriteLine("----- Delete Student -----");
        ShowAllStudents();
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
            Colored.WriteLine("Student deleted succesffully", ConsoleColor.DarkGreen);
            Convertion(classroomPath);
            return;
        }
        Colored.WriteLine("Student is not found", ConsoleColor.DarkRed);
    }
    public void UpdateStudent()
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

        ShowAllStudents();
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
            foreach (var student in classroom.Students)
            {
                if (student.Id == idUpdate)
                {
                    student.Name = name;
                    student.Surname = surname;
                    Convertion(classroomPath);
                    Colored.WriteLine("Student updated succussfulyy", ConsoleColor.DarkGreen);
                    return;
                }
            }
        }
        Colored.WriteLine("Student is not found", ConsoleColor.DarkRed);
    }
    public void ShowAllStudents()
    {
        if(classrooms.Count == 0)
        {
            Colored.WriteLine("No classroom created yet", ConsoleColor.DarkRed);
            return;
        }
        foreach (var classroom in classrooms)
        {
            foreach (var student in classroom.Students)
            {
                Colored.WriteLine($"{student}, Group: {classroom.Name}, Type: {classroom.ClassType}", ConsoleColor.DarkCyan);
            }
        }
    }
    public void ShowAllClassrooms()
    {
        if (classrooms.Count == 0)
        {
            Colored.WriteLine("No classroom created yet", ConsoleColor.DarkRed);
            return;
        }
        foreach (var classroom in classrooms)
        {
            Colored.WriteLine(classroom.ToString(), ConsoleColor.Magenta);
        }
    }
    public void GetStudentById()
    {
        try
        {
            Console.Write("Id: ");
            int id = int.Parse(Console.ReadLine());
            foreach (var classroom in classrooms)
            {
                var foudStudent = classroom.Students.FirstOrDefault(x => x.Id == id);
                if (foudStudent is null)
                    continue;
                Colored.WriteLine($"{foudStudent}, Group: {classroom.Name}, Type: {classroom.ClassType}", ConsoleColor.Magenta);
                return;
            }
            throw new StudentNotFoundException("Student not found");
        }
        catch (StudentNotFoundException ex)
        {
            Colored.WriteLine(ex.Message, ConsoleColor.DarkRed);
        }
        catch (Exception ex)
        {
            Colored.WriteLine(ex.Message, ConsoleColor.DarkRed);
        }
    }
    public void AddClassroom()
    {
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
        string option = Console.ReadLine();
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
        int maxId = classrooms.Count == 0 ? 1 :  classrooms.Max(x =>  x.Id) + 1; 

        Classroom classRoom = new Classroom(groupName, type, maxId);
        classrooms.Add(classRoom);

        Convertion(classroomPath);
        Colored.WriteLine($"Classroom \"{classRoom.Name}\" created successfully", ConsoleColor.DarkGreen);
    }
    public void Convertion(string classroomPath)
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
    private int GetMaxStudentId()
    {
        int maxId = 0;
        foreach (var classroom in classrooms)
        {
            foreach (var student in classroom.Students)
            {
                if (student.Id > maxId)
                    maxId = student.Id;
            }
        }
        return maxId;
    }

}
