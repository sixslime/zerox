namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Ability.Types;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedSourcedAbility>;
public static class ResolveSourcedAbility
{
    public static Korvessa<IRoveggi<uSourcedAbility>, ResolvedObj> Construct(IKorssa<IRoveggi<uSourcedAbility>> ability) =>
        new(ability)
        {
            Du = Axoi.Korvedu("ResolveSourcedAbility"),
            Definition =
                (_, iAbility) =>
                    Core.kSubEnvironment<ResolvedObj>(
                    new()
                    {
                        Environment =
                        [
                            Infinite.AllUnits.kWhere(
                            iPotentialSource =>
                                iAbility.kRef()
                                    .kGetRovi(uSourcedAbility.SOURCE_SELECTOR)
                                    .kExecuteWith(new()
                                    {
                                        A = iPotentialSource.kRef()
                                            .kRead()
                                            .kGetRovi(uUnitData.OWNER)
                                            .kEquals(Infinite.CurrentPlayer)
                                    })
                                    .kExecuteWith(new()
                                    {
                                        A = iPotentialSource.kRef()
                                    }))
                                .kIOSelectOne()
                                .kAsVariable(out var iSourceUnit)
                        ],
                        Value =
                            iSourceUnit.kRef().kKeepNolla(
                            () =>
                                Core.kSubEnvironment<ResolvedObj>(new()
                                {
                                    Environment =
                                        [
                                            iAbility.kRef()
                                                .kGetRovi(uSourcedAbility.TYPE)
                                                .kAsVariable(out var iType),

                                            Infinite.AllUnits.kWhere(
                                            iPotentialTarget =>
                                                iAbility.kRef()
                                                    .kGetRovi(uSourcedAbility.TARGET_SELECTOR)
                                                    .kExecuteWith(new()
                                                    {
                                                        A = Core.kSubEnvironment<Bool>(new()
                                                        {
                                                            Environment =
                                                                [
                                                                    iPotentialTarget.kRef()
                                                                        .kRead()
                                                                        .kGetRovi(uUnitData.OWNER)
                                                                        .kEquals(Infinite.CurrentPlayer)
                                                                        .kAsVariable(out var iMatchesTeam)
                                                                ],
                                                            Value = Core.kSelector<Bool>(new()
                                                            {
                                                                () => iType.kRef()
                                                                    .kIsType<uAttack>()
                                                                    .kKeepNolla(
                                                                    () => iMatchesTeam.kRef().kNot()),
                                                                () => iType.kRef()
                                                                    .kIsType<uDefense>()
                                                                    .kKeepNolla(
                                                                    () => iMatchesTeam.kRef()),
                                                            })
                                                        }),
                                                        B = iSourceUnit.kRef()
                                                            .kRead()
                                                            .kGetRovi(uUnitData.POSITION)
                                                            .
                                                    })
                                        ],
                                    Value =
                                }))
                    })
        };
}