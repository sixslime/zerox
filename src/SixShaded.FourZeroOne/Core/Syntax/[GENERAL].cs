namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class KorssaSyntax
{
    public static Korssas.IsEqual kEquals(this Kor a, Kor b) => new(a, b);

    public static Korssas.TryCast<R> kTryCast<R>(this Kor obj)
        where R : class, Rog =>
        new(obj);
}