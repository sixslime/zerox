﻿namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

using Roveggi;

public sealed record Insert<R> : Korssa.Defined.Function<IRoveggi<Rovedantu<R>>, R, Roggis.Instructions.Assign<R>>
    where R : class, Rog
{
    public Insert(IKorssa<IRoveggi<Rovedantu<R>>> address, IKorssa<R> obj) : base(address, obj)
    { }

    protected override ITask<IOption<Roggis.Instructions.Assign<R>>> Evaluate(IKorssaContext runtime, IOption<IRoveggi<Rovedantu<R>>> in1, IOption<R> in2) =>
        in1.RemapAs(
            addressObject =>
                new Roggis.Instructions.Assign<R>
                {
                    Address = addressObject.Rovedanggi(),
                    Data = in2
                })
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
}