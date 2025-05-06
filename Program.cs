using System.CommandLine;
using TextCopy;

var rootCmd = new RootCommand("guid");

rootCmd.SetHandler(() =>
{
    var guid = GenerateAndCopyGuid();
    DisplayGuid(guid);
});

return await rootCmd.InvokeAsync(args);

static Guid GenerateAndCopyGuid()
{
    var guid = Guid.NewGuid();
    ClipboardService.SetText(guid.ToString());
    return guid;
}

static void DisplayGuid(Guid guid)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("UUID Copied! ");
    Console.ResetColor();
    Console.Write(guid.ToString());
}