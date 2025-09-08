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
                typeof(Rt.Instructions.MellsanoAdd), SimpleRoggi(new Rm.Instructions.MellsanoAdd())
            },
            {
                typeof(Rt.Instructions.LoadProgramState), SimpleRoggi(new Rm.Instructions.LoadProgramState())
            },
            {
                typeof(Rt.Instructions.Assign<>), (t, c) =>
                    new Rm.Instructions.Assign
                    {
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Bool), SimpleRoggi(new Rm.Bool())
            },
            {
                typeof(Number), SimpleRoggi(new Rm.Number())
            },
            {
                typeof(NumRange), SimpleRoggi(new Rm.NumRange())
            },
            {
                typeof(ProgramState), SimpleRoggi(new Rm.ProgramState())
            },
            {
                typeof(Multi<>), (t, c) =>
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
                typeof(MetaArgs<>), (t, c) =>
                    new Rm.MetaArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(MetaArgs<,>), (t, c) =>
                    new Rm.MetaArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(MetaArgs<,,>), (t, c) =>
                    new Rm.MetaArgs
                    {
                        ArgTypes = t.GenericTypeArguments.Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(MetaFunction<>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = [],
                    }
            },
            {
                typeof(MetaFunction<,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..1].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(MetaFunction<,,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..2].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(MetaFunction<,,,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[3].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..3].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
            {
                typeof(OverflowingMetaFunction<,,,,>), (t, c) =>
                    new Rm.MetaFunction
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[4].TryGetFZOTypeInfo(c).Unwrap(),
                        ArgTypes = t.GenericTypeArguments[..4].Map(x => (RoggiTypeInfo)x.TryGetFZOTypeInfo(c).Unwrap()).ToArray(),
                    }
            },
        };
}