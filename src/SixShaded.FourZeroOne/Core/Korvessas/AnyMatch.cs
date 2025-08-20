namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class AnyMatch<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, MetaFunction<R, Bool>, Bool> Construct(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) =>
        new(multi, predicate)
        {
            Du = Axoi.Korvedu("AnyMatch"),
            Definition =
                (_, iMulti, iPredicate) =>
                    iMulti.kRef()
                        .kAllMatch(
                        iX =>
                            iPredicate.kRef()
                                .kExecuteWith(
                                new()
                                {
                                    A = iX.kRef()
                                })
                                .kNot())
                        .kNot()
        };
}