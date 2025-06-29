namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis.Instructions;

public sealed record AddMellsano : Korssa.Defined.RegularKorssa<MellsanoAdd>
{
    public required Mel Mellsano { get; init; }

    protected override ITask<IOption<MellsanoAdd>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        new MellsanoAdd
            {
                Mellsano = Mellsano
            }.AsSome()
            .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{Mellsano}".AsSome();
}