namespace SixShaded.CoreTypeMatcher;

using Kt = FourZeroOne.Core.Korssas;
using Km = Types.Korssa;
using FZOTypeMatch;

internal static partial class Maps
{
    public static Dictionary<Type, Func<Type, FZOTypeMatch, IKorssaType>> Korssa =>
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
                        MetaArgTypes = [],
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.DefineMetaFunction<,,,,>), (t, c) =>
                    new Km.DefineMetaFunction
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[4].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..4].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.Execute<>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = [],
                    }
            },
            {
                typeof(Kt.Execute<,>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.Execute<,,>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.Execute<,,,>), (t, c) =>
                    new Km.Execute
                    {
                        MetaOutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        MetaArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.ToBoxedArgs<>), (t, c) =>
                    new Km.ToBoxedArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.ToBoxedArgs<,>), (t, c) =>
                    new Km.ToBoxedArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(Kt.ToBoxedArgs<,,>), (t, c) =>
                    new Km.ToBoxedArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
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
                    new Km.Memory.Assign
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        RodaInfoGetter = RodaInfoGetter(c),
                    }
            },
            {
                typeof(Kt.Memory.Reference<>), (t, c) =>
                    new Km.Memory.Reference
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        RodaInfoGetter = RodaInfoGetter(c),
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
                    new Km.Memory.Rovedanggi.AllKeys
                    {
                        RovedantuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Rovedanggi.AllValues<,>), (t, c) =>
                    new Km.Memory.Rovedanggi.AllValues
                    {
                        RovedantuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Rovedanggi.Read<>), (t, c) =>
                    new Km.Memory.Rovedanggi.Read
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Memory.Rovedanggi.Write<>), (t, c) =>
                    new Km.Memory.Rovedanggi.Write
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
                        RovuInfoGetter = RovuInfoGetter(c),
                    }
            },
            {
                typeof(Kt.Rovi.With<,>), (t, c) =>
                    new Km.Rovi.Get
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        RovuInfoGetter = RovuInfoGetter(c),
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.Get<,,>), (t, c) =>
                    new Km.Rovi.Varovi.Get
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c),
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.With<,,>), (t, c) =>
                    new Km.Rovi.Varovi.With
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c),
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.GetKeys<,,>), (t, c) =>
                    new Km.Rovi.Varovi.GetKeys
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c),
                    }
            },
            {
                typeof(Kt.Rovi.Varovi.GetValues<,,>), (t, c) =>
                    new Km.Rovi.Varovi.GetValues
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        VarovuInfoGetter = VarovuInfoGetter(c),
                    }
            },
        };
}