namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;
public interface ITypeMatcher
{
    public IKorssaType GetKorssaType(Kor korssa);
    public IRoggiType GetRoggiType(Rog roggi);
    public IRovetuType GetRovetuType<C>()
        where C : IRovetu;
}