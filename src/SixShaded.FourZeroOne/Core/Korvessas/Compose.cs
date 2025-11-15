namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;

public record Compose<C>() : Korvessa<IRoveggi<C>>()
    where C : IConcreteRovetu
{
    protected override RecursiveMetaDefinition<IRoveggi<C>> InternalDefinition() =>
        _ => new Korssas.Fixed<IRoveggi<C>>(new Roveggi.Defined.Roveggi<C>());
}
