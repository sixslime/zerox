namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class Core
{
    public static Korssas.SubEnvironment<R> tSubEnvironment<R>(Structure.Korssa.SubEnvironment<R> block) where R : class, Rog => new(block.Environment.tToMulti(), block.Value);

    public static Korssas.Multi.Union<Rog> tEnv(params Kor[] environment) => new(environment.Map(x => x.tYield()));
}

public static partial class KorssaSyntax
{
    public static Korssas.IfElse<R> tIfTrueDirect<R>(this IKorssa<Bool> condition, Structure.Korssa.IfElse<MetaFunction<R>> block) where R : class, Rog => new(condition, block.Then, block.Else);

    public static Korssas.Execute<R> tIfTrue<R>(this IKorssa<Bool> condition, Structure.Korssa.IfElse<R> block) where R : class, Rog =>
        condition.tIfTrueDirect<R>(new() { Then = block.Then.tMetaBoxed(), Else = block.Else.tMetaBoxed() }).tExecute();
}