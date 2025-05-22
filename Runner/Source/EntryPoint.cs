using TaskTrain.Testing;

internal static class EntryPoint
{
    internal static void Main(string[] args)
    {
        var runner = new TestRunner();
        runner.Run(args);
    }
}