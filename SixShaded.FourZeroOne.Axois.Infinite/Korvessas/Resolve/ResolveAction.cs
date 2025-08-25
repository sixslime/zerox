namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using Rovetus.Constructs.Move;
using Rovetus.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedAction>;
using Core = Core.Syntax.Core;
using u.Constructs;
using Infinite = Syntax.Infinite;

public record ResolveAction(IKorssa<IRoveggi<uPlayableAction>> action) : Korvessa<IRoveggi<uPlayableAction>, ResolvedObj>(action)
{
    protected override RecursiveMetaDefinition<IRoveggi<uPlayableAction>, ResolvedObj> InternalDefinition() =>
        (_, iAction) =>
            Core.kCompose<uResolvedAction>()
                .kWithRovi(uResolvedAction.ACTION, iAction.kRef())
                .kWithRovi(uResolvedAction.PLAYER, Infinite.CurrentPlayer)
                .kWithRovi(
                Core.Hint<uResolvedAction>(),
                uResolved.INSTRUCTIONS,
                iAction.kRef()
                    .kGetRovi(uPlayableAction.STATEMENT)
                    .kExecute());
}
