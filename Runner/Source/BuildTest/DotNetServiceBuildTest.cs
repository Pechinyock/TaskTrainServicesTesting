using TaskTrain.Testing.Constants;

namespace TaskTrain.Testing;

internal sealed class DotNetServiceBuildTest : TestBase
                                             , IWithExternalTools
{
    public required string RepositoryURL { get; set; }
    public string[] RequiredTools => [ TestTools.Git, TestTools.Docker, TestTools.DotNet ];

    private readonly ITestStage[] _testSages;

    private readonly string _serviceRepositoriesHomePath = Path.Combine(AppContext.BaseDirectory, "repositories");
    public DotNetServiceBuildTest(string serviceName)
    {
        var serviceSaveDirectoryPath = Path.Combine(_serviceRepositoriesHomePath, serviceName);

        _testSages = new[]
        {
            new SingleThreadStage($"cloning repository: {serviceName}"
            , () => 
            {
                if(!GitHandler.CloneRepository(RepositoryURL, serviceSaveDirectoryPath))
                    return false;

                return true;
            }),
            new SingleThreadStage($"building service: {serviceName}"
            , () => 
            {
                if(!DotNetHandler.DotNetBuild(serviceSaveDirectoryPath, "Release"))
                    return false;

                return true; 
            }),
            new SingleThreadStage($"verify service project structure"
            , () =>
            {
                if(!IsProjectStructureCorrect(serviceSaveDirectoryPath))
                    return false;

                return true;
            })
        };
    }

    public bool AreRequiredToolsInstalled()
    {
        foreach (var tool in RequiredTools) 
        {
            var result = ToolsInstalledVerifier.IsToolsInstalled(tool);
            if(!result)
                return false;
        }
        return true;
    }

    public override TestResult Run()
    {
        if(!AreRequiredToolsInstalled())
            return TestResult.Failure;
        const string padding = "    ";
        foreach (var stage in _testSages) 
        {
            Log.Trace($"{stage.Name}");
            if (!stage.ProceedDelegate.Invoke())
            {
                Log.Error($"{padding}Failed on stage: {stage.Name}");
                return TestResult.Failure;
            }
            Log.Info($"{padding}Success!");
        }
        return TestResult.Success;
    }

    private static bool IsProjectStructureCorrect(string pathToCSharpProject) 
    {
        if(!Directory.Exists(pathToCSharpProject))
            return false;

        var dirs = Directory.GetDirectories(pathToCSharpProject);

        var outDirsNames = new Dictionary<string, bool>()
        {
            { "_bin", false },
            { "_bin-int", false },
            { "Tests" , false },
        };

        foreach (var dir in dirs) 
        {
            var dirName = new DirectoryInfo(dir).Name;

            if (outDirsNames.ContainsKey(dirName)) 
                outDirsNames[dirName] = true;
        }

        var notFounded = outDirsNames.Where(x => x.Value == false);

        if (notFounded.Any())
        {
            Log.Error("Invalid project structure");
            foreach (var notFound in notFounded) 
            {
                Log.Error($"{notFound.Key} - is not founded");
            }
            return false;
        }

        return true;
    }
}
