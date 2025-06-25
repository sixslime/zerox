namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class KorssaSyntax
{
    public static Korssas.IsEqual kEquals(this Kor a, Kor b) => new(a, b);
}