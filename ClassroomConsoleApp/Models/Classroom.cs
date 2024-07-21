using ClassroomConsoleApp.Enums;
using ClassroomConsoleApp.Exceptions;
using ClassroomConsoleApp.Helpers;

namespace ClassroomConsoleApp.Models;

public class Classroom
{
    private static int _studentId;
    public static int _id;
    public int Id { get;  set; }
    public string Name { get; set; }
    public ClassType ClassType { get; set; }
    public List<Student> Students;

    public Student this[int index]
    {
        get => Students[index];
        set => Students[index] = value;
    }

    public Classroom(string name, ClassType classType)
    {
        //Id = ++_id;
        Name = name;
        ClassType = classType;
        Students = new List<Student>((int)classType);
    }
    public void AddStudent(Student student)
    {
        if ((int)ClassType == Students.Count)
            throw new CapacityLimitException($"Classroom is full, capacity is {(int)ClassType}");
        Students.Add(student);
        student.Id = ++_studentId;
        Colored.WriteLine("Student added successfully", ConsoleColor.DarkGreen);
    }
    public Student GetStudentById(int id)
    {
        foreach (var student in Students)
        {
            if (student.Id == id)
                return student;
        }
        throw new StudentNotFoundException("Student not found");
    }
    public void DeleteStudent(int id)
    {
        foreach (var student in Students)
        {
            if (student.Id == id)
            {
                Students.Remove(student);
                Colored.WriteLine("Student deleted successfully", ConsoleColor.DarkGreen);
                return;
            }
        }
        throw new StudentNotFoundException("Student not found");
    }

    public void UpdateStudent(int id, string name, string surname)
    {
        foreach (var student in Students)
        {
            if(student.Id == id)
            {
                student.Name = name;
                student.Surname = surname;
                Colored.WriteLine("Student updated successfully", ConsoleColor.DarkGreen);
                return;
            }
        }
        throw new StudentNotFoundException("Student not found");
    }
    public override string ToString()
    {
        return $"Id: {Id}, Class name: {Name}, Type: {ClassType}, Student's Count: {Students.Count}, Class Capacity: {(int)ClassType}";
    }

}
