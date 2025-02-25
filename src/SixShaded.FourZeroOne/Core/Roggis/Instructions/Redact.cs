namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record Redact : Roggi.Defined.Instruction
{
    public required Addr Address { get; init; }
    public override IMemory TransformMemory(IMemory context) => context.WithClearedAddresses([Address]);
}