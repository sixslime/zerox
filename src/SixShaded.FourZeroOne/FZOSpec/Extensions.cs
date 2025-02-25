namespace SixShaded.FourZeroOne.FZOSpec;

public static class Extensions
{
    public static IMemoryFZO WithResolution(this IMemoryFZO memory, Res resolution) => resolution.Instructions.AccumulateInto(memory, (mem, instruction) => instruction.TransformMemoryUnsafe(mem));

    public static IMemoryFZO WithResolution(this IMemoryFZO memory, ResOpt resolution) => resolution.Check(out var r) ? memory.WithResolution(r) : memory;
}