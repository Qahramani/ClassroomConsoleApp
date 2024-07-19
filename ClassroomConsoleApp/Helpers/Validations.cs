namespace ClassroomConsoleApp.Helpers;

public static class Validations
{
    public static bool IsCapitalized(this string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            Colored.WriteLine("Input cannot be null or empty", ConsoleColor.DarkRed);
            return false;
        }

        if (!char.IsUpper(word[0]))
        {
            Colored.WriteLine("First letter should be upper", ConsoleColor.DarkRed);
            return false;
        }
        for (int i = 1; i < word.Length; i++)
        {
            if (!char.IsLower(word[i]))
            {
                Colored.WriteLine("First letter should be Upper and rest should be Lower", ConsoleColor.DarkRed);
                return false;
            }
        }

        return true;
    }
    public static bool WordCount(this string word)
    {
        if (word.Length < 3)
        {
            Colored.WriteLine("Length should be >= 3", ConsoleColor.DarkRed);
            return false;
        }
        if(word.Contains(' '))
        {
            Colored.WriteLine("Input should contain only one word", ConsoleColor.DarkRed);
            return false;
        }
        return true;
    }
    public static bool ClassroomNameChecker(this string className)
    {
        if(className.Length != 5)
        {
            Colored.WriteLine("Lenght should be 5", ConsoleColor.DarkRed);
            return false;
        }

        if (char.IsUpper(className[0]) && char.IsUpper(className[1]) &&
            char.IsDigit(className[2]) && char.IsDigit(className[3]) && char.IsDigit(className[4]))
        {
            return true;
        }
        Colored.WriteLine("Classroom name should contain 2 Upper letters and 3 digits (ex PB303)", ConsoleColor.DarkRed);
        return false;

    }
}
