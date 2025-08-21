namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Korvessas.Resolve;
using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using u.Constructs;
using u.Constructs.Resolved;
using u.Constructs.Move;
partial class KorssaSyntax
{
    public static Korvessa<IRoveggi<uAbility>, IRoveggi<uResolvedAbility>> kAbstractResolve(this IKorssa<IRoveggi<uAbility>> ability) => ResolveAbility.Construct(ability);
    public static Korvessa<IRoveggi<uMove>, IMulti<IRoveggi<uResolvedMove>>> kAbstractResolve(this IKorssa<IRoveggi<uMove>> move) => ResolveMove.Construct(move);
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uResolvedSourcedAbility>> kResolve(this IKorssa<IRoveggi<uSourcedAbility>> ability) => ResolveSourcedAbility.Construct(ability);
    public static Korvessa<IRoveggi<uUnsourcedAbility>, IRoveggi<uResolvedUnsourcedAbility>> kResolve(this IKorssa<IRoveggi<uUnsourcedAbility>> ability) => ResolveUnsourcedAbility.Construct(ability);
    public static Korvessa<IRoveggi<uPositionalMove>, IRoveggi<uResolvedPositionalMove>> kResolve(this IKorssa<IRoveggi<uPositionalMove>> move) => ResolvePositionalMove.Construct(move);
    public static Korvessa<IRoveggi<uNumericalMove>, Multi<IRoveggi<uResolvedNumericalMove>>> kResolve(this IKorssa<IRoveggi<uNumericalMove>> move) => ResolveNumericalMove.Construct(move);
    public static Korvessa<IRoveggi<uPlayableAction>, IRoveggi<uResolvedAction>> kResolve(this IKorssa<IRoveggi<uPlayableAction>> action) => ResolveAction.Construct(action);
}