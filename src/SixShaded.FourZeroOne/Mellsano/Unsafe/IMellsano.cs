namespace SixShaded.FourZeroOne.Mellsano.Unsafe;

public interface IMellsano<out R>
    where R : class, Rog
{
    public MellsanoID ID { get; }
    public IUllasem<IKorssa<R>> MatcherUnsafe { get; }
    public Roggi.Unsafe.IBoxedMetaFunction<R> DefinitionUnsafe { get; }
    public IOption<IKorssaOfMellsano<R>> TryApply(Kor korssa);
}