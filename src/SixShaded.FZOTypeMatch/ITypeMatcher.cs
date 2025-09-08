namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;

public interface ITypeMatcher
{
    public IOption<IKorssaType> GetKorssaType<K>(FZOTypeMatch caller)
        where K : Kor;

    public IOption<IRoggiType> GetRoggiType<R>(FZOTypeMatch caller)
        where R : Rog;

    public IOption<IRovetuType> GetRovetuType<C>(FZOTypeMatch caller)
        where C : IRovetu;
}