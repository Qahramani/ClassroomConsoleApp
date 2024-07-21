namespace ClassroomConsoleApp.Models;

public class Student
{
    public Student(string name, string surname, int studentId)
    {
        Name = name;
        Surname = surname;
        Id = studentId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Fullname: {Name} {Surname}";
    }
}
