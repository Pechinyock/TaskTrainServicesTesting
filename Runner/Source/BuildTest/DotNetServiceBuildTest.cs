using TaskTrain.Testing.Constants;

namespace TaskTrain.Testing;

internal sealed class DotNetServiceBuildTest : TestBase
                                             , IWithExternalTools
{
    public required string RepositoryURL { get; set; }

    public string[] RequiredTools => [ TestTools.Git, TestTools.Docker ];

    public bool AreRequiredToolsInstalled()
    {
        throw new NotImplementedException();
    }

    public override TestResult Run()
    {
        throw new NotImplementedException();
    }
}
