namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Concat<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, IMulti<R>, Multi<R>> Construct(IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b) =>
        new(a, b)
        {
            Du = Axoi.Korvedu("concat"),
            Definition =
                Core.kMetaFunction<IMulti<R>, IMulti<R>, Multi<R>>(
                [],
                (iA, iB) =>
                    Core.kMulti<IMulti<R>>(
                        iA.kRef(),
                        iB.kRef())
                        .kFlatten()),
        };
}