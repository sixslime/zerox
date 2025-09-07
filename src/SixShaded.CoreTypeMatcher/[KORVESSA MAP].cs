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
        };
}