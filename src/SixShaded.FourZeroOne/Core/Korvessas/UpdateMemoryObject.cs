namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class UpdateMemoryObject<R>
    where R : class, Rog
{
    public static Korvessa<IMemoryObject<R>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> Construct(IKorssa<IMemoryObject<R>> memObj, IKorssa<MetaFunction<R, R>> updateFunction) =>
        new(memObj, updateFunction)
        {
            Du = Axoi.Korvedu("updatememobj"),
            Definition =
                Core.tMetaFunction<IMemoryObject<R>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>>(
                    (memObjI, updateFunctionI) =>
                        memObjI.tRef()
                            .tWrite(
                            updateFunctionI.tRef()
                                .tExecuteWith(
                                new()
                                {
                                    A = memObjI.tRef().tGet(),
                                })))
                    .Roggi,
        };
}