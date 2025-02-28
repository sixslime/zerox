namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.Fixed<Bool> tFixed(this bool value) => new(value);

    public static Korssas.Fixed<Number> tFixed(this int value) => new(value);

    public static Korssas.Fixed<NumRange> tFixed(this Range value) => new(value);

    public static Korssas.Fixed<R> tFixed<R>(this R value) where R : class, Rog => new(value);

    public static Korssas.Fixed<Multi<R>> tFixed<R>(this IEnumerable<R> values) where R : class, Rog => new(new() { Values = values.ToPSequence() });
    public static Korssas.Fixed<Multi<Number>> tFixed(this IEnumerable<int> values) => new(new() { Values = values.Map(x => (Number)x).ToPSequence() });
    public static Korssas.Fixed<Multi<Bool>> tFixed(this IEnumerable<bool> values) => new(new() { Values = values.Map(x => (Bool)x).ToPSequence() });
}