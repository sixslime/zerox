namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korssas.Bool;

partial class KorssaSyntax
{
    public static And kAnd(this IKorssa<Bool> a, IKorssa<Bool> b) => new(a, b);

    public static IKorssa<Bool> ksLazyAnd(this IKorssa<Bool> first, IKorssa<Bool> second) =>
        first.kIfTrue<Bool>(
        new()
        {
            Then = second,
            Else = false.kFixed()
        });
    public static Or kOr(this IKorssa<Bool> a, IKorssa<Bool> b) => new(a, b);

    public static IKorssa<Bool> ksLazyOr(this IKorssa<Bool> first, IKorssa<Bool> second) =>
        first.kIfTrue<Bool>(
        new()
        {
            Then = true.kFixed(),
            Else = second
        });
    public static Not kNot(this IKorssa<Bool> val) => new(val);
}