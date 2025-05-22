using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TaskTrain.Testing;

internal static class ToolsInstalledVerifier
{
    public static bool IsToolsInstalled(string toolAlias)
    {
        Debug.Assert(toolAlias is not null);
        Debug.Assert(toolAlias.Length > 0);
        Debug.Assert(!String.IsNullOrWhiteSpace(toolAlias), "tool has to be not empty string");

        string pattern = @"'.+' is not recognized as an (internal|external) command, operable program or batch file\.";

        var processInfo = new ProcessStartInfo()
        {
            FileName = toolAlias,
            Arguments = "--version",
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };
        try
        {
            var proc = Process.Start(processInfo);
            var output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            var match = Regex.IsMatch(output, pattern);
            if (match)
            {
                PrintNotInstalledError(toolAlias);
                return false;
            }
        }
        catch
        {
            PrintNotInstalledError(toolAlias);
            return false;
        }
        return true;
    }

    private static void PrintNotInstalledError(string toolAlias) 
    {
        Log.Error($"In order to run this test you have to install: {toolAlias}" +
            $" and provide it to Path environment variable"
        );
    }
}
