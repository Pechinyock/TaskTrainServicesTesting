namespace TaskTrain.Testing;

internal sealed class SingleThreadStage : ITestStage
{
    private readonly string _name;
    private readonly Func<bool> _proceedLogic;

    public string Name => _name;
    public Func<bool> ProceedDelegate => _proceedLogic;

    public SingleThreadStage(string name, Func<bool> proceedDelegate)
    {
        _name = name;
        _proceedLogic = proceedDelegate;
    }
}
