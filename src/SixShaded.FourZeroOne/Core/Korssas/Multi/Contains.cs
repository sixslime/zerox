namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Contains<R> : Korssa.Defined.Function<IMulti<R>, R, Bool>
    where R : class, Rog
{
    public Contains(IKorssa<IMulti<R>> multi, IKorssa<R> element) : base(multi, element)
    { }

    protected override ITask<IOption<Bool>> Evaluate(IKorssaContext runtime, IOption<IMulti<R>> in1, IOption<R> in2) =>
        in2.RemapAs(
            item =>
                new Bool
                {
                    IsTrue = in1.RemapAs(arr => arr.Elements.Contains(item)).Or(false),
                })
            .ToCompletedITask();
}