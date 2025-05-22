namespace TaskTrain.Testing;

internal interface ITestStage
{
    string Name { get; }
    Func<bool> ProceedDelegate { get; }
}
