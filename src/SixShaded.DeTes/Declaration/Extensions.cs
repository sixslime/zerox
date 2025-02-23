namespace SixShaded.DeTes.Declaration;

internal static class Extensions
{
    internal static T DeTesUnwrap<T>(this IOption<T> option)
    {
        return option.Check(out var v)
            ? v
            : throw new UnexpectedNollaException();
    }
}