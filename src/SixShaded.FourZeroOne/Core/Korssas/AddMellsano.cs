namespace SixShaded.FourZeroOne.Core.Korssas;

public sealed record AddMellsano : Korssa.Defined.PureValue<Roggis.Instructions.MellsanoAdd>
{
    public readonly Mellsano.Unsafe.IMellsano<Rog> Mellsano;

    public AddMellsano(Mellsano.Unsafe.IMellsano<Rog> mellsano)
    {
        Mellsano = mellsano;
    }

    protected override Roggis.Instructions.MellsanoAdd EvaluatePure() =>
        new()
        {
            Mellsano = Mellsano,
        };
}