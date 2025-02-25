namespace SixShaded.FourZeroOne.Core.Syntax;

using Resolutions;

public static partial class Core
{
    public static Tokens.SubEnvironment<R> tSubEnvironment<R>(Structure.Token.SubEnvironment<R> block) where R : class, Res => new(block.Environment.tToMulti(), block.Value);

    public static Tokens.Multi.Union<Res> tEnv(params Tok[] environment) => new(environment.Map(x => x.tYield()));
}

public static partial class TokenSyntax
{
    public static Tokens.IfElse<R> tIfTrueDirect<R>(this IToken<Bool> condition, Structure.Token.IfElse<MetaFunction<R>> block) where R : class, Res => new(condition, block.Then, block.Else);

    public static Tokens.Execute<R> tIfTrue<R>(this IToken<Bool> condition, Structure.Token.IfElse<R> block) where R : class, Res =>
        condition.tIfTrueDirect<R>(new() { Then = block.Then.tMetaBoxed(), Else = block.Else.tMetaBoxed() }).tExecute();
}