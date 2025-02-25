namespace SixShaded.FourZeroOne.Handles;

internal static class Extensions
{
    public static IKorssaContext ToHandle(this FZOSpec.IProcessorFZO.IKorssaContext implementation) => new Defined.KorssaContextHandle(implementation);
    public static IMemory ToHandle(this FZOSpec.IMemoryFZO implementation) => new Defined.MemoryHandle(implementation);
    public static IInput ToHandle(this FZOSpec.IInputFZO implementation) => new Defined.InputHandle(implementation);
    public static IMemory WithResolution(this IMemory state, Rog resolution) => resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.TransformMemory(prevState));
}