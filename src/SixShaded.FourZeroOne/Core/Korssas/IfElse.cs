namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record IfElse<R> : Korssa.Defined.Function<Bool, MetaFunction<R>, MetaFunction<R>, MetaFunction<R>>
    where R : class, Rog
{
    public IfElse(IKorssa<Bool> condition, IKorssa<MetaFunction<R>> positive, IKorssa<MetaFunction<R>> negative) : base(condition, positive, negative)
    { }

    protected override ITask<IOption<MetaFunction<R>>> Evaluate(IKorssaContext runtime, IOption<Bool> in1, IOption<MetaFunction<R>> in2, IOption<MetaFunction<R>> in3) => in1.RemapAs(x => x.IsTrue ? in2 : in3).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"({Arg1} ? {Arg2} : {Arg3})".AsSome();
}