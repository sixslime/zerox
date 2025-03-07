namespace SixShaded.FourZeroOne.Mellsano.Unsafe;

public interface IMellsanossa<out R> : IKorssa<R>
    where R : class, Rog
{
    public IMellsano<R> AppliedMellsano { get; }
}