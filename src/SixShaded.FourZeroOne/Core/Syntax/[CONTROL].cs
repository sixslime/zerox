namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class Core
{
    public static Korssas.SubEnvironment<R> kSubEnvironment<R>(Structure.Korssa.SubEnvironment<R> block)
        where R : class, Rog =>
        new(block.Environment.kToMulti(), block.Value);

    public static Korssas.Multi.Create<Rog> kEnv(params Kor[] environment) => new(environment.Map(x => x.kYield()));
}

public static partial class KorssaSyntax
{
    public static Korssas.IfElse<R> kIfTrueExplicit<R>(this IKorssa<Bool> condition, Structure.Korssa.IfElse<MetaFunction<R>> block)
        where R : class, Rog =>
        new(condition, block.Then, block.Else);

    public static Korssas.Execute<R> kIfTrue<R>(this IKorssa<Bool> condition, Structure.Korssa.IfElse<R> block)
        where R : class, Rog =>
        condition.kIfTrueExplicit<R>(
            new()
            {
                Then = block.Then.kMetaBoxed([]),
                Else = block.Else.kMetaBoxed([]),
            })
            .kExecute();
}