using System.Diagnostics;
using System.Runtime.InteropServices;

namespace guid_cli.Helper;

public static class ClipboardHelper
{
    public static async Task SetText(string text)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            await RunProcessAsync("clip", text);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            await RunProcessAsync("pbcopy", text);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            if (CommandExists("wl-copy"))
            {
                await RunProcessAsync("wl-copy", text);
            }
            else if (CommandExists("xclip"))
            {
                await RunProcessAsync("xclip", text, "-selection clipboard");
            }
            else if (CommandExists("xsel"))
            {
                await RunProcessAsync("xsel", text, "--clipboard --input");
            }
            else
            {
                throw new NotSupportedException(
                    "No clipboard tool found. Install wl-clipboard, xclip, or xsel."
                );
            }
        }
        else
        {
            throw new PlatformNotSupportedException("Clipboard is not supported on this OS.");
        }
    }

    private static async Task RunProcessAsync(string fileName, string input, string arguments = "")
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        await process.StandardInput.WriteAsync(input);
        process.StandardInput.Close();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Clipboard command failed: {fileName}");
        }
    }

    private static bool CommandExists(string command)
    {
        var checker = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "where"
            : "which";

        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = checker,
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            process?.WaitForExit();
            return process?.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
