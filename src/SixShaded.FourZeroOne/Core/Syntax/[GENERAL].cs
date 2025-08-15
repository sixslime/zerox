namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class KorssaSyntax
{
    public static Korssas.IsEqual kEquals<R>(this IKorssa<R> a, IKorssa<R> b)
        where R : class, Rog
        => new(a, b);

    public static Korssas.TryCast<R> kIsType<R>(this Kor obj)
        where R : class, Rog =>
        new(obj);
    public static Korssas.TryCast<IRoveggi<C>> kIsType<C>(this IKorssa<IRoveggi<IRovetu>> obj)
        where C : IRovetu =>
        new(obj);
}

public static partial class Core
{
    public static Structure.Hint<T> Hint<T>() => new();
}