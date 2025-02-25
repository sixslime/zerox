namespace SixShaded.FourZeroOne.Core.Syntax;

using Resolutions;
public static partial class TokenSyntax
{
    public static Tokens.IO.Select.One<R> tIOSelectOne<R>(this IToken<IMulti<R>> source) where R : class, Res => new(source);

    public static Tokens.IO.Select.Multiple<R> tIOSelectMultiple<R>(this IToken<IMulti<R>> source, IToken<Number> count) where R : class, Res => new(source, count);
}