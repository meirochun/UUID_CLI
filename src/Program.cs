using Helper;
using System.CommandLine;

var versionArgument = new Argument<string>(
    name: "version",
    getDefaultValue: () => "v4",
    description: "The version of UUID to generate");

var upperOption = new Option<bool>(
    aliases: ["-upper"],
    description: "Generates the UUID in uppercase");

var verboseOption = new Option<bool>(
    aliases: ["--verbose", "-v"],
    description: "Enable verbose output");

var mainCommand = new RootCommand("Generates a v4 or v7 UUID (default is v4)");
mainCommand.AddArgument(versionArgument);

mainCommand.SetHandler((version, upper, verbose) =>
{
    Guid guid = version.IsUUIDVersion7() ? Guid.CreateVersion7() : Guid.NewGuid();
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

    ClipboardHelper.SetTextAsync(guidString).Wait();
    CliHelper.DisplayGuid(guidString);
},
versionArgument,
upperOption,
verboseOption);

return await mainCommand.InvokeAsync(args);
