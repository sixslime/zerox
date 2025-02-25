namespace SixShaded.FourZeroOne.Handles;

internal static class HandlesSyntax
{
    public static ITokenContext ToHandle(this FZOSpec.IProcessorFZO.ITokenContext implementation) => new Defined.TokenContextHandle(implementation);
    public static IMemory ToHandle(this FZOSpec.IMemoryFZO implementation) => new Defined.MemoryHandle(implementation);
    public static IInput ToHandle(this FZOSpec.IInputFZO implementation) => new Defined.InputHandle(implementation);
    public static IMemory WithResolution(this IMemory state, Res resolution) => resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.TransformMemory(prevState));
}