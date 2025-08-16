namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedPositionalMove>;
using Core = Core.Syntax.Core;

public static class ResolvePositionalMove
{
    public static Korvessa<IRoveggi<uPositionalMove>, Multi<ResolvedObj>> Construct(IKorssa<IRoveggi<uPositionalMove>> move) =>
        new(move)
        {
            Du = Axoi.Korvedu("ResolvePositionalMove"),
            Definition =
                (_, iMove) =>
                    Core.kSubEnvironment<Multi<ResolvedObj>>(
                    new()
                    {
                        Environment =
                        [
                            iMove.kRef()
                                .kGetRovi(uMove.SUBJECT_COLLECTOR)
                                .kExecute()
                                .kWhere(
                                iUnit =>
                                    iMove.kRef()
                                        .kGetRovi(uMove.SUBJECT_SELECTOR)
                                        .kExecuteWith(new()
                                        {
                                            A = iMove.kRef()
                                                .kSubjectChecks(iUnit.kRef())
                                        })
                                        .kExecuteWith(new()
                                        {
                                            A = iUnit.kRef()
                                        }))
                                .kIOSelectOne()
                                .kAsVariable(out var iSubjectUnit)
                                    
                        ],
                        Value =
                            iSubjectUnit.kRef()
                                .kKeepNolla(
                                () =>
                                    iMove.kRef()
                                        .kGetRovi(uPositionalMove.MOVE_FUNCTION)
                                        .kExecuteWith(new()
                                        {
                                            A = iSubjectUnit.kRef()
                                        })
                                        .kWhere(
                                        iHex =>
                                            iMove.kRef()
                                                .kGetRovi(uMove.DESTINATION_SELECTOR)
                                                .kExecuteWith(new()
                                                {

                                                })
                    })
        };
}