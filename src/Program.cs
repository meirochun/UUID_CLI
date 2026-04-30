using Helper;
using System.CommandLine;

var versionsCommand = new Command("versions", "List all available UUID versions");

versionsCommand.SetHandler(() =>
{
    Console.WriteLine("Available UUID Versions:");
    Console.WriteLine("- v4: Randomly generated (Standard)");
    Console.WriteLine("- v7: Time-ordered / Sortable (Better for Database Keys)");
});

var uuidVersionArgument = new Argument<string>("version",() => "v4", "The version of UUID to generate");
var upperOption = new Option<bool>(["-u", "--uppercase"], "Generates a UUID in uppercase");
var verboseOption = new Option<bool>(["--verbose"], "Enable verbose output");

var rootCommand = new RootCommand("UUID Generator CLI");
rootCommand.AddArgument(uuidVersionArgument);
rootCommand.AddOption(upperOption);
rootCommand.AddOption(verboseOption);

rootCommand.AddCommand(versionsCommand);

rootCommand.SetHandler(async (version, upper, verbose) =>
{
    Guid guid = version.Contains("v7") ? Guid.CreateVersion7() : Guid.NewGuid();
    string guidString = guid.ToString();

    if (upper)
    {
        guidString = guidString.ToUpper();
    }

    if (verbose)
    {
        CliHelper.DisplayGuid(guidString);
        return;
    }

    await ClipboardHelper.SetTextAsync(guidString);
    CliHelper.DisplayGuid(guidString);
},
uuidVersionArgument, upperOption, verboseOption);

return await rootCommand.InvokeAsync(args);
