namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using SixShaded.FourZeroOne.Roveggi;

public static class UpdateMemoryObject<R>
    where R : class, Rog
{
    public static Korvessa<IRoveggi<IMemoryRovetu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> Construct(IKorssa<IRoveggi<IMemoryRovetu<R>>> memObj, IKorssa<MetaFunction<R, R>> updateFunction) =>
        new(memObj, updateFunction)
        {
            Du = Axoi.Korvedu("updatememobj"),
            Definition =
                Core.kMetaFunction<IRoveggi<IMemoryRovetu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>>(
                [],
                (memObjI, updateFunctionI) =>
                    memObjI.kRef()
                        .kWrite(
                        updateFunctionI.kRef()
                            .kExecuteWith(
                            new()
                            {
                                A = memObjI.kRef().kGet(),
                            }))),
        };
}