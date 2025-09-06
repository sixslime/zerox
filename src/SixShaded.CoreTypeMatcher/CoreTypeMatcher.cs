namespace SixShaded.CoreTypeMatcher;

using FZOTypeMatch;
using Rt = FourZeroOne.Core.Roggis;
using Kt = FourZeroOne.Core.Korssas;
using Km = Types.Korssa;
using FourZeroOne.Core;

public class CoreTypeMatcher : ITypeMatcher
{
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
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray()
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
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
        };
    private static Func<Type, FZOTypeMatch, IKorssaType> SimpleKorssa(IKorssaType typeObj) => (_, _) => typeObj;

    public IOption<IKorssaType> GetKorssaType<K>(FZOTypeMatch caller)
        where K : Kor
    {
        var type = typeof(K);
        return KORSSA_MAP.TryGetValue(type.IsGenericType ? type.GetGenericTypeDefinition() : type, out var generator)
            ? generator(type, caller).AsSome()
            : new None<IKorssaType>();
    }

    public IOption<IRoggiType> GetRoggiType<R>(FZOTypeMatch caller)
        where R : Rog =>
        new None<IRoggiType>();

    public IOption<IRovetuType> GetRovetuType<C>(FZOTypeMatch caller)
        where C : IRovetu =>
        new None<IRovetuType>();
}