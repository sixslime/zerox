namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;
public class FZOTypeMatch
{
    private List<ITypeMatcher> _matchers = new();
    public IEnumerable<ITypeMatcher> Matchers => _matchers;
    public void AddMatcher(ITypeMatcher matcher)
    {
        _matchers.Add(matcher);
    }

    public enum EGetDynamicTypeError
    {
        NotFZOType,
        UnsupportedType
    }
    public IResult<IFZOType, EGetDynamicTypeError> GetFZOTypeDynamic(Type type)
    {
        throw new NotImplementedException();
    }
    public IOption<IKorssaType> GetKorssaType(Kor korssa)
    {
        throw new NotImplementedException();
    }

    public IOption<IRoggiType> GetRoggiType(Rog roggi)
    {
        throw new NotImplementedException();
    }

    public IOption<IRovetuType> GetRovetuType(IRoveggi<IRovetu> roveggi)
    {
        throw new NotImplementedException();
    }

    public RovuInfo GetRovuInfo(IRovu rovu)
    {
        throw new NotImplementedException();
    }
    public AbstractRovuInfo GetRovuInfo(IAbstractRovu abstractRovu)
    {
        throw new NotImplementedException();
    }
    public VarovuInfo GetVarovuInfo(IVarovu varovu)
    {
        throw new NotImplementedException();
    }
}