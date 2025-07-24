﻿namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class UpdateRovi<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction, IRovu<C, R> component) =>
        new(roveggi, updateFunction)
        {
            Du = Axoi.Korvedu("UpdateRovi"),
            CustomData = [component],
            Definition =
                Core.kMetaFunction<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>>(
                [],
                (roveggiM, updateFunctionM) =>
                    roveggiM.kRef()
                        .kWithRovi(
                        component,
                        updateFunctionM.kRef()
                            .kExecuteWith(
                            new()
                            {
                                A = roveggiM.kRef().kGetRovi(component),
                            }))),
        };
}