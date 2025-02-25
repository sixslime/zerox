namespace SixShaded.FourZeroOne.Mellsano.Defined;

public record KorssaOfMellsano<R> : Korssa.Defined.RuntimeHandledValue<R>, Unsafe.IKorssaOfMellsano<R>
    where R : class, Rog
{
    // [0] is always self/original proxy, rest are arg proxies in-order.
    public required IProxy<Rog>[] Proxies { get; init; }
    public required Unsafe.IMellsano<R> AppliedMellsano { get; init; }

    protected override FZOSpec.EStateImplemented MakeData()
    {
        var definition = AppliedMellsano.DefinitionUnsafe;
        return new FZOSpec.EStateImplemented.MetaExecute
        {
            Korssa = definition.Korssa,
            ObjectWrites =
                definition.SelfIdentifier.IsA<IMemoryAddress<Rog>>().Yield()
                    .Concat(definition.ArgAddresses)
                    .ZipShort(
                        definition.IsA<Rog>().Yield()
                            .Concat(Proxies)
                            .Map(x => x.AsSome()))
                    .Tipled(),
            MellsanoMutes = [AppliedMellsano.ID],
        };
    }
}