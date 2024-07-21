using ClassroomConsoleApp.Enums;
using ClassroomConsoleApp.Exceptions;
using ClassroomConsoleApp.Helpers;

namespace ClassroomConsoleApp.Models;

public class Classroom
{
    public int Id { get;  set; }
    public string Name { get; set; }
    public ClassType ClassType { get; set; }
    public List<Student> Students;
    public Classroom(string name, ClassType classType, int id)
    {
        Name = name;
        ClassType = classType;
        Students = new List<Student>((int)ClassType);
        Id = id;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Class name: {Name}, Type: {ClassType}, Student's Count: {Students.Count}, Class Capacity: {(int)ClassType}";
    }

}
