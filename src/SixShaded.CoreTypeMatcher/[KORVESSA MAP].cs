namespace SixShaded.CoreTypeMatcher;

using Kt = FourZeroOne.Core.Korvessas;
using Km = Types.Korvessa;
using FZOTypeMatch;
internal static partial class Maps
{
    public static Dictionary<Type, Func<Type, FZOTypeMatch, IKorssaType>> Korvessa =>
        new()
        {
            {
                typeof(Kt.Compose<>), (t, c) =>
                    new Km.Compose
                    {
                        RovetuType = (RovetuTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.CatchNolla<>), (t, c) =>
                    new Km.CatchNolla()
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.KeepNolla<>), (t, c) =>
                    new Km.KeepNolla()
                    {
                        RoggiType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Switch<>), (t, c) =>
                    new Km.Switch()
                    {
                        OutputType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Update.UpdateRovi<,>), (t, c) =>
                    new Km.Update.UpdateRovi()
                    {
                        RovuInfoGetter = RovuInfoGetter(c),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Update.SafeUpdateRovi<,>), (t, c) =>
                    new Km.Update.SafeUpdateRovi()
                    {
                        RovuInfoGetter = RovuInfoGetter(c),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Update.UpdateVarovi<,,>), (t, c) =>
                    new Km.Update.UpdateVarovi()
                    {
                        VarovuInfoGetter = VarovuInfoGetter(c),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Update.SafeUpdateVarovi<,,>), (t, c) =>
                    new Km.Update.SafeUpdateVarovi()
                    {
                        VarovuInfoGetter = VarovuInfoGetter(c),
                        KeyType = (RoggiTypeInfo)t.GenericTypeArguments[1].TryGetFZOTypeInfo(c).Unwrap(),
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[2].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Update.UpdateRovedanggi<>), (t, c) =>
                    new Km.Update.UpdateRovedanggi()
                    {
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            {
                typeof(Kt.Update.SafeUpdateRovedanggi<>), (t, c) =>
                    new Km.Update.SafeUpdateRovedanggi()
                    {
                        DataType = (RoggiTypeInfo)t.GenericTypeArguments[0].TryGetFZOTypeInfo(c).Unwrap(),
                    }
            },
            { typeof(Kt.Number.Clamp), SimpleKorssa(new Km.Number.Clamp()) },
            { typeof(Kt.Number.Min), SimpleKorssa(new Km.Number.Min()) },
            { typeof(Kt.Number.Max), SimpleKorssa(new Km.Number.Max()) },
            { typeof(Kt.Number.SingleRange), SimpleKorssa(new Km.Number.SingleRange()) },
        };
}