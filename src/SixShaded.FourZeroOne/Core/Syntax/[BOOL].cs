namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korssas.Bool;

partial class KorssaSyntax
{
    public static And kAnd(this IKorssa<Bool> a, IKorssa<Bool> b) => new(a, b);
    public static Or kOr(this IKorssa<Bool> a, IKorssa<Bool> b) => new(a, b);
    public static Not kNot(this IKorssa<Bool> val) => new(val);
}