#nullable enable
namespace FourZeroOne.Core.Resolutions.Instructions
{
    public sealed record Assign<D> : Instruction where D : class, ResObj
    {
        public required IMemoryAddress<D> Address { get; init; }
        public required D Subject { get; init; }
        public override IMemory TransformMemory(IMemory previousState)
        {
            return previousState.WithObjects([(Address, Subject).Tiple()]);
        }
        public override string ToString() => $"{Address}<-{Subject}";
    }
}