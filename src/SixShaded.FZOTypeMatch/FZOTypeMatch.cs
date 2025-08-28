namespace SixShaded.FZOTypeMatch;

using System.Reflection;
using FourZeroOne.Roveggi.Unsafe;
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
        
        return new()
        {
            Origin = systemType,
            MatchedType = _matchers.Map(x => CallMatcherMethod<IKorssaType>(x, _unclosedKorssaMethod, systemType, _getMethodCallArgs)).Filtered().GetAt(0)
        };
    }

    
    private RoggiTypeInfo CalculateRoggiInfo(Type systemType)
    {
        return new()
        {
            Origin = systemType,
            MatchedType = _matchers.Map(x => CallMatcherMethod<IRoggiType>(x, _unclosedRoggiMethod, systemType, _getMethodCallArgs)).Filtered().GetAt(0)
        };
    }
    private RovetuTypeInfo CalculateRovetuInfo(Type systemType)
    {
        return new()
        {
            Origin = systemType,
            MatchedType = _matchers.Map(x => CallMatcherMethod<IRovetuType>(x, _unclosedRovetuMethod, systemType, _getMethodCallArgs)).Filtered().GetAt(0)
        };
    }

    private static MethodInfo _unclosedKorssaMethod = typeof(ITypeMatcher).GetMethod("GetKorssaType")!;
    private static MethodInfo _unclosedRoggiMethod = typeof(ITypeMatcher).GetMethod("GetRoggiType")!;
    private static MethodInfo _unclosedRovetuMethod = typeof(ITypeMatcher).GetMethod("GetRovetuType")!;

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