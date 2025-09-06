namespace SixShaded.CoreTypeMatcher;

using FZOTypeMatch;
using Rt = FourZeroOne.Core.Roggis;
using Rm = Types.Roggi;
using Kt = FourZeroOne.Core.Korssas;
using Km = Types.Korssa;
using FourZeroOne.Core;
using FourZeroOne.Roveggi.Unsafe;

public class CoreTypeMatcher : ITypeMatcher
{
    // TODO: Korvessas
    private static readonly Dictionary<Type, Func<Type, FZOTypeMatch, IKorssaType>> KORSSA_MAP =
        new()
        {
            {
                typeof(Kt.Exists), SimpleKorssa(new Km.Exists())
            },
            {
                typeof(Kt.IsEqual), SimpleKorssa(new Km.IsEqual())
            },
            {
                typeof(Kt.AddMellsano), SimpleKorssa(new Km.AddMellsano())
            },
            {
                typeof(Kt.Fixed<>), (t, c) =>
                    new Km.Fixed
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Nolla<>), (t, c) =>
                    new Km.Nolla
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.TryCast<>), (t, c) =>
                    new Km.TryCast
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.IfElse<>), (t, c) =>
                    new Km.IfElse
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.SubEnvironment<>), (t, c) =>
                    new Km.SubEnvironment
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.MetaExecuted<>), (t, c) =>
                    new Km.MetaExecuted
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = []
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[4].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..4].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.Execute<>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = []
                    }
            },
            {
                typeof(Kt.Execute<,>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.Execute<,,>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.Execute<,,,>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.ToBoxedArgs<>), (t, c) =>
                    new Km.ToBoxedArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.ToBoxedArgs<,>), (t, c) =>
                    new Km.ToBoxedArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.ToBoxedArgs<,,>), (t, c) =>
                    new Km.ToBoxedArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.Number.Add), SimpleKorssa(new Km.Number.Add())
            },
            {
                typeof(Kt.Number.Modulo), SimpleKorssa(new Km.Number.Modulo())
            },
            {
                typeof(Kt.Number.Subtract), SimpleKorssa(new Km.Number.Subtract())
            },
            {
                typeof(Kt.Number.Multiply), SimpleKorssa(new Km.Number.Multiply())
            },
            {
                typeof(Kt.Number.GreaterThan), SimpleKorssa(new Km.Number.GreaterThan())
            },
            {
                typeof(Kt.Number.Divide), SimpleKorssa(new Km.Number.Divide())
            },
            {
                typeof(Kt.Bool.And), SimpleKorssa(new Km.Bool.And())
            },
            {
                typeof(Kt.Bool.Or), SimpleKorssa(new Km.Bool.Or())
            },
            {
                typeof(Kt.Bool.Not), SimpleKorssa(new Km.Bool.Not())
            },
            {
                typeof(Kt.IO.SelectOne<>), (t, c) =>
                    new Km.IO.SelectOne
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.IO.SelectMultiple<>), (t, c) =>
                    new Km.IO.SelectMultiple
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Assign<>), (t, c) =>
                    new Km.Memory.Assign()
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Reference<>), (t, c) =>
                    new Km.Memory.Reference()
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.ProgramState.Get), SimpleKorssa(new Km.Memory.ProgramState.Get())
            },
            {
                typeof(Kt.Memory.ProgramState.Load), SimpleKorssa(new Km.Memory.ProgramState.Load())
            },
            {
                typeof(Kt.Memory.Rovedanggi.AllKeys<,>), (t, c) =>
                    new Km.Memory.Rovedanggi.AllKeys()
                    {
                        RovedantuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Rovedanggi.AllValues<,>), (t, c) =>
                    new Km.Memory.Rovedanggi.AllValues()
                    {
                        RovedantuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Rovedanggi.Read<>), (t, c) =>
                    new Km.Memory.Rovedanggi.Read()
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Rovedanggi.Write<>), (t, c) =>
                    new Km.Memory.Rovedanggi.Write()
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Clean<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Contains<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Create<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Distinct<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Except<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Flatten<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.GetIndex<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.GetSlice<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.IndiciesOf<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Intersect<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Reverse<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Union<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Yield<>), (t, c) =>
                    new Km.Multi.Clean
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Multi.Count), SimpleKorssa(new Km.Multi.Count())
            },
            {
                typeof(Kt.Range.Create), SimpleKorssa(new Km.Range.Create())
            },
            {
                typeof(Kt.Range.Get.Start), SimpleKorssa(new Km.Range.Get.Start())
            },
            {
                typeof(Kt.Range.Get.End), SimpleKorssa(new Km.Range.Get.End())
            },
            {
                typeof(Kt.Rovi.Get<,>), (t, c) =>
                    new Km.Rovi.Get
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        RovuInfoGetter = RovuInfoGetter(c)
                    }
            },
            {
                typeof(Kt.Rovi.With<,>), (t, c) =>
                    new Km.Rovi.Get
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        RovuInfoGetter = RovuInfoGetter(c)
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.Get<,,>), (t, c) =>
                    new Km.Rovi.Varovi.Get
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c)
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.With<,,>), (t, c) =>
                    new Km.Rovi.Varovi.With
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c)
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.GetKeys<,,>), (t, c) =>
                    new Km.Rovi.Varovi.GetKeys
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c)
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.GetValues<,,>), (t, c) =>
                    new Km.Rovi.Varovi.GetValues
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c)
                    }
            },
        };

    private static readonly Dictionary<Type, Func<Type, FZOTypeMatch, IRoggiType>> ROGGI_MAP =
        new()
        {
            {
                typeof(Rt.Instructions.MellsanoAdd), SimpleRoggi(new Rm.Instructions.MellsanoAdd())
            },
            {
                typeof(Rt.Instructions.LoadProgramState), SimpleRoggi(new Rm.Instructions.LoadProgramState())
            },
            {
                typeof(Rt.Instructions.Assign<>), (t, c) =>
                    new Rm.Instructions.Assign()
                    {
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Rt.Bool), SimpleRoggi(new Rm.Bool())
            },
            {
                typeof(Rt.Number), SimpleRoggi(new Rm.Number())
            },
            {
                typeof(Rt.NumRange), SimpleRoggi(new Rm.NumRange())
            },
            {
                typeof(Rt.ProgramState), SimpleRoggi(new Rm.ProgramState())
            },
            {
                typeof(Rt.Multi<>), (t, c) =>
                    new Rm.Multi
                    {
                        ElementType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(IMulti<>), (t, c) =>
                    new Rm.IMulti
                    {
                        ElementType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(IRoveggi<>), (t, c) =>
                    new Rm.Roveggi
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Roveggi<>), (t, c) =>
                    new Rm.Roveggi
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Rt.MetaArgs<>), (t, c) =>
                    new Rm.MetaArgs()
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Rt.MetaArgs<,>), (t, c) =>
                    new Rm.MetaArgs()
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Rt.MetaArgs<,,>), (t, c) =>
                    new Rm.MetaArgs()
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Rt.MetaFunction<>), (t, c) =>
                    new Rm.MetaFunction()
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = []
                    }
            },
            {
                typeof(Rt.MetaFunction<,>), (t, c) =>
                    new Rm.MetaFunction()
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Rt.MetaFunction<,,>), (t, c) =>
                    new Rm.MetaFunction()
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Rt.MetaFunction<,,,>), (t, c) =>
                    new Rm.MetaFunction()
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Rt.OverflowingMetaFunction<,,,,>), (t, c) =>
                    new Rm.MetaFunction()
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[4].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..4].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
        };
    private static Func<Kor, IResult<RovuInfo, AbstractRovuInfo>> RovuInfoGetter(FZOTypeMatch matcher) =>
        k =>
            k.GetType().GetProperty("Rovu")!.GetMethod!.Invoke(k, [])
                .ExprAs(
                rovuObj =>
                    rovuObj switch
                    {
                        IRovu rovu => rovu.FZOTypeInfo(matcher).AsOk(Hint<AbstractRovuInfo>.HINT),
                        IAbstractRovu abstractRovu => abstractRovu.FZOTypeInfo(matcher).AsErr(Hint<RovuInfo>.HINT),
                        _ => throw new Exception("Rovu is not IRovu or IAbstractRovu?")
                    });

    private static Func<Kor, VarovuInfo> VarovuInfoGetter(FZOTypeMatch matcher) =>
        k =>
            k.GetType().GetProperty("Varovu")!.GetMethod!.Invoke(k, [])
                .ExprAs(
                varovuObj =>
                    varovuObj is IVarovu varovu
                        ? varovu.FZOTypeInfo(matcher)
                        : throw new Exception("Varovu is not IVarovu?"));
    private static Func<Type, FZOTypeMatch, IKorssaType> SimpleKorssa(IKorssaType typeObj) => (_, _) => typeObj;
    private static Func<Type, FZOTypeMatch, IRoggiType> SimpleRoggi(IRoggiType typeObj) => (_, _) => typeObj;

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