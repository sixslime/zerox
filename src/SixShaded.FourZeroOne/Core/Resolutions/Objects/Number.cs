#nullable enable
namespace FourZeroOne.Core.Resolutions.Objects
{
    public sealed record Number : NoOp
    {
        public required int Value { get; init; }
        public static implicit operator Number(int value) => new() { Value = value };
        public override string ToString() => $"{Value}";
    }
}