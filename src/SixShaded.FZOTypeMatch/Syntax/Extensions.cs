namespace SixShaded.FZOTypeMatch.Syntax;

using FourZeroOne.Roveggi.Unsafe;

public static class Extensions
{
    public static KorssaTypeInfo FZOTypeInfo(this Kor korssa, FZOTypeMatch typeMatch) => typeMatch.GetKorssaTypeInfo(korssa);
    public static RoggiTypeInfo FZOTypeInfo(this Rog roggi, FZOTypeMatch typeMatch) => typeMatch.GetRoggiTypeInfo(roggi);
    public static RovetuTypeInfo FZOTypeInfo(this IRoveggi<IRovetu> roveggi, FZOTypeMatch typeMatch) => typeMatch.GetRovetuTypeInfo(roveggi);
    public static RodaInfo FZOTypeInfo(this Addr roda, FZOTypeMatch typeMatch) => typeMatch.GetRodaInfo(roda);
    public static RovuInfo FZOTypeInfo(this IRovu rovu, FZOTypeMatch typeMatch) => typeMatch.GetRovuInfo(rovu);
    public static AbstractRovuInfo FZOTypeInfo(this IAbstractRovu rovu, FZOTypeMatch typeMatch) => typeMatch.GetRovuInfo(rovu);
    public static VarovuInfo FZOTypeInfo(this IVarovu varovu, FZOTypeMatch typeMatch) => typeMatch.GetVarovuInfo(varovu);
    public static IOption<IFZOTypeInfo<IFZOType>> TryGetFZOTypeInfo(this Type type, FZOTypeMatch typeMatch) => typeMatch.GetFZOTypeInfoDynamic(type);
}