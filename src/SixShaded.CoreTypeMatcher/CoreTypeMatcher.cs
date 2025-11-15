namespace SixShaded.CoreTypeMatcher;

using FZOTypeMatch;
using FourZeroOne.Core;
using FourZeroOne.Roveggi.Unsafe;

public class CoreTypeMatcher : ITypeMatcher
{
    private static readonly Dictionary<Type, Func<Type, FZOTypeMatch, IKorssaType>> KORSSA_MAP =
        new(
        [
            ..Maps.Korssa,
            ..Maps.Korvessa,
        ]);
    private static readonly Dictionary<Type, Func<Type, FZOTypeMatch, IRoggiType>> ROGGI_MAP = Maps.Roggi;

    public IOption<IKorssaType> GetKorssaType<K>(FZOTypeMatch caller)
        where K : Kor
    {
        var type = typeof(K);
        return KORSSA_MAP.TryGetValue(type.IsGenericType ? type.GetGenericTypeDefinition() : type, out var generator)
            ? generator(type, caller).AsSome()
            : new None<IKorssaType>();
    }

    public IOption<IRoggiType> GetRoggiType<R>(FZOTypeMatch caller)
        where R : Rog
    {
        var type = typeof(R);
        return ROGGI_MAP.TryGetValue(type.IsGenericType ? type.GetGenericTypeDefinition() : type, out var generator)
            ? generator(type, caller).AsSome()
            : new None<IRoggiType>();
    }

    public IOption<IRovetuType> GetRovetuType<C>(FZOTypeMatch caller)
        where C : IRovetu =>
        new None<IRovetuType>();
}