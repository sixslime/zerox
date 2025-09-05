namespace SixShaded.CoreTypeMatcher;

using FZOTypeMatch;
using Rt = FourZeroOne.Core.Roggis;
using Kt = FourZeroOne.Core.Korssas;
using Km = Types.Korssa;
using FourZeroOne.Core;

public class CoreTypeMatcher : ITypeMatcher
{
    private static readonly Dictionary<Type, Func<Type, FZOTypeMatch, IKorssaType>> KORSSA_MAP =
        new()
        {
            {
                typeof(Kt.Fixed<>), (t, c) =>
                    new Km.Fixed
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Number.Add), SimpleKorssa(new Km.Number.Add())
            },
            {
                typeof(Kt.Number.Modulo), SimpleKorssa(new Km.Number.Modulo())
            },
            {
                typeof(Kt.Number.Subtract), SimpleKorssa(new Km.Number.Subtract())
            },
            {
                typeof(Kt.Number.Multiply), SimpleKorssa(new Km.Number.Multiply())
            },
            {
                typeof(Kt.Number.GreaterThan), SimpleKorssa(new Km.Number.GreaterThan())
            },
            {
                typeof(Kt.Number.Divide), SimpleKorssa(new Km.Number.Divide())
            },
        };
    private static Func<Type, FZOTypeMatch, IKorssaType> SimpleKorssa(IKorssaType typeObj) => (_, _) => typeObj;

    public IOption<IKorssaType> GetKorssaType<K>(FZOTypeMatch caller)
        where K : Kor
    {
        var type = typeof(K);
        return KORSSA_MAP.TryGetValue(type.IsGenericType ? type.GetGenericTypeDefinition() : type, out var generator)
            ? generator(type, caller).AsSome()
            : new None<IKorssaType>();
    }

    public IOption<IRoggiType> GetRoggiType<R>(FZOTypeMatch caller)
        where R : Rog =>
        new None<IRoggiType>();

    public IOption<IRovetuType> GetRovetuType<C>(FZOTypeMatch caller)
        where C : IRovetu =>
        new None<IRovetuType>();
}