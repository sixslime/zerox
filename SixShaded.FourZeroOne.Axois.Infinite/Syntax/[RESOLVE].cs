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
    public static ResolveAbility kAbstractResolve(this IKorssa<IRoveggi<uAbility>> ability) => new(ability);
    public static ResolveMove kAbstractResolve(this IKorssa<IRoveggi<uMove>> move) => new(move);
    public static ResolveSourcedAbility kResolve(this IKorssa<IRoveggi<uSourcedAbility>> ability) => new(ability);
    public static ResolveUnsourcedAbility kResolve(this IKorssa<IRoveggi<uUnsourcedAbility>> ability) => new(ability);
    public static ResolvePositionalMove kResolve(this IKorssa<IRoveggi<uPositionalMove>> move) => new(move);
    public static ResolveNumericalMove kResolve(this IKorssa<IRoveggi<uNumericalMove>> move) => new(move);
    public static ResolveAction kResolve(this IKorssa<IRoveggi<uPlayableAction>> action) => new(action);
}