namespace SixShaded.CoreTypeMatcher;
using FZOTypeMatch;
using FourZeroOne.Roveggi.Unsafe;

internal static partial class Maps
{
    private static Func<Kor, IResult<RovuInfo, AbstractRovuInfo>> RovuInfoGetter(FZOTypeMatch matcher) =>
        k =>
            k.GetType().GetProperty("Rovu")!.GetMethod!.Invoke(k, [])
                .ExprAs(
                rovuObj =>
                    rovuObj switch
                    {
                        IRovu rovu => rovu.FZOTypeInfo(matcher).AsOk(Hint<AbstractRovuInfo>.HINT),
                        IAbstractRovu abstractRovu => abstractRovu.FZOTypeInfo(matcher).AsErr(Hint<RovuInfo>.HINT),
                        _ => throw new Exception("Rovu is not IRovu or IAbstractRovu?")
                    });



    private static Func<Kor, VarovuInfo> VarovuInfoGetter(FZOTypeMatch matcher) =>
        k =>
            k.GetType().GetProperty("Varovu")!.GetMethod!.Invoke(k, [])
                .ExprAs(
                varovuObj =>
                    varovuObj is IVarovu varovu
                        ? varovu.FZOTypeInfo(matcher)
                        : throw new Exception("Varovu is not IVarovu?"));

    private static Func<Kor, RodaInfo> RodaInfoGetter(FZOTypeMatch matcher) =>
        k =>
            k.GetType().GetProperty("Roda")!.GetMethod!.Invoke(k, [])
                .ExprAs(
                rodaObj =>
                    rodaObj is Addr roda
                        ? roda.FZOTypeInfo(matcher)
                        : throw new Exception("Roda is not IRoda?"));
    private static Func<Type, FZOTypeMatch, IKorssaType> SimpleKorssa(IKorssaType typeObj) => (_, _) => typeObj;
    private static Func<Type, FZOTypeMatch, IRoggiType> SimpleRoggi(IRoggiType typeObj) => (_, _) => typeObj;
}