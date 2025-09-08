namespace SixShaded.FZOTypeMatch;

using System.Reflection;
using FourZeroOne.Roveggi.Unsafe;

// really really shitty code design bro.
public class FZOTypeMatch
{
    private static readonly MethodInfo INTERFACE_KORSSA_METHOD = typeof(ITypeMatcher).GetMethod("GetKorssaType")!;
    private static readonly MethodInfo INTERFACE_ROGGI_METHOD = typeof(ITypeMatcher).GetMethod("GetRoggiType")!;
    private static readonly MethodInfo INTERFACE_ROVETU_METHOD = typeof(ITypeMatcher).GetMethod("GetRovetuType")!;
    private readonly object[] _getMethodCallArgs;
    private readonly ITypeMatcher[] _matchers;
    private readonly Dictionary<Type, IFZOTypeInfo<IFZOType>> _typeMap = new();

    public FZOTypeMatch(ITypeMatcher[] matchers)
    {
        _getMethodCallArgs = [this];
        _matchers = matchers;
    }

    public IEnumerable<ITypeMatcher> Matchers => _matchers;

    public IOption<IFZOTypeInfo<IFZOType>> GetFZOTypeInfoDynamic(Type systemType)
    {
        if (_typeMap.TryGetValue(systemType, out var cachedValue))
            return cachedValue.AsSome();
        var o = CalculateDynamicType(systemType);
        if (o.Check(out var typeInfo))
            _typeMap[systemType] = typeInfo;
        return o;
    }

    public KorssaTypeInfo GetKorssaTypeInfo(Kor korssa) => (KorssaTypeInfo)GetFZOTypeInfoDynamic(korssa.GetType()).Unwrap();
    public RoggiTypeInfo GetRoggiTypeInfo(Rog roggi) => (RoggiTypeInfo)GetFZOTypeInfoDynamic(roggi.GetType()).Unwrap();
    public RovetuTypeInfo GetRovetuTypeInfo(IRoveggi<IRovetu> roveggi) => (RovetuTypeInfo)GetFZOTypeInfoDynamic(roveggi.GetType()).Unwrap();

