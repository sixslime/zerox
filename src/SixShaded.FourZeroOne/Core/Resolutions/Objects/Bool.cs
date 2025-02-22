#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Resolutions.Objects
{
    public sealed record Bool : NoOp
    {
        public required bool IsTrue { get; init; }
        public static implicit operator Bool(bool value) => new() { IsTrue = value };
        public override string ToString() => $"{IsTrue}";
    }
}