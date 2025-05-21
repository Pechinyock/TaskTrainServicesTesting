namespace TaskTrain.Testing;

internal interface IWithExternalTools
{
    bool AreRequiredToolsInstalled();
    string[] RequiredTools { get; }
}
