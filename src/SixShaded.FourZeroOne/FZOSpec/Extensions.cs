namespace SixShaded.FourZeroOne.FZOSpec;

public static class Extensions
{
    public static IMemoryFZO WithRoggi(this IMemoryFZO memory, Rog roggi) => roggi.Instructions.AccumulateInto(memory, (mem, instruction) => instruction.TransformMemoryUnsafe(mem));

    public static IMemoryFZO WithRoggi(this IMemoryFZO memory, RogOpt roggi) => roggi.Check(out var r) ? memory.WithRoggi(r) : memory;
}