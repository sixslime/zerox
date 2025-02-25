namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

public sealed record Insert<R> : Korssa.Defined.PureFunction<IMemoryObject<R>, R, Roggis.Instructions.Assign<R>>
    where R : class, Rog
{
    public Insert(IKorssa<IMemoryObject<R>> address, IKorssa<R> obj) : base(address, obj) { }
    protected override Roggis.Instructions.Assign<R> EvaluatePure(IMemoryObject<R> in1, R in2) => new() { Address = in1, Subject = in2 };
    protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
}