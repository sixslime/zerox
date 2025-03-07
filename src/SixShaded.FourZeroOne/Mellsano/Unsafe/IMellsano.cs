namespace SixShaded.FourZeroOne.Mellsano.Unsafe;

public interface IMellsano<out R>
    where R : class, Rog
{
    public MellsanoID ID { get; }
    public IUllasem<IKorssa<R>> MatcherUnsafe { get; }
    public IMetaFunctionDefinition<R, Roggi.Unsafe.IMetaFunction<R>> DefinitionUnsafe { get; }
    public IOption<IMellsanossa<R>> TryApply(Kor korssa);
}