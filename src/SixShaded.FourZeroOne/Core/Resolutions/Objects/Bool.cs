#nullable enable
namespace SixShaded.FourZeroOne.Core.Resolutions.Objects
{
    public sealed record Bool : Resolution.Defined.NoOp
    {
        public required bool IsTrue { get; init; }
        public static implicit operator Bool(bool value) => new() { IsTrue = value };
        public override string ToString() => $"{IsTrue}";
    }
}