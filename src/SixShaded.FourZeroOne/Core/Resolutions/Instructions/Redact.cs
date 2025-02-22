namespace SixShaded.FourZeroOne.Core.Resolutions.Instructions;

public sealed record Redact : Resolution.Defined.Instruction
{
    public required Addr Address { get; init; }
    public override IMemory TransformMemory(IMemory context) => context.WithClearedAddresses([Address]);
}