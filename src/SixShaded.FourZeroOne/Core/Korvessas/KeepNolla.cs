namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class KeepNolla<R>
    where R : class, Rog
{
    public static Korvessa<Rog, MetaFunction<R>, R> Construct(Kor potentialNolla, IKorssa<MetaFunction<R>> valueGen) =>
        new(potentialNolla, valueGen)
        {
            Du = Axoi.Korvedu("KeepNolla"),
            Definition =
                (_, iPotentialNolla, iValueGen) =>
                    iPotentialNolla.kRef().kExists()
                        .kIfTrue<R>(new()
                        {
                            Then = iValueGen.kRef().kExecute(),
                            Else = Core.kNollaFor<R>()
                        })
        };
}