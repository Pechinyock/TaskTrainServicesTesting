namespace TaskTrain.Testing;

internal enum TestResult 
{
    Success,
    Failure,
}

internal abstract class TestBase
{
    public required string Name { get; set; }
    public string ServiceName { get; set; }

    public abstract TestResult Run();
}
