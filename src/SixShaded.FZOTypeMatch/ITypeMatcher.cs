namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;
public interface ITypeMatcher
{
    public IKorssaType GetKorssaType<K>(FZOTypeMatch caller)
        where K : Kor;

    public IRoggiType GetRoggiType<R>(FZOTypeMatch caller)
        where R : Rog;

    public IRovetuType GetRovetuType<C>(FZOTypeMatch caller)
        where C : IRovetu;
}