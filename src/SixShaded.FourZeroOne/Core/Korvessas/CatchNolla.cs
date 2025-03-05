namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class CatchNolla<R>
    where R : class, Rog
{
    public static Korvessa<R, MetaFunction<R>, R> Construct(IKorssa<R> value, IKorssa<MetaFunction<R>> fallback) => new(value, fallback)
    {
        Du = Axoi.Korvedu("CatchNolla"),
        Definition = Core.tMetaFunction<R, MetaFunction<R>, R>(
                (valueI, fallbackI) =>
                    valueI.tRef().tExists().kIfTrueExplicit<R>(new() { Then = valueI.tRef().tMetaBoxed(), Else = fallbackI.tRef() })
                        .tExecute())
            .Roggi,
    };
}