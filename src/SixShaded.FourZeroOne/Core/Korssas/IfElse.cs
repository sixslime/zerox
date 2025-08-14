namespace SixShaded.FourZeroOne.Core.Korssas;

using FZOSpec;
using Roggis;

public record IfElse<R> : Korssa.Defined.StateImplementedKorssa<Roggis.Bool, MetaFunction<R>, MetaFunction<R>, R>
    where R : class, Rog
{
    public IfElse(IKorssa<Roggis.Bool> condition, IKorssa<MetaFunction<R>> positive, IKorssa<MetaFunction<R>> negative) : base(condition, positive, negative)
    { }

    protected override IOption<EStateImplemented> MakeData(IKorssaContext context, IOption<Roggis.Bool> in1, IOption<MetaFunction<R>> in2, IOption<MetaFunction<R>> in3) =>
        in1.RemapAs(
            condition =>
                condition.IsTrue
                    ? in2.RemapAs(func => func.ConstructMetaExecute())
                    : in3.RemapAs(func => func.ConstructMetaExecute()))
            .Press();
    protected override IOption<string> CustomToString() => $"( if {Arg1} then {(Arg2 is DefineMetaFunction<R> f1 ? f1.Korssa : $"do {Arg2}")} else {(Arg3 is DefineMetaFunction<R> f2 ? f2.Korssa : $"do {Arg3}")} )".AsSome();
}