#nullable enable
namespace SixShaded.FourZeroOne.Core.Resolutions
{
    public sealed record Number : Resolution.Defined.NoOp
    {
        public required int Value { get; init; }
        public static implicit operator Number(int value) => new() { Value = value };
        public override string ToString() => $"{Value}";
    }
}