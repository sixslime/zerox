namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;
using SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedAction>;
using Core = Core.Syntax.Core;
using u.Constructs;
using Infinite = Syntax.Infinite;

public static class ResolveAction
{
    public static Korvessa<IRoveggi<uPlayableAction>, ResolvedObj> Construct(IKorssa<IRoveggi<uPlayableAction>> action) =>
        new(action)
        {
            Du = Axoi.Korvedu("ResolveAction"),
            Definition =
                (_, iAction) =>
                    Core.kCompose<uResolvedAction>()
                        .kWithRovi(uResolvedAction.ACTION, iAction.kRef())
                        .kWithRovi(uResolvedAction.PLAYER, Infinite.CurrentPlayer)
                        .kWithRovi(
                        Core.Hint<uResolvedAction>(),
                        uResolved.INSTRUCTIONS,
                        iAction.kRef()
                            .kGetRovi(uPlayableAction.STATEMENT)
                            .kExecute())
        };
}