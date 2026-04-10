namespace Atc.UndoRedo.Interfaces;

/// <summary>
/// Extends <see cref="IUndoCommand"/> with serialization support,
/// enabling commands to be persisted to disk and restored across
/// application restarts.
/// </summary>
public interface ISerializableUndoCommand : IUndoCommand
{
    /// <summary>
    /// Gets a stable identifier for the command type, used during
    /// deserialization to locate the correct deserializer.
    /// </summary>
    string TypeId { get; }

    /// <summary>
    /// Serializes the command state to a byte array.
    /// </summary>
    /// <returns>A byte array containing the serialized command state.</returns>
    byte[] Serialize();
}