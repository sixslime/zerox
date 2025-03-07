namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record IfElse<R> : Korssa.Defined.Function<Bool, Roggis.MetaFunction<R>, Roggis.MetaFunction<R>, Roggis.MetaFunction<R>> where R : class, Rog
{
    public IfElse(IKorssa<Bool> condition, IKorssa<Roggis.MetaFunction<R>> positive, IKorssa<Roggis.MetaFunction<R>> negative) : base(condition, positive, negative) { }
    protected override ITask<IOption<Roggis.MetaFunction<R>>> Evaluate(IKorssaContext runtime, IOption<Bool> in1, IOption<Roggis.MetaFunction<R>> in2, IOption<Roggis.MetaFunction<R>> in3) => in1.RemapAs(x => x.IsTrue ? in2 : in3).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"if {Arg1} then {Arg2} else {Arg3}".AsSome();
}