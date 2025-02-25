namespace SixShaded.DeTes.Declaration.Impl;

internal static class Extensions
{
    internal static T DeTesUnwrap<T>(this IOption<T> option) =>
        option.Check(out var v)
            ? v
            : throw new UnexpectedNollaException();
}