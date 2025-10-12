namespace SixShaded.CoreTypeMatcher;

using FZOTypeMatch;
using Rt = FourZeroOne.Core.Roggis;
using Rm = Types.Roggi;

internal static partial class Maps
{
    public static Dictionary<Type, Func<Type, FZOTypeMatch, IRoggiType>> Roggi =>
        new()
        {
            {
                typeof(Rog), SimpleRoggi(new Rm.Any())
            },
            {
                typeof(Rt.Instructions.MellsanoAdd), SimpleRoggi(new Rm.Instructions.MellsanoAdd()
                {
                    MellsanoGetter = r => r.IsA<Rt.Instructions.MellsanoAdd>().Mellsano
                })
            },
            {
                typeof(Rt.Instructions.LoadProgramState), SimpleRoggi(new Rm.Instructions.LoadProgramState()
                {
                    ProgramStateGetter = r => r.IsA<Rt.Instructions.LoadProgramState>().State
                })
            },
            {
                typeof(Rt.Instructions.Assign<>), (t, c) =>
                    new Rm.Instructions.Assign
                    {
                        DataGetter = PropertyGetter<Rog, RogOpt>("Data"),
                        RodaGetter = PropertyGetter<Rog, Addr>("Roda"),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Bool), SimpleRoggi(new Rm.Bool()
                {
                    ValueGetter = r => r.IsA<Rt.Bool>().IsTrue
                })
            },
            {
                typeof(Number), SimpleRoggi(new Rm.Number()
                {
                    ValueGetter = r => r.IsA<Rt.Number>().Value
                })
            },
            {
                typeof(NumRange), SimpleRoggi(new Rm.NumRange()
                {
                    StartGetter = r => r.IsA<Rt.NumRange>().Start,
                    EndGetter = r => r.IsA<Rt.NumRange>().End,
                })
            },
            {
                typeof(ProgramState), SimpleRoggi(new Rm.ProgramState()
                {
                    MemoryGetter = r => r.IsA<Rt.ProgramState>().Memory.InternalValue
                })
            },
            {
                typeof(Multi<>), (t, c) =>
                    new Rm.Multi
                    {
                        ElementType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        ElementsGetter = PropertyGetter<Rog, IEnumerable<RogOpt>>("Elements"),
                    }
            },
            {
                typeof(IMulti<>), (t, c) =>
                    new Rm.IMulti
                    {
                        ElementType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        ElementsGetter = PropertyGetter<Rog, IEnumerable<RogOpt>>("Elements")
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
                typeof(MetaArgs<>), (t, c) =>
                    new Rm.MetaArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        ArgsGetter = r => r.IsA<FourZeroOne.Roggi.Unsafe.IMetaArgs>().Args,
                    }
            },
            {
                typeof(MetaArgs<,>), (t, c) =>
                    new Rm.MetaArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        ArgsGetter = r => r.IsA<FourZeroOne.Roggi.Unsafe.IMetaArgs>().Args,
                    }
            },
            {
                typeof(MetaArgs<,,>), (t, c) =>
                    new Rm.MetaArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        ArgsGetter = r => r.IsA<FourZeroOne.Roggi.Unsafe.IMetaArgs>().Args,
                    }
            },
            {
                typeof(MetaFunction<>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = [],
                        MetaKorssaGetter = PropertyGetter<Rog, Kor>("Korssa"),
                        SelfRodaGetter = PropertyGetter<Rog, Addr>("SelfRoda"),
                        ArgRodasGetter = PropertyGetter<Rog, Addr[]>("ArgAddresses"),
                        CapturedRodasGetter = PropertyGetter<Rog, Addr[]>("CapturedVariables"),
                        CapturedMemoryGetter = r => PropertyGetter<Rog, FourZeroOne.Handles.IMemory>("ArgAddresses")(r).InternalValue,
                    }
            },
            {
                typeof(MetaFunction<,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        MetaKorssaGetter = PropertyGetter<Rog, Kor>("Korssa"),
                        SelfRodaGetter = PropertyGetter<Rog, Addr>("SelfRoda"),
                        ArgRodasGetter = PropertyGetter<Rog, Addr[]>("ArgAddresses"),
                        CapturedRodasGetter = PropertyGetter<Rog, Addr[]>("CapturedVariables"),
                        CapturedMemoryGetter = r => PropertyGetter<Rog, FourZeroOne.Handles.IMemory>("ArgAddresses")(r).InternalValue,
                    }
            },
            {
                typeof(MetaFunction<,,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        MetaKorssaGetter = PropertyGetter<Rog, Kor>("Korssa"),
                        SelfRodaGetter = PropertyGetter<Rog, Addr>("SelfRoda"),
                        ArgRodasGetter = PropertyGetter<Rog, Addr[]>("ArgAddresses"),
                        CapturedRodasGetter = PropertyGetter<Rog, Addr[]>("CapturedVariables"),
                        CapturedMemoryGetter = r => PropertyGetter<Rog, FourZeroOne.Handles.IMemory>("ArgAddresses")(r).InternalValue,
                    }
            },
            {
                typeof(MetaFunction<,,,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        MetaKorssaGetter = PropertyGetter<Rog, Kor>("Korssa"),
                        SelfRodaGetter = PropertyGetter<Rog, Addr>("SelfRoda"),
                        ArgRodasGetter = PropertyGetter<Rog, Addr[]>("ArgAddresses"),
                        CapturedRodasGetter = PropertyGetter<Rog, Addr[]>("CapturedVariables"),
                        CapturedMemoryGetter = r => PropertyGetter<Rog, FourZeroOne.Handles.IMemory>("ArgAddresses")(r).InternalValue,
                    }
            },
            {
                typeof(OverflowingMetaFunction<,,,,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[4].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..4].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                        MetaKorssaGetter = PropertyGetter<Rog, Kor>("Korssa"),
                        SelfRodaGetter = PropertyGetter<Rog, Addr>("SelfRoda"),
                        ArgRodasGetter = PropertyGetter<Rog, Addr[]>("ArgAddresses"),
                        CapturedRodasGetter = PropertyGetter<Rog, Addr[]>("CapturedVariables"),
                        CapturedMemoryGetter = r => PropertyGetter<Rog, FourZeroOne.Handles.IMemory>("ArgAddresses")(r).InternalValue,
                    }
            },
        };
}