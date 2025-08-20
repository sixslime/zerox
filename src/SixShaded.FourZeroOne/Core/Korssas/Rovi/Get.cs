namespace SixShaded.FourZeroOne.Core.Korssas.Rovi;

using SixShaded.FourZeroOne.FZOSpec;
using SixShaded.FourZeroOne.Roveggi;
using SixShaded.FourZeroOne.Roveggi.Unsafe;

public sealed record Get<C, R> : Korssa.Defined.Korssa<R>
    where R : class, Rog
    where C : IRovetu
{
    public Get(IKorssa<IRoveggi<C>> holder) : base(holder)
    { }

    public required IGetRovu<C, R> Rovu { get; init; }
    
    // this is crazy
    protected override IResult<ITask<IOption<R>>, EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => x.IsA<IRoveggi<C>>()).Check(out var roveggi))
            ? (Rovu is IAbstractRovu abstr)
                ? Master.ASSEMBLY.RovenData.GetImplementations.TryGetValue(roveggi.GetType().GenericTypeArguments[0], out var getMap)
                    ? getMap.TryGetValue(abstr, out var implementation)
                        ? implementation.ConstructMetaExecute(roveggi.AsSome()).AsErr(Hint<ITask<IOption<R>>>.HINT)
                        : throw new Exception($"{roveggi.GetType().GenericTypeArguments[0].Name} has no implementation for abstract getrovu {Rovu}?")
                    : throw new Exception($"No assembly mappings exist for rovetu {roveggi.GetType().GenericTypeArguments[0].Name}?")
                : roveggi.GetComponent(Rovu.IsA<IRovu<C, R>>()).ToCompletedITask().AsOk(Hint<EStateImplemented>.HINT)
            : new Ok<ITask<IOption<R>>, EStateImplemented>(new None<R>().ToCompletedITask());
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}->{Rovu}".AsSome();
}