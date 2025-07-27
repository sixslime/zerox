namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class KorssaSyntax
{
    public static Korssas.IsEqual kEquals(this Kor a, Kor b) => new(a, b);

    public static Korssas.TryCast<R> kCast<R>(this Kor obj)
        where R : class, Rog =>
        new(obj);
}

public static partial class Core
{
    public static Structure.Hint<T> Hint<T>() => new();
}