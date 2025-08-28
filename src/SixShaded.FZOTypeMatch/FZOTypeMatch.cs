namespace SixShaded.FZOTypeMatch;

using System.Reflection;
using FourZeroOne.Roveggi.Unsafe;

// really really shitty code design bro.
public class FZOTypeMatch
{
    private readonly List<ITypeMatcher> _matchers = new();
    private readonly Dictionary<Type, IFZOTypeInfo<IFZOType>> _typeMap = new();
    private readonly object[] _getMethodCallArgs;

    public FZOTypeMatch()
    {
        _getMethodCallArgs = [this];
    }
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
        if (o.Check(out var typeInfo))
            _typeMap[systemType] = typeInfo;
        return o;
    }
    public KorssaTypeInfo GetKorssaTypeInfo(Kor korssa)
    {
        return (KorssaTypeInfo)(GetFZOTypeInfoDynamic(korssa.GetType()).Unwrap());
    }

    public RoggiTypeInfo GetRoggiTypeInfo(Rog roggi)
    {
        return (RoggiTypeInfo)(GetFZOTypeInfoDynamic(roggi.GetType()).Unwrap());
    }

    public RovetuTypeInfo GetRovetuTypeInfo(IRoveggi<IRovetu> roveggi)
    {
        return (RovetuTypeInfo)(GetFZOTypeInfoDynamic(roveggi.GetType()).Unwrap());
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
        return new()
        {
            Origin = systemType,
            MatchedType = _matchers.Map(x => CallMatcherMethod<IKorssaType>(x, INTERFACE_KORSSA_METHOD, systemType, _getMethodCallArgs)).Filtered().GetAt(0)
        };
    }

    
    private RoggiTypeInfo CalculateRoggiInfo(Type systemType)
    {
        return new()
        {
            Origin = systemType,
            MatchedType = _matchers.Map(x => CallMatcherMethod<IRoggiType>(x, INTERFACE_ROGGI_METHOD, systemType, _getMethodCallArgs)).Filtered().GetAt(0)
        };
    }
    private RovetuTypeInfo CalculateRovetuInfo(Type systemType)
    {
        return new()
        {
            Origin = systemType,
            MatchedType = _matchers.Map(x => CallMatcherMethod<IRovetuType>(x, INTERFACE_ROVETU_METHOD, systemType, _getMethodCallArgs)).Filtered().GetAt(0)
        };
    }

    private static readonly MethodInfo INTERFACE_KORSSA_METHOD = typeof(ITypeMatcher).GetMethod("GetKorssaType")!;
    private static readonly MethodInfo INTERFACE_ROGGI_METHOD = typeof(ITypeMatcher).GetMethod("GetRoggiType")!;
    private static readonly MethodInfo INTERFACE_ROVETU_METHOD = typeof(ITypeMatcher).GetMethod("GetRovetuType")!;

    // this is scary bro.
    private static IOption<T> CallMatcherMethod<T>(ITypeMatcher matcher, MethodInfo interfaceMethod, Type type, object[] callArgs)
        where T : IFZOType
    {
        var interfaceMap = matcher.GetType().GetInterfaceMap(typeof(ITypeMatcher));
        int methodIndex = Array.FindIndex(interfaceMap.InterfaceMethods, method => method == interfaceMethod);
        if (methodIndex == -1)
            throw new Exception($"cannot find unclosed {interfaceMethod.Name} method in interface map.");
        return (IOption<T>)(interfaceMap.TargetMethods[methodIndex].MakeGenericMethod(type).Invoke(matcher, callArgs)!);
    }
}