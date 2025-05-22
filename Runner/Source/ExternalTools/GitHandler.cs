using System.Diagnostics;
using System.Text.RegularExpressions;
using TaskTrain.Testing.Constants;

namespace TaskTrain.Testing;

internal static class GitHandler
{
    public static bool CloneRepository(string url, string outPath)
    {
        string gitUrlPattern = @"^((https?:\/\/)([^\/:]+)(:[0-9]+)?(\/.*)?\.git|(git@|ssh:\/\/)([^\/:]+)[:\/]([^\/]+)\/(.+?)\.git)$";
        if (!Regex.IsMatch(url, gitUrlPattern))
        {
            Log.Error("Invalid repository url");
            return false;
        }

        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        var gitClone = new ProcessStartInfo()
        {
            FileName = TestTools.Git,
            Arguments = $"clone {url} {outPath}",
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };
        try
        {
            var process = Process.Start(gitClone);
            var resultOut = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
        catch 
        {
            return false;
        }

        return true;
    }
}
