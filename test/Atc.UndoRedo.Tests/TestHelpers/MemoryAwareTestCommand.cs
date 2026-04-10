namespace Atc.UndoRedo.Tests.TestHelpers;

internal sealed class MemoryAwareTestCommand(
    string description,
    long estimatedMemoryBytes)
    : IMemoryAwareUndoCommand
{
    public string Description { get; } = description;

    public long EstimatedMemoryBytes { get; } = estimatedMemoryBytes;

    public void Execute()
    {
    }

    public void UnExecute()
    {
    }
}