namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.Fixed<Bool> kFixed(this bool value) => new(value);
    public static Korssas.Fixed<Number> kFixed(this int value) => new(value);
    public static Korssas.Fixed<NumRange> kFixed(this Range value) => new(value);

    public static Korssas.Fixed<R> kFixed<R>(this R value)
        where R : class, Rog =>
        new(value);

    public static Korssas.Fixed<Multi<R>> kFixed<R>(this IEnumerable<R> values)
        where R : class, Rog =>
        new(
        new()
        {
            Values = values.ToPSequence(),
        });

    public static Korssas.Fixed<Multi<Number>> kFixed(this IEnumerable<int> values) =>
        new(
        new()
        {
            Values = values.Map(x => (Number)x).ToPSequence(),
        });

    public static Korssas.Fixed<Multi<Bool>> kFixed(this IEnumerable<bool> values) =>
        new(
        new()
        {
            Values = values.Map(x => (Bool)x).ToPSequence(),
        });
}