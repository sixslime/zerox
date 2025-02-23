namespace SixShaded.FourZeroOne.FZOSpec;

public static class Extensions
{
    public static IMemoryFZO WithResolution(this IMemoryFZO memory, Res resolution)
    {
        return resolution.Instructions.AccumulateInto(memory, (mem, instruction) => instruction.TransformMemoryUnsafe(mem));
    }
    public static IMemoryFZO WithResolution(this IMemoryFZO memory, ResOpt resolution)
    {
        return resolution.Check(out var r) ? memory.WithResolution(r) : memory;
    }
}
