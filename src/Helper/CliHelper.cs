namespace Helper;

public static class CliHelper
{
    public static void DisplayGuid(string uuid)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("UUID Copied! ");
        Console.ResetColor();
        Console.Write($"{uuid}\n\n");
    }

    public static bool IsUUIDVersion7(this string version)
    {
        return version.Equals("v7", StringComparison.OrdinalIgnoreCase);
    }
}
