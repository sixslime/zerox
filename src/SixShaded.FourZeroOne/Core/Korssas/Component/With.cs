namespace SixShaded.FourZeroOne.Core.Korssas.Component;

using FZOSpec;
using Roveggi;
using Roveggi.Unsafe;

public sealed record With<C, R> : Korssa.Defined.Korssa<IRoveggi<C>>
    where R : class, Rog
    where C : IRovetu
{
    public With(IKorssa<IRoveggi<C>> holder, IKorssa<R> data) : base(holder, data)
    { }

    public required ISetRovu<C, R> Rovu { get; init; }

    // this is crazy
    protected override IResult<ITask<IOption<IRoveggi<C>>>, EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => x.IsA<IRoveggi<C>>()).Check(out var roveggi))
            ? (Rovu is IAbstractRovu abstr)
                ? Master.ASSEMBLY.RovenData.SetImplementations.TryGetValue(roveggi.GetType().GenericTypeArguments[0], out var setMap)
                    ? setMap.TryGetValue(abstr, out var implementation)
                        ? implementation.ConstructMetaExecute(roveggi.AsSome(), args[1]).AsErr(Hint<ITask<IOption<IRoveggi<C>>>>.HINT)
                        : throw new Exception($"{roveggi.GetType().GenericTypeArguments[0].Name} has no implementation for abstract setrovu {Rovu}?")
                    : throw new Exception($"No assembly mappings exist for rovetu {roveggi.GetType().GenericTypeArguments[0].Name}?")
                : ((args[1].RemapAs(x => (R)x).Check(out var data))
                    ? roveggi.WithComponent((IRovu<C, R>)Rovu, data)
                    : roveggi.WithoutComponents([(IRovu<C, R>)Rovu]))
                .AsSome().ToCompletedITask().AsOk(Hint<EStateImplemented>.HINT)
            : new Ok<ITask<IOption<IRoveggi<C>>>, EStateImplemented>(new None<IRoveggi<C>>().ToCompletedITask());
    protected override IOption<string> CustomToString() => $"({ArgKorssas[0]}~{Rovu}={ArgKorssas[1]})".AsSome();

}