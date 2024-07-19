namespace ClassroomConsoleApp.Models;

public class Student
{
    private static int _id;

    public Student(string name, string surname)
    {
        Name = name;
        Surname = surname;
        Id = ++_id;
    }

    public int Id { get; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Fullname: {Name} {Surname}";
    }
}