    public RovuInfo GetRovuInfo(IRovu rovu)
    {
        var compType = typeof(IRovu<,>);
        var rovuGenerics =
            rovu.GetType()
                .GetInterfaces()
                .FirstMatch(x => x.IsGenericType && x.GetGenericTypeDefinition() == compType)
                .Expect($"{rovu} implements Unsafe.IRovu but not Rovu<C, R>?")
                .GenericTypeArguments;
        return new()
        {
            Rovu = rovu,
            RovetuType = (RovetuTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[0]).Unwrap(),
            DataType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[1]).Unwrap(),
        };
    }

    public AbstractRovuInfo GetRovuInfo(IAbstractRovu abstractRovu)
    {
        var compTypeSet = typeof(ISetRovu<,>);
        var compTypeGet = typeof(IGetRovu<,>);

        // bs but whatever
        (bool isGet, var rovuGenerics) =
            abstractRovu.GetType()
                .GetInterfaces()
                .FilterMap(x => x.IsGenericType.ToOptionLazy(x.GetGenericTypeDefinition))
                .FirstMatch(x => x == compTypeGet || x == compTypeSet)
                .Expect($"{abstractRovu} implements Unsafe.IAbstractRovu but not IGetRovu<C, R> or ISetRovu<C, R>?")
                .ExprAs(
                type =>
                    (type == compTypeGet, type.GenericTypeArguments));
        return new()
        {
            Rovu = abstractRovu,
            RovetuType = (RovetuTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[0]).Unwrap(),
            DataType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[1]).Unwrap(),
            Interaction = isGet ? AbstractRovuInfo.EInteraction.Get : AbstractRovuInfo.EInteraction.Set,
        };
    }

    public VarovuInfo GetVarovuInfo(IVarovu varovu)
    {
        var compType = typeof(IVarovu<,,>);
        var rovuGenerics =
            varovu.GetType()
                .GetInterfaces()
                .FirstMatch(x => x.IsGenericType && x.GetGenericTypeDefinition() == compType)
                .Expect($"{varovu} implements Unsafe.IVarovu but not IVarovu<C, RKey, RVal>?")
                .GenericTypeArguments;
        return new()
        {
            Varovu = varovu,
            RovetuType = (RovetuTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[0]).Unwrap(),
            KeyType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[1]).Unwrap(),
            DataType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(rovuGenerics[2]).Unwrap(),
        };
    }

    public RodaInfo GetRodaInfo(Addr roda)
    {
        var compType = typeof(IRoda<>);
        var rodaGenerics =
            roda.GetType()
                .GetInterfaces()
                .FirstMatch(x => x.IsGenericType && x.GetGenericTypeDefinition() == compType)
                .Expect("Logically unreachable")
                .GenericTypeArguments;
        return new()
        {
            Roda = roda,
            DataType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(rodaGenerics[0]).Unwrap(),
        };
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
        // this is so fucking retarded guys.
        RoggiTypeInfo?[] argTypes =
        [
            null,
            null,
            null,
        ];
        RoggiTypeInfo? outType = null;
        IOption<RoggiTypeInfo> combinerOpt = new None<RoggiTypeInfo>();
        foreach (var t in systemType.GetInterfaces())
        {
            if (!t.IsGenericType) continue;
            var openDef = t.GetGenericTypeDefinition();
            var generics = t.GenericTypeArguments;

            // DEV: assumes that all IHasArgs<...> implements previous IHasArgs.
            // does not use IHasNoArgs.
            if (openDef == typeof(IKorssa<>)) outType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(generics[0]).Expect("???");
            else if (openDef == typeof(IHasArgs<,>)) argTypes[0] = (RoggiTypeInfo)GetFZOTypeInfoDynamic(generics[0]).Expect("???");
            else if (openDef == typeof(IHasArgs<,,>)) argTypes[1] = (RoggiTypeInfo)GetFZOTypeInfoDynamic(generics[1]).Expect("???");
            else if (openDef == typeof(IHasArgs<,,,>)) argTypes[2] = (RoggiTypeInfo)GetFZOTypeInfoDynamic(generics[2]).Expect("???");
            else if (openDef == typeof(IHasCombinerArgs<,>))
            {
                combinerOpt = ((RoggiTypeInfo)GetFZOTypeInfoDynamic(generics[0]).Expect("???")).AsSome();
            }
        }
        return new()
        {
            Origin = systemType,
            Match = _matchers.Map(x => CallMatcherMethod<IKorssaType>(x, INTERFACE_KORSSA_METHOD, systemType, _getMethodCallArgs)).Filtered().GetAt(0),
            ResultType = outType!,
            ArgTypes = combinerOpt.Check(out var combinerType)
                ? new KorssaTypeInfo.EArgTypes.Combiner()
                {
                    Type = combinerType
                }
                : new KorssaTypeInfo.EArgTypes.Positional()
                {
                    Types = argTypes.FilterMap(x => x.NullToNone()).ToArray()
                }
        };
    }

    private RoggiTypeInfo CalculateRoggiInfo(Type systemType) =>
        new()
        {
            Origin = systemType,
            Match = _matchers.Map(x => CallMatcherMethod<IRoggiType>(x, INTERFACE_ROGGI_METHOD, systemType, _getMethodCallArgs)).Filtered().GetAt(0),
        };

    private RovetuTypeInfo CalculateRovetuInfo(Type systemType)
    {
        var matchedType = _matchers.Map(x => CallMatcherMethod<IRovetuType>(x, INTERFACE_ROVETU_METHOD, systemType, _getMethodCallArgs)).Filtered().GetAt(0);
        RoggiTypeInfo? memoryDataType = null;
        for (var baseType = systemType.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Rovedantu<>))
            {
                memoryDataType = (RoggiTypeInfo)GetFZOTypeInfoDynamic(baseType.GenericTypeArguments[0]).Expect("???");
                break;
            }
        }
        return new()
        {
            Origin = systemType,
            Match = matchedType,
            MemoryDataType = memoryDataType.NullToNone(),
        };
    }

    // this is scary bro.
    private static IOption<T> CallMatcherMethod<T>(ITypeMatcher matcher, MethodInfo interfaceMethod, Type type, object[] callArgs)
        where T : IFZOType
    {
        var interfaceMap = matcher.GetType().GetInterfaceMap(typeof(ITypeMatcher));
        int methodIndex = Array.FindIndex(interfaceMap.InterfaceMethods, method => method == interfaceMethod);
        if (methodIndex == -1)
            throw new($"cannot find unclosed {interfaceMethod.Name} method in interface map.");
        return (IOption<T>)interfaceMap.TargetMethods[methodIndex].MakeGenericMethod(type).Invoke(matcher, callArgs)!;
    }
}