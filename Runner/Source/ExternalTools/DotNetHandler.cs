using System.Diagnostics;
using TaskTrain.Testing.Constants;

namespace TaskTrain.Testing;

internal static class DotNetHandler
{
    public static bool DotNetBuild(string pathToSln, string configuration) 
    {
        if (!Directory.Exists(pathToSln)) 
        {
            Log.Error($"Couldn't get resently cloned repository at path: {pathToSln}");
            return false;
        }

        var dotnetBuild = new ProcessStartInfo()
        {
            FileName = TestTools.DotNet,
            Arguments = $"build {pathToSln} -c {configuration}",
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };

        try
        {
            var process = Process.Start(dotnetBuild);
            var resultOut = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var exitCode = process.ExitCode;
            if (exitCode != 0) 
            {
                Log.Error("Failed to build! Error message:");
                Log.Error($"{resultOut}");
                return false;
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
}
