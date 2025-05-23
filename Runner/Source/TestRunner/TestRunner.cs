namespace TaskTrain.Testing;

internal enum Mode
{
    FromFile,
    Interactive
}

internal static class Log
{
    private static ILogger _log;

    public static void Initialize(ILogger logger = null)
    {
        _log = logger ?? new ConsoleLogger();
        _log.SetVerbosity(Verbosity.Error | Verbosity.Info | Verbosity.Warn | Verbosity.Trace);
    }

    public static void Trace(string message) 
    {
        if (IsMessageEmpty(message))
            return;

        _log.Trace(message);
    }

    public static void Info(string message) 
    {
        if (IsMessageEmpty(message))
            return;

        _log.Info(message);
    }

    public static void Warn(string message) 
    {
        if (IsMessageEmpty(message))
            return;

        _log.Warn(message);
    }

    public static void Error(string message) 
    {
        if (IsMessageEmpty(message))
            return;

        _log.Error(message);
    }

    private static bool IsMessageEmpty(string message) 
    {
        return String.IsNullOrEmpty(message);
    }
}

internal sealed class TestRunner
{
    public void Run(string[] args) 
    {
        Log.Initialize();
        var userHubBuildTest = new DotNetServiceBuildTest("user-hub") { Name = "user-hub build tests"
            , RepositoryURL = "https://github.com/Pechinyock/users-hub.git" 
        };
        var apiGatewayBuildTest = new DotNetServiceBuildTest("apiGateway"){ Name = "api gateway build tests"
            , RepositoryURL = "https://github.com/Pechinyock/api-gateway.git"
        };
        userHubBuildTest.Run();
        apiGatewayBuildTest.Run();
    }
}
