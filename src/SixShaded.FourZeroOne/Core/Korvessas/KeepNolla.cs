namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public record KeepNolla<R>(Kor potentialNolla, IKorssa<MetaFunction<R>> valueGen) : Korvessa<Rog, MetaFunction<R>, R>(potentialNolla, valueGen)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<Rog, MetaFunction<R>, R> InternalDefinition() =>
        (_, iPotentialNolla, iValueGen) =>
            iPotentialNolla.kRef().kExists()
                .kIfTrue<R>(new()
                {
                    Then = iValueGen.kRef().kExecute(),
                    Else = Core.kNollaFor<R>()
                });
}
