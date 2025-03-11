namespace SixShaded.FourZeroOne.Mellsano.Defined;

public record Mellsanossa<R> : Korssa.Defined.StateImplementedKorssa<R>, Unsafe.IMellsanossa<R>
    where R : class, Rog
{
    // [0] is always self/original proxy, rest are arg proxies in-order.
    public required IProxy<Rog>[] Proxies { get; init; }

    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext context) =>
        AppliedMellsano.DefinitionUnsafe
                .ConstructConcreteMetaFunction(context.CurrentMemory)
                .ConstructMetaExecute(Proxies.Map(x => x.AsSome()).ToArray<RogOpt>()) with
            {
                MellsanoMutes = [AppliedMellsano.ID],
            };

    public required Unsafe.IMellsano<R> AppliedMellsano { get; init; }
}