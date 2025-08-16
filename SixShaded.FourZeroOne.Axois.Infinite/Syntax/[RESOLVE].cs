namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Korvessas.Resolve;
using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using u.Constructs;
using u.Constructs.Resolved;
partial class KorssaSyntax
{
    public static Korvessa<IRoveggi<uAbility>, IRoveggi<uResolvedAbility>> kAbstractResolve(this IKorssa<IRoveggi<uAbility>> ability) => ResolveAbility.Construct(ability);
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uResolvedSourcedAbility>> kResolve(this IKorssa<IRoveggi<uSourcedAbility>> ability) => ResolveSourcedAbility.Construct(ability);
    public static Korvessa<IRoveggi<uUnsourcedAbility>, IRoveggi<uResolvedUnsourcedAbility>> kResolve(this IKorssa<IRoveggi<uUnsourcedAbility>> ability) => ResolveUnsourcedAbility.Construct(ability);
}