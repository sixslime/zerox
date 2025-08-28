namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;
public class FZOTypeMatch
{
    private readonly List<ITypeMatcher> _matchers = new();
    private readonly Dictionary<Type, IFZOTypeInfo<IFZOType>> _typeMap = new();
    public IEnumerable<ITypeMatcher> Matchers => _matchers;
    public void AddMatcher(ITypeMatcher matcher)
    {
        _matchers.Add(matcher);
    }

    public IOption<IFZOTypeInfo<IFZOType>> GetFZOTypeInfoDynamic(Type systemType)
    {
        if (_typeMap.TryGetValue(systemType, out var cachedValue))
            return cachedValue.AsSome();

        var o = CalculateDynamicType(systemType);
        if (o.Check(out var success))
            _typeMap[systemType] = success;
        return o;
    }
    public KorssaTypeInfo GetKorssaTypeInfo(Kor korssa)
    {
        throw new NotImplementedException();
    }

    public RoggiTypeInfo GetRoggiTypeInfo(Rog roggi)
    {
        throw new NotImplementedException();
    }

    public RovetuTypeInfo GetRovetuTypeInfo(IRoveggi<IRovetu> roveggi)
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

    private IOption<IFZOTypeInfo<IFZOType>> CalculateDynamicType(Type systemType)
    {
        if (systemType.IsAssignableTo(typeof(Kor)))
            return CalculateKorssaInfo(systemType).AsSome();
        if (systemType.IsAssignableTo(typeof(Rog)))
            return CalculateRoggiInfo(systemType).AsSome();
        if (systemType.IsAssignableTo(typeof(IRoveggi<>)))
            return CalculateRovetuInfo(systemType.GenericTypeArguments[0]).AsSome();
        if (systemType.IsAssignableTo(typeof(IRovetu)))
            return CalculateRovetuInfo(systemType).AsSome();

        return new None<IFZOTypeInfo<IFZOType>>();
    }
    private KorssaTypeInfo CalculateKorssaInfo(Type systemType)
    {
        throw new NotImplementedException();
    }
    private RoggiTypeInfo CalculateRoggiInfo(Type systemType)
    {
        throw new NotImplementedException();
    }
    private RovetuTypeInfo CalculateRovetuInfo(Type systemType)
    {
        throw new NotImplementedException();
    }
}