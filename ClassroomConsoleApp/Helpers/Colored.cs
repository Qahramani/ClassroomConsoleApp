
using ClassroomConsoleApp.Models;

namespace ClassroomConsoleApp.Helpers;

public static class Colored
{
    public static void WriteLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}

