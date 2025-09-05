namespace SixShaded.CoreTypeMatcher;

using FZOTypeMatch;
using Rt = FourZeroOne.Core.Roggis;
using Kt = FourZeroOne.Core.Korssas;
using Km = Types.Korssa;
using FourZeroOne.Core;


public class CoreTypeMatcher : ITypeMatcher
{
    public static CoreTypeMatcher Matcher { get; } = new();

    private CoreTypeMatcher()
    {

    }

    public IOption<IKorssaType> GetKorssaType<K>(FZOTypeMatch caller)
        where K : Kor
    {
        IKorssaType? o = typeof(K) switch
        {
            var t when t == typeof(Kt.Fixed<>) =>
                new Km.Fixed
                {
                    RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(caller).Unwrap()
                },
            var t when t == typeof(Kt.Number.Add) => new Km.Number.Add(),
            _ => null
        };
        return o.NullToNone();
    }


    public IOption<IRoggiType> GetRoggiType<R>(FZOTypeMatch caller)
        where R : Rog =>
        new None<IRoggiType>();

    public IOption<IRovetuType> GetRovetuType<C>(FZOTypeMatch caller)
        where C : IRovetu =>
        new None<IRovetuType>();
}