namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;
public class FZOTypeMatch
{
    private readonly List<ITypeMatcher> _matchers = new();
    private readonly Dictionary<Type, IFZOType> _typeMap = new();
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
    public IResult<IFZOType, EGetDynamicTypeError> GetFZOTypeDynamic(Type systemType)
    {
        if (_typeMap.TryGetValue(systemType, out var cachedValue))
            return cachedValue.AsOk(Hint<EGetDynamicTypeError>.HINT);

        var o = CalculateDynamicType(systemType).Check(out var calcResult)
            ? calcResult.OrErr(EGetDynamicTypeError.UnsupportedType)
            : EGetDynamicTypeError.NotFZOType.AsErr(Hint<IFZOType>.HINT);
        if (o.CheckOk(out var success))
            _typeMap[systemType] = success;
        return o;
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

    private IOption<IOption<IFZOType>> CalculateDynamicType(Type systemType)
    {
        if (systemType.IsAssignableTo(typeof(Kor)))
            return CalculateKorssaType(systemType).AsSome();
        if (systemType.IsAssignableTo(typeof(Rog)))
            return CalculateRoggiType(systemType).AsSome();
        if (systemType.IsAssignableTo(typeof(IRoveggi<>)))
            return CalculateRovetuType(systemType.GenericTypeArguments[0]).AsSome();
        if (systemType.IsAssignableTo(typeof(IRovetu)))
            return CalculateRovetuType(systemType).AsSome();

        return new None<IOption<IFZOType>>();
    }
    private IOption<IKorssaType> CalculateKorssaType(Type systemType)
    {
        throw new NotImplementedException();
    }
    private IOption<IRoggiType> CalculateRoggiType(Type systemType)
    {
        throw new NotImplementedException();
    }
    private IOption<IRovetuType> CalculateRovetuType(Type systemType)
    {
        throw new NotImplementedException();
    }
}